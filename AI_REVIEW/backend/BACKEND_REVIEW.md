# Digivice — Backend Review

> **Data da análise original:** 22 de julho de 2026  
> **Última atualização:** agosto 2026 (pós-triagem dos 5 tópicos do sumário)  
> **Escopo:** apenas `Backend/` (e cobertura cruzada com `Tests/` e `AI/*.md` como balizadores)  
> **Referências de “certo/errado”:** `AI/CODE_RULES.md`, `AI/BUSINESS_RULES.md`  
> **Métricas aproximadas:** ~207 arquivos `.cs` no Backend (~4.4k LOC), ~95 arquivos de teste (~6.3k LOC, ~277 Fact/Theory), 22 JSONs de definição de memória

---

## 1. Sumário executivo

O Backend do Digivice é um serviço ASP.NET Core (.NET 10) **local, Windows-only**, que faz polling da memória compartilhada do emulador DuckStation a cada ~1s, monta um `State` de domínio, calcula diffs e empurra apenas deltas via SignalR para o frontend Tauri/Vue.

A arquitetura é **clara, previsível e altamente consistente**: fatias verticais repetem o mesmo pipeline (`Addresses → Reader → Resource → Assembler → Model → Converter/Differ → DTO → Event`). O investimento em testes é acima da média para o tamanho do código. As regras de estilo do `CODE_RULES.md` estão bem aplicadas no código recente (Assemblers, Loaders, Providers, Infrastructure).

### Status da triagem (agosto 2026)

Os cinco riscos do sumário original foram revisados:

**Feito**
- **Operabilidade** — Serilog lê `appsettings` (`Warning` em produção / sidecar; `Information` em Development via `appsettings.Development.json` + `launchSettings`).
- **Cascade da main quest** — `NormalizeMainQuestProgression` documentado em `AI/BUSINESS_RULES.md` §2.3.
- **CORS** — allowlist explícita (`Uri.TryCreate` + `IsLoopback` + origins Tauri), sem `Contains("tauri")`.
- **Fail-soft** — `Log.Warning` nos fallbacks de `MemoryBlockReader` e converters hex; continua retornando `0` (sem throw).

**Adiado (baixa prioridade / redesign futuro)**
- **Concorrência** — race Hub × `GameStateStore`; sintomas sobretudo em connect/reconnect (F5). Redesign da conexão previsto depois.
- **“Confiar na RAM” / invariantes fortes** — Party ≥1 e Digievolution filled→empty: Backend permanece espelho; pré-load do jogo pode gerar estados estranhos. Doc B/F/G e guards no servidor não priorizados agora.
- **Party tipada com tamanho 3** — contagem já vem de `PartyAddresses.json` (3 slots); ROI baixo.

O backlog restante (higiene / evolução) está em **§11.3**, ordenado do mais fácil ao mais difícil.

**Nota geral sugerida: B+ / 8.0** — arquitetura sólida e bem testada; ops/CORS/fail-soft melhoraram após a triagem; gaps restantes são sobretudo estilo, boilerplate e decisões de produto adiadas.

---

## 2. Metodologia

A análise cruzou:

| Fonte | Uso |
|-------|-----|
| `AI/BUSINESS_RULES.md` | Invariantes de domínio e fluxo esperado |
| `AI/CODE_RULES.md` (seções Backend + Tests) | Padrões de estilo e qualidade |
| Código em `Backend/**` | Arquitetura, segurança, cheiros, outliers |
| `Tests/**` | Cobertura, gaps, testes que documentam comportamento |
| `appsettings.json` / `.csproj` | Config, dependências, superfície de deploy |

Dimensões avaliadas: arquitetura, regras de negócio, qualidade/estilo, segurança, concorrência, observabilidade, testes, manutenibilidade e extensibilidade.

---

## 3. Visão arquitetural

### 3.1. Mapa de camadas

```
Program.cs                     → bootstrap ASP.NET Core + CORS + SignalR
Infrastructure/                → DI, processos Windows, MemoryMappedFile, DuckStation
Memory/                        → endereços JSON, readers, resources, repositório
Domain/                        → models + assemblers (puros)
Application/                   → loaders, providers, StateComposer, GameLoopService
Events/                        → DTOs (Optional<T>), converters, diffs, factories, hub, store
Diagnostics/                   → console ANSI (Feature flag)
```

### 3.2. Pipeline de dados (conforme BUSINESS_RULES)

```
DuckStation (MMF)
  → IMemoryReader / MemoryBlockReader
  → *Reader → *Resource
  → *Loader (orquestra)
  → *Assembler → Domain Model
  → StateComposer → State
  → StateEventFactory + *Differ → Event(s) com DTO patch
  → EventDispatcherService → SignalR (/gamehub)
  → GameStateStore (estado anterior)
```

Isso **bate** com o diagrama do `BUSINESS_RULES.md` §1. O loop está em `GameLoopService`, intervalo default `1000ms` (`GameLoop:PollingIntervalMs`).

### 3.3. Fatia vertical típica

Cada entidade (Player, Digimon, Quest, Auction…) segue o mesmo “stack” de 6–7 artefatos. Isso torna o projeto excelente para skills/IA e onboarding por cópia de padrão — e também é a principal fonte de **boilerplate** (Differs/Converters quase espelhados).

### 3.4. Pontos fortes estruturais

- Separação nítida entre **leitura bruta** (Memory), **higiene de domínio** (Assemblers) e **protocolo de eventos** (Events).
- Domínio sem dependência de ASP.NET/SignalR/IO (assemblers estáticos).
- Diff incremental com `Optional<T>` + `JsonIgnore(WhenWritingDefault)` — contrato limpo com o frontend (“undefined = não mexer”).
- `InitialState` no connect (`GameHub.OnConnectedAsync`) + diffs depois — alinhado à regra de ouro do syncer.
- Arquivos pequenos (~21 LOC médios); quase nenhum arquivo > 130 linhas.
- DI centralizado em um único `AddBackendServices`.
- Sem ORM/DB — correto para um tracker de RAM em processo.

---

## 4. Regras de negócio (`AI/BUSINESS_RULES.md`)

### 4.1. Quadro de conformidade

| Regra | Status no Backend | Notas |
|-------|-------------------|--------|
| Loop ~1000ms via `GameLoopService` | ✅ | Default + override por config |
| Assemblers → State → Diff → SignalR | ✅ | Pipeline completo |
| Party: pairing ocupado/vazio | ✅ | `PartyLoader` garante ambos null ou ambos preenchidos |
| Party: exatamente 3 slots | ⚠️ | Implícito via `PartyAddresses.json`; sem assert |
| Party: ≥1 slot ocupado | ❌ (adiado) | Aceito all-empty de propósito (espelho / pré-load); ver §11.2 |
| `digimonId` no slot, não no Digimon | ✅ | Modelo correto |
| Digievolution: proibido filled→empty | ❌ (adiado) | Pass-through da RAM; sem guard; ver §11.2 |
| Level 1–99 (sem clamp) | ✅ | Correto *não* clampar (doc diz que é redundante) |
| Blast por Digimon | ✅ | Endereços +2 por rookie; model no Digimon |
| Journal: só `Value` dinâmico | ⚠️ | Quase; cascade da main quest documentado |
| Journal: set fixo / InitialState / sem flag de conclusão | ✅ | |
| Player name não rastreado | ✅ | `0x00048D88` ausente do Backend |

### 4.2. Detalhamento dos gaps

#### Party — “sempre 3 slots” e “pelo menos 1 ocupado”

`PartyAssembler` mapeia 1:1 o que veio do JSON/resource. Não há validação de `Count == 3`. A invariante “≥1 ocupado” **não é tratada como erro crítico** no Backend: `PartyLoaderTests.Load_ShouldReturnAllEmptySlots_WhenAllSlotsAreEmpty` documenta all-empty como comportamento esperado, e `DebugConsoleRenderer` até renderiza “No Digimons detected…”.

**Insight:** o Backend trata Party como espelho tolerante da RAM + normalização de pairing; as regras mais duras do doc estão mais no nível “contrato esperado do jogo + frontend”, não como asserts de domínio no servidor. **Decisão (ago/2026):** não endurecer no Backend agora — pré-load do jogo / F5; ver §11.2.

#### Digievolution filled → empty

`DigievolutionSlotReader` define vazio como `digievolutionId <= 0`. O Differ emite delta se o ID mudou — inclusive para `null`. Não há comparação com o estado anterior no assembler/loader para rejeitar “esvaziamento”.

**Risco prático:** save-state, frame intermediário ou glitch de leitura poderia empurrar filled→empty ao frontend. A regra de negócio assume que a gameplay nunca faz isso; o Backend não defende. **Decisão (ago/2026):** adiado — permanece pass-through; ver §11.2.

#### `NormalizeMainQuestProgression` — desvio consciente (**documentado**)

Em `JournalAssembler`:

```csharp
// Se o próximo step > 0 e o atual == 0 → força atual = 1
```

- Só na **main quest** (side/legendary/DRI não recebem).
- **Muta** `Step.Value` depois da montagem — não é pass-through puro da RAM.
- Tem testes dedicados (`JournalAssemblerTests`).
- **Documentado** em `AI/BUSINESS_RULES.md` §2.3 (cascade + escopo só main quest).

#### Blast — doc vs código

`BUSINESS_RULES` e o código usam `ReadInt16` com stride de 2 bytes (`0x42B74`, `76`, `78`…). Manter o MD alinhado ao `Blast` / `BlastAddress` do Digimon.

#### Auction no Journal

Auction está corretamente sob `Journal.Auctions` (não orphaned). O `AuctionLoader` fica flat em `Application/Loaders/` enquanto `QuestLoader`/`DigimonLoader` estão em subpastas — inconsistência de pasta, não de wiring.

### 4.3. Filosofia de validação: “trust the RAM”

Sanitização ativa só em:

1. **PartyLoader** — pairing DigimonId/DigimonResource (+ ID desconhecido → slot vazio, sem log).
2. **JournalAssembler** — cascade da main quest.
3. **DigimonAssembler** — `ActiveDigievolutionId` `0`/`0xFFFF` → `0`.

Todo o resto (level, HP/MP, blast, digievolution empty/filled, contagem de slots) é **pass-through**. Isso é coerente com a orientação da doc para level, mas **diverge** do tom de “inconsistência crítica” usado para Party/Digievolution.

---

## 5. Qualidade de código vs `AI/CODE_RULES.md`

### 5.1. Scorecard

| Regra | Nota | Situação |
|-------|------|----------|
| Construtores primários | A | Quase 100%; exceção justificável em `MemoryReadException` |
| Um tipo por arquivo | A | `OptionalJsonConverter` / Factory separados (**feito**) |
| Collection expressions `[ ]` / `[..]` | A- | Differs/`StateEventFactory` com `List<T> = []`; Converters mantêm `.ToList()` por `Optional<List<T>>` (**feito**) |
| Usings limpos | A | Spot-check limpo |
| Readers/Converters stateless | A | Sem estado mutável de instância |
| Sem prefixo `_` | A | `_firstRender` → `FirstRender` (**feito**) |
| `new();` | A | Sem violações |
| Ordem de membros (privados → públicos) | A | `SafeDispatch` reordenado (**feito**) |
| Tests: corner cases | A- | Infra bem coberta; gap pequeno em fallback de hex list |

### 5.2. Violações concretas (estilo)

1. ~~`DebugConsoleRenderer.cs` — `private bool _firstRender`~~ (**feito** → `FirstRender`).
2. ~~`EventDispatcherService.cs` — método privado após públicos~~ (**feito**).
3. ~~`OptionalJsonConverter.cs` — dois tipos no mesmo arquivo~~ (**feito** → `OptionalJsonConverterFactory.cs`).
4. **`AddressesRepository.cs`** — `private readonly string dataDirectory = dataDirectory;` redundante com primary ctor (cheiro adjacente à regra 6).
5. ~~Converters/Diffing — `new List<T>()`~~ (**feito** nos Differs/`StateEventFactory` via `List<T> x = []`). Converters **mantêm** `.ToList()` ao atribuir em `Optional<List<T>>` — collection expression exigiria cast `(List<T>)[.. ]`, pior que `.ToList()`.

### 5.3. Outliers de padrão de projeto (além do CODE_RULES)

| Outlier | Detalhe |
|---------|---------|
| `QuestLoader` / `DigimonLoader` sem interface | ~~exceção~~ → `IQuestLoader` / `IDigimonLoader` (**feito**) |
| Logging híbrido | Serviços DI usam `ILogger<T>` (**feito** em GameLoop/MemoryReader/Duckstation); hex converters + `MemoryBlockReader` + `Program` mantêm `Serilog.Log` (stateless/bootstrap) |
| Typos na camada Memory | `Wisdow` / `Equipaments` nos Addresses/Resources/JSON; Domain corrige (`Wisdom`, `Equipments`) no Assembler — fronteira OK, mas confunde busca/refator |
| Arquivo ≠ tipo | ~~`AuctionEntryAddresses.cs`~~ → `AuctionAddresses.cs` (**feito**) |
| Pasta vazia | ~~`Domain/Shared/`~~ — já inexistente (**feito**) |
| `Event.Payload` como `object` | Perde tipagem; consumers fazem cast |
| `IDTO` marker vazio | Só constraint genérica; sem contrato real |
| Equals/GetHashCode manuais em records com `List<T>` | Necessário e bem feito; risco se um model novo esquecer (diff silencioso) |

### 5.4. Cheiros e bugs de qualidade

#### Logging via appsettings (**feito** na triagem)

Serilog usa `ReadFrom.Configuration`: default `Warning` em `appsettings.json`; `Information` em Development (`appsettings.Development.json` + `launchSettings`). Sidecar/produção permanece quieto; lifecycle Info aparece no `dotnet run` em Dev.

#### `MemoryBlockReader` fail-soft vs `MemoryReader` fail-loud (**Warnings feitos**)

- `MemoryReader`: exceção → `MemoryReadException` → game loop limpa sessão.
- `MemoryBlockReader`: offset inválido / catch → **retorna 0** e agora emite `Log.Warning`.

Comportamento de valor inalterado (espelho tolerante); diagnóstico melhorou.

#### Converters JSON de endereço (**Warnings feitos**)

`HexStringToLongConverter`, `HexOrIntStringToIntConverter`, `HexStringListToLongListConverter` ainda fazem fallback `0` / skip em input malformado ou vazio, mas o `catch` de parse inválido loga `Warning`. Typo em `*Addresses.json` ≠ crash; = tracker errado **com** trilha no log.

#### `Features:Debugging` Dev vs Release (**feito**)

`appsettings.json` → `Debugging: false` (sidecar/prod). `appsettings.Development.json` → `true` (monitor ANSI no `dotnet run` / IDE).

#### Exit code em falha fatal (**feito**)

`Program.cs` catch top-level seta `Environment.ExitCode = 1` após `Log.Fatal`, para o sidecar Tauri detectar crash (`code != 0`).

#### Dead code (**feito**)

~~`player.Bits.ToString(...) ?? "Unknown"`~~ — `?? "Unknown"` removido; `Bits` é `int` e `ToString` nunca é null.

---

## 6. Segurança

Contexto: ferramenta **single-user, loopback**. O risco absoluto é baixo; os padrões abaixo importam se o bind/URL mudar ou se o app for embutido de forma mais exposta.

### 6.1. Achados

| # | Achado | Severidade (contexto local) | Severidade (se bind público) |
|---|--------|----------------------------|------------------------------|
| 1 | CORS substring `"tauri"` (**corrigido** → allowlist) | — | — |
| 2 | Hub sem autenticação (`/gamehub` aberto) | Aceitável no loopback | Alta |
| 3 | `new Uri(origin)` sem try/catch (**mitigado** com `TryCreate`) | — | — |
| 4 | HTTP sem TLS em `127.0.0.1:5000` | OK para local | N/A se permanecer loopback |
| 5 | MMF `duckstation_{pid}` sem validação de conteúdo | Inerente ao propósito | — |
| 6 | `AllowedHosts: "*"` | Irrelevante no loopback | Revisar se expandir |

### 6.2. Comentário sobre CORS (**feito**)

Política atual: `Uri.TryCreate` + `IsLoopback` + `HashSet` de origins Tauri (`http://tauri.localhost`, `https://tauri.localhost`, `tauri://localhost`) + `AllowCredentials` para SignalR. Hub aberto e `AllowedHosts: "*"` seguem aceitos enquanto o bind for loopback.

### 6.3. Superfície de memória

Ler MMF do DuckStation é o core do produto. Não há elevação especial além do que o OS exige para abrir o mapping. Risco de “processo malicioso criando mapping com nome previsível” exige já ter código local — mencionar só por completude.

---

## 7. Concorrência e estado compartilhado

> **Status (ago/2026): adiado** — ver §11.2. Sintomas palpáveis sobretudo em connect/reconnect; redesign da conexão previsto no futuro.

### 7.1. `GameStateStore` sem sincronização

Escritores/leitores:

| Quem | O quê |
|------|--------|
| `GameLoopService` (BackgroundService) | lê `CurrentState`, chama `UpdateState`, passa store ao `ConnectionEventFactory` |
| `ConnectionEventFactory` | muta `IsConnectedWithEmulator`, `ClearState()`, error fields |
| `EventDispatcherService` / `GameHub` (threads SignalR) | lê `CurrentState` e flags de conexão no connect |

**Não há** `lock`, `Interlocked`, `volatile`, nem imutabilidade defensiva no store.

Cenário plausível: cliente conecta no meio de um tick; `DispatchInitialStateToClient` lê um `State` enquanto outro caminho chama `ClearState()` → InitialState nulo + status inconsistente, ou referência a grafo sendo observado enquanto outro tick já avançou (mitigado em parte porque `State` é substituído por referência, não mutado in-place — mas `ClearState` + flags ainda racing).

**Risco:** baixo volume (1 cliente típico, 1 Hz), mas é race real. Correção leve: snapshot imutável + lock curto, ou channel single-reader.

### 7.2. Dispatch fire-and-forget

`SafeDispatch` faz `SendAsync(...).ContinueWith(...)` sem await e sem backpressure. Erros são logados; sob muitos clients/diffs grandes, tasks podem acumular entre ticks. Aceitável hoje; frágil se o produto escalar.

### 7.3. Readers stateless — positivo

A regra de Readers/Converters sem estado mutável **é seguida** e ajuda thread-safety no pipeline puro. O problema está concentrado no store + dispatch, não nos readers.

---

## 8. Observabilidade e diagnóstico

| Aspecto | Avaliação |
|---------|-----------|
| Serilog presente | Sim — config por ambiente (Warning prod / Information Dev) |
| Correlation / structured fields | Parcial (templates bons em alguns Error) |
| Métricas | Ausentes (ok para o escopo) |
| Debug console | Útil; gated por feature + `!Console.IsOutputRedirected` |
| Erros de conexão tipados | Bom — `EmulatorConnectionErrorCodes` cobrem Config/Process/Mapping/Connection/MemoryRead/StateCompose |
| Falhas soft de bloco/hex | **Warning** no fallback (triagem); valor ainda `0` |

**Pós-triagem:** ops + fail-soft + `Features:Debugging` + `ILogger<T>` nos serviços DI principais. Converters/block reader seguem com Serilog estático de propósito.

---

## 9. Testes (`Tests/` + CODE_RULES Tests)

### 9.1. Panorama

- Cobertura ampla espelhando namespaces do Backend (Unit + Integration).
- Destaques: `GameLoopServiceTests`, `DuckstationConnectorTests`, `DependencyInjectionTests`, converters/differs/assemblers quase 1:1.
- Ratio testes/código alto — sinal de maturidade.

### 9.2. Gaps relevantes

| Gap | Por quê importa |
|-----|-----------------|
| Fallback malformado em `HexStringListToLongListConverter` | Caminho `catch → Add(0)` sem teste |
| Sem assert de race no `GameStateStore` | Concorrência Hub × Loop |
| Sem teste de política CORS | Segurança |
| Party all-empty tratado como OK nos testes | Documenta **ausência** da invariante BUSINESS_RULES |
| Digievolution filled→empty | Sem teste de “não deveria emitir / deveria logar” |
| `DebugConsoleRenderer` | Sem testes (aceitável) |

### 9.3. Conformidade com “dupla verificação de corner cases”

Infraestrutura (MemoryReader, DuckstationConnector, AddressesRepository, hex converters simples) **cumpre bem** a regra. O gap do hex-list é pontual. Em domínio, os testes reforçam o comportamento *atual* (incluindo all-empty e cascade só na main quest), o que é bom — mas também cristaliza gaps de regra de negócio em “spec viva”.

---

## 10. Manutenibilidade e extensibilidade

### 10.1. O que escala bem

- Adicionar campo em Player/Digimon/Quest é mecânico (skills do repo existem para isso).
- JSON de endereços desacopla “onde está na RAM” do C#.
- Diff/`Optional<T>` evita reescrever protocolo a cada campo.
- Arquivos pequenos + naming previsível.

### 10.2. O que escala mal / dívida

1. **`AddressesRepository` monolítico** — um campo privado + getter por JSON de quest/DRI/weapon. Adicionar agente = editar DI-facing repository com ~10 linhas boilerplate. Candidato a descoberta por pasta (`Directory.EnumerateFiles`) com convenção de path, mantendo cache.
2. **Boilerplate Differ** — `DigimonDiffer` é o arquivo mais longo do Events; padrão `bool xChanged` + `dto with { }` se repete. Geração parcial ou helper `SetIfChanged` reduziria ruído (sem mudar comportamento).
3. **Equals manual em records com listas** — correto, mas frágil; um helper compartilhado ou collections imutáveis (`EquatableList` / `ImmutableArray`) reduziria risco de esquecimento.
4. **Typos Memory vs Domain** — manter mapping explícito no Assembler está ok; idealmente renomear JSON/Addresses para `Wisdom`/`Equipments` em migração única (quebra defs — coordenar).
5. **Dois idiomas de logging** — padronizar `ILogger<T>` facilita testes e níveis.

### 10.3. Dependências

Só Serilog (+ AspNetCore/Console). Superfície mínima — positivo para um sidecar. Sem pacotes de segurança extras necessários no contexto atual.

---

## 11. Achados — status pós-triagem

### 11.1 Feito

| ID original | Achado | Resolução |
|-------------|--------|-----------|
| P0-1 | Serilog / appsettings | `ReadFrom.Configuration`; Warning prod, Information Dev |
| P0-2 | Documentar `NormalizeMainQuestProgression` | `AI/BUSINESS_RULES.md` §2.3 |
| P1-2 | CORS substring `"tauri"` | Allowlist + `IsLoopback` + `TryCreate` |
| P1-3 | Fail-soft silencioso | `Log.Warning` em block reader e converters hex (sem throw) |
| P2-10 | Pasta `Domain/Shared` + arquivo Auction | Shared já inexistente; `AuctionEntryAddresses.cs` → `AuctionAddresses.cs` |
| — | Dead code `Bits.ToString ?? "Unknown"` | Removido em `DebugConsoleRenderer` |
| P2-7 | Exit code em fatal startup | `Environment.ExitCode = 1` no catch de `Program.cs` |
| P2-4 | Ordem de membros + `_firstRender` | `SafeDispatch` antes dos públicos; `FirstRender` sem `_` |
| P2-5 | Split OptionalJsonConverter / Factory | `OptionalJsonConverterFactory.cs` separado |
| P2-8 | `Features:Debugging` Dev vs Release | `false` em appsettings; `true` em Development |
| P2-2 | Interfaces QuestLoader / DigimonLoader | `IQuestLoader` / `IDigimonLoader` + DI |
| P2-1 | Collection expressions Converters/Diffing | Differs/`StateEventFactory`: `List<T> = []`; Converters: `.ToList()` mantido (`Optional<List<T>>`) |
| P2-3 | Unificar logging `ILogger<T>` | `GameLoopService`, `MemoryReader`, `DuckstationConnector`; converters/block reader + Program ficam com Serilog estático |

### 11.2 Adiado

| ID original | Item | Motivo |
|-------------|------|--------|
| P1-1 | Race Hub × GameLoop no `GameStateStore` | Sintomas sobretudo em connect/reconnect (F5); redesign de conexão previsto |
| P1-4 | Party ≥1 ocupado (assert ou doc) | Pré-load tolerado; Backend permanece espelho |
| P1-5 | Digievolution filled→empty (guard/doc) | Mesma filosofia; sem guard agora |
| P3-5 | Assert / tipar `Slots.Count == 3` | JSON já fixa 3 slots; ROI baixo |
| P3-4 | Backpressure / await no `SafeDispatch` | Irrelevante com 1 cliente; junto com redesign de conexão |

Governança B/F/G no `BUSINESS_RULES` (§13.1) fica opcional e ligada a este bloco adiado.

### 11.3 Backlog restante (fácil → difícil)

Ordem sugerida para a próxima onda de higiene / evolução. Hub aberto e `AllowedHosts: "*"` seguem aceitos enquanto o bind for loopback.

| # | ID | Achado | Esforço relativo |
|---|----|--------|------------------|
| 1 | P2-9 | Renomear typos Memory (`Wisdow`, `Equipaments`) alinhando Domain | Médio (JSON + Memory + testes) |
| 2 | P2-6 | `Event.Payload` tipado / `IDTO` | Médio–alto |
| 3 | P3-2 | Helper anti-boilerplate nos Differs | Alto |
| 4 | P3-3 | `Optional<T> : IEquatable<Optional<T>>` | Alto (hot path / cuidado) |
| 5 | P3-1 | Descoberta automática de quest JSONs no `AddressesRepository` | Alto |

---

## 12. Pontos fortes (para não perder de vista)

1. **Arquitetura legível** — qualquer dev/IA segue o pipeline sem adivinhar.
2. **Contrato de eventos maduro** — InitialState + patches `Optional<T>` + syncer rules.
3. **Resiliência do game loop** — conexão falha sem matar o processo; códigos de erro tipados para o frontend.
4. **Party pairing** — inconsistência DigimonId XOR Digimon é estruturalmente evitada.
5. **Testes densos** — especialmente conector, game loop e DI.
6. **CODE_RULES bem internalizadas** no código novo (primary ctors, collection expressions, arquivos unitários, stateless readers).
7. **Sem dependências pesadas** — sidecar enxuto.
8. **ActiveDigievolution / empty slot IDs** tratados com sentinelas conhecidos do jogo (`0xFFFF`, `EmptySlotId`).

---

## 13. Insights transversais

### 13.1. O Backend é um “espelho inteligente”, não um “guardião de domínio”

A doc de negócio descreve invariantes fortes; o código **prioriza fidelidade à RAM** com poucas normalizações. Isso não é necessariamente errado para um memory tracker — mas há **desalinhamento documental**: o MD soa prescritivo (“erro crítico”), o Backend soa descritivo (“o que a memória disse”).

**Sugestão de governança (adiada):** marcar no `BUSINESS_RULES.md` quais invariantes são:

- **(B)** enforced no Backend  
- **(F)** enforced só no Frontend  
- **(G)** garantidas pelo jogo / assumidas  

Triagem ago/2026: não priorizar guards nem reescrita ampla da doc agora; ver §11.2.

### 13.2. Consistência geracional do código

Assemblers/Loaders ok com collection expressions. Differs/`StateEventFactory`: `List<T> x = []` (**feito**). Converters: `.ToList()` permanece ao popular `Optional<List<T>>` (cast `(List<T>)[.. ]` rejeitado como pior estilo).

### 13.3. Extensão de quests via repository hardcoded

O sucesso dos DRI agents/legendary weapons veio com custo: cada JSON novo toca `AddressesRepository` + (às vezes) loaders. Skills mitigam, mas a estrutura pede descoberta por convenção. (Backlog §11.3 item 5.)

### 13.4. Segurança “adequada ao produto” ≠ “padrão seguro genérico”

Para app desktop local, hub aberto + HTTP loopback é razoável. O CORS com substring foi o ponto tratado como bug no contexto atual — **corrigido** na triagem (allowlist).

### 13.5. Observabilidade

Após a triagem, Information em Development, Warnings no fail-soft, `Features:Debugging` por ambiente e `ILogger<T>` nos serviços DI principais fecharam o buraco principal de observabilidade. Serilog estático permanece em converters/`MemoryBlockReader`/`Program` (exceção consciente).

---

## 14. Recomendações práticas (ordem sugerida)

Seguir o backlog **§11.3** (fácil → difícil). Próximo passo natural da triagem: **item 1** (typos Memory `Wisdow` / `Equipaments`).

Itens de conexão (`GameStateStore`, `SafeDispatch`) e invariantes Party/Digievolution: **não** entram nesta fila — ver §11.2.

---

## 15. Nota final por dimensão

| Dimensão | Nota | Comentário curto |
|----------|------|------------------|
| Arquitetura / fluxo | 9.0 | Clara, alinhada ao BUSINESS_RULES §1 |
| Regras de negócio | 7.5 | Core ok; cascade documentado; Party≥1 / Digievolution adiados como espelho |
| Qualidade / CODE_RULES | 8.5 | Excelente no novo; retrofit pendente em Diff/Convert |
| Segurança | 8.0 | Ok local; CORS allowlist feito; hub aberto aceitável no loopback |
| Concorrência | 6.5 | Race no store adiada; dispatch fire-and-forget |
| Observabilidade | 7.5 | Serilog por ambiente + Warnings fail-soft |
| Testes | 8.5 | Amplos e bons; alguns gaps de corner/segurança |
| Manutenibilidade | 8.0 | Padrão vertical ótimo; repository/diff boilerplate |
| **Geral Backend** | **8.0 / B+** | Base sólida; próximos passos = higiene do backlog §11.3 |

---

## 16. Apêndice — inventário rápido de interfaces

28 interfaces públicas (+ `IQuestLoader` / `IDigimonLoader`); quase todas com implementação 1:1. Marker: `IDTO`.

Camadas de implementação Windows acopladas por design: `WindowsProcessProvider`, `WindowsMemoryProvider`, `WindowsMemoryAccessor` — adequado ao escopo DuckStation/Windows; abstrações (`IProcessService`, `IMemoryProvider`) já existem se um dia houver outro host.

---

*Fim da review do Backend (análise jul/2026; status atualizado ago/2026). Próximo passo natural do backlog: §11.3 item 1 (typos Wisdow/Equipaments). Review espelhada do Frontend em `AI_REVIEW/frontend/`.*
