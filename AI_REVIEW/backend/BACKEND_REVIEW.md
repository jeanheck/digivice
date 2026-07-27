# Digivice — Backend Review

> **Data:** 22 de julho de 2026  
> **Escopo:** apenas `Backend/` (e cobertura cruzada com `Tests/` e `AI/*.md` como balizadores)  
> **Referências de “certo/errado”:** `AI/CODE_RULES.md`, `AI/BUSINESS_RULES.md`  
> **Métricas aproximadas:** ~207 arquivos `.cs` no Backend (~4.4k LOC), ~95 arquivos de teste (~6.3k LOC, ~277 Fact/Theory), 22 JSONs de definição de memória

---

## 1. Sumário executivo

O Backend do Digivice é um serviço ASP.NET Core (.NET 10) **local, Windows-only**, que faz polling da memória compartilhada do emulador DuckStation a cada ~1s, monta um `State` de domínio, calcula diffs e empurra apenas deltas via SignalR para o frontend Tauri/Vue.

A arquitetura é **clara, previsível e altamente consistente**: fatias verticais repetem o mesmo pipeline (`Addresses → Reader → Resource → Assembler → Model → Converter/Differ → DTO → Event`). O investimento em testes é acima da média para o tamanho do código. As regras de estilo do `CODE_RULES.md` estão bem aplicadas no código recente (Assemblers, Loaders, Providers, Infrastructure).

Os principais riscos e dívidas não estão em “código caótico”, e sim em:

1. **Operabilidade** — Serilog com `MinimumLevel.Warning()` engole todos os `Log.Information` de startup/conexão (config morta em `appsettings.json`).
2. **Filosofia “confiar na RAM”** — a maior parte das invariantes de negócio do `BUSINESS_RULES.md` não é validada no Backend; só o pareamento de slots da Party e o cascade da main quest sanitizam dados.
3. **Concorrência implícita** — `GameStateStore` é mutado pelo game loop e lido pelo Hub SignalR **sem sincronização**.
4. **CORS frouxo** — `origin.Contains("tauri")` + credentials; risco baixo no uso local, mas padrão inseguro se o bind mudar.
5. **Fail-soft silencioso** — `MemoryBlockReader` e converters de hex retornam `0` em erro, mascarando typos em `*Addresses.json`.

**Nota geral sugerida: B+ / 8.0** — arquitetura sólida e bem testada, com gaps pontuais de observabilidade, thread-safety e alinhamento explícito com algumas regras de negócio.

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
| Party: ≥1 slot ocupado | ❌ | Aceito all-empty; teste de integração prova isso |
| `digimonId` no slot, não no Digimon | ✅ | Modelo correto |
| Digievolution: proibido filled→empty | ❌ | Pass-through da RAM; sem guard histórico |
| Level 1–99 (sem clamp) | ✅ | Correto *não* clampar (doc diz que é redundante) |
| Blast gauge por Digimon | ✅ | Endereços +2 por rookie; model no Digimon |
| Journal: só `Value` dinâmico | ⚠️ | Quase; ver cascade da main quest |
| Journal: set fixo / InitialState / sem flag de conclusão | ✅ | |
| Player name não rastreado | ✅ | `0x00048D88` ausente do Backend |

### 4.2. Detalhamento dos gaps

#### Party — “sempre 3 slots” e “pelo menos 1 ocupado”

`PartyAssembler` mapeia 1:1 o que veio do JSON/resource. Não há validação de `Count == 3`. A invariante “≥1 ocupado” **não é tratada como erro crítico** no Backend: `PartyLoaderTests.Load_ShouldReturnAllEmptySlots_WhenAllSlotsAreEmpty` documenta all-empty como comportamento esperado, e `DebugConsoleRenderer` até renderiza “No Digimons detected…”.

**Insight:** o Backend trata Party como espelho tolerante da RAM + normalização de pairing; as regras mais duras do doc estão mais no nível “contrato esperado do jogo + frontend”, não como asserts de domínio no servidor.

#### Digievolution filled → empty

`DigievolutionSlotReader` define vazio como `digievolutionId <= 0`. O Differ emite delta se o ID mudou — inclusive para `null`. Não há comparação com o estado anterior no assembler/loader para rejeitar “esvaziamento”.

**Risco prático:** save-state, frame intermediário ou glitch de leitura poderia empurrar filled→empty ao frontend. A regra de negócio assume que a gameplay nunca faz isso; o Backend não defende.

#### `NormalizeMainQuestProgression` — desvio consciente e não documentado

Em `JournalAssembler`:

```csharp
// Se o próximo step > 0 e o atual == 0 → força atual = 1
```

- Só na **main quest** (side/legendary/DRI não recebem).
- **Muta** `Step.Value` depois da montagem — não é pass-through puro da RAM.
- Tem testes dedicados (`JournalAssemblerTests`).
- **Não aparece** em `BUSINESS_RULES.md`.

**Avaliação:** parece compensação de quirk real da memória do jogo (steps “pulados” na RAM). É regra de negócio *de fato*, mas está só no código. Recomendação: documentar no `BUSINESS_RULES.md` e deixar explícito por que side quests não usam o mesmo cascade.

#### Blast gauge — doc vs código

`BUSINESS_RULES` cita `Int32 LE`; o código usa `ReadInt16` e o stride dos endereços é de 2 bytes (`0x42B74`, `76`, `78`…). O código é **autoconsistente**; o doc parece desatualizado. Preferir corrigir o MD.

#### Auction no Journal

Auction está corretamente sob `Journal.Auctions` (não orphaned). O `AuctionLoader` fica flat em `Application/Loaders/` enquanto `QuestLoader`/`DigimonLoader` estão em subpastas — inconsistência de pasta, não de wiring.

### 4.3. Filosofia de validação: “trust the RAM”

Sanitização ativa só em:

1. **PartyLoader** — pairing DigimonId/DigimonResource (+ ID desconhecido → slot vazio, sem log).
2. **JournalAssembler** — cascade da main quest.
3. **DigimonAssembler** — `ActiveDigievolutionId` `0`/`0xFFFF` → `0`.

Todo o resto (level, vitals, blast, digievolution empty/filled, contagem de slots) é **pass-through**. Isso é coerente com a orientação da doc para level, mas **diverge** do tom de “inconsistência crítica” usado para Party/Digievolution.

---

## 5. Qualidade de código vs `AI/CODE_RULES.md`

### 5.1. Scorecard

| Regra | Nota | Situação |
|-------|------|----------|
| Construtores primários | A | Quase 100%; exceção justificável em `MemoryReadException` |
| Um tipo por arquivo | A | Única quebra: `OptionalJsonConverter` + Factory no mesmo arquivo |
| Collection expressions `[ ]` / `[..]` | C+ | Assemblers/Loaders ok; Converters/Diffing ainda usam `.ToList()` / `new List<>()` |
| Usings limpos | A | Spot-check limpo |
| Readers/Converters stateless | A | Sem estado mutável de instância |
| Sem prefixo `_` | A- | 1 violação: `_firstRender` em `DebugConsoleRenderer` |
| `new();` | A | Sem violações |
| Ordem de membros (privados → públicos) | B+ | Violação clara em `EventDispatcherService` (`SafeDispatch` depois dos públicos) |
| Tests: corner cases | A- | Infra bem coberta; gap pequeno em fallback de hex list |

### 5.2. Violações concretas (estilo)

1. **`DebugConsoleRenderer.cs`** — `private bool _firstRender` (underline).
2. **`EventDispatcherService.cs`** — método privado após públicos.
3. **`OptionalJsonConverter.cs`** — dois tipos no mesmo arquivo.
4. **`AddressesRepository.cs`** — `private readonly string dataDirectory = dataDirectory;` redundante com primary ctor (cheiro adjacente à regra 6).
5. **Converters/Diffing legados** — `JournalConverter`, `DigimonConverter`, `QuestConverter`, `PartyConverter`, `StepConverter`, `JournalDiffer`, `DigimonDiffer`, `PartyDiffer`, `QuestDiffer`, `StepDiffer`, `StateEventFactory` ainda com `.ToList()` / `new List<T>()`.

### 5.3. Outliers de padrão de projeto (além do CODE_RULES)

| Outlier | Detalhe |
|---------|---------|
| `QuestLoader` / `DigimonLoader` sem interface | Demais loaders têm `I*Loader`; estes são registrados como concreto |
| Logging híbrido | `ILogger<T>` em alguns serviços; `Serilog.Log` estático em `GameLoopService`, `MemoryReader`, `DuckstationConnector` |
| Typos na camada Memory | `Wisdow` / `Equipaments` nos Addresses/Resources/JSON; Domain corrige (`Wisdom`, `Equipments`) no Assembler — fronteira OK, mas confunde busca/refator |
| Arquivo ≠ tipo | `AuctionEntryAddresses.cs` declara `AuctionAddresses` |
| Pasta vazia | `Domain/Shared/` |
| `Event.Payload` como `object` | Perde tipagem; consumers fazem cast |
| `IDTO` marker vazio | Só constraint genérica; sem contrato real |
| Equals/GetHashCode manuais em records com `List<T>` | Necessário e bem feito; risco se um model novo esquecer (diff silencioso) |

### 5.4. Cheiros e bugs de qualidade

#### Logging Information morto (alto impacto operacional)

`Program.cs` configura Serilog com `MinimumLevel.Warning()` e `UseSerilog()` **sem** `ReadFrom.Configuration`. Resultado:

- `appsettings.json` → `"Default": "Information"` **não afeta** o Serilog.
- Mensagens como “Initializing…”, “SignalR Hub available…”, “Connected to DuckStation!”, “Starting GameLoopService…” **nunca aparecem**.
- Só Warning/Error/Fatal (e poucos caminhos) são visíveis.

Isso parece regressão de configuração (há vestígios históricos de logs em `Backend/logs/`).

#### `MemoryBlockReader` fail-soft vs `MemoryReader` fail-loud

- `MemoryReader`: exceção → `MemoryReadException` → game loop limpa sessão.
- `MemoryBlockReader`: offset inválido / catch genérico → **retorna 0**.

Leituras de Digimon/status (bloco pré-lido) podem corromper stats para zero **sem** derrubar a conexão — difíceis de diagnosticar.

#### Converters JSON de endereço

`HexStringToLongConverter`, `HexOrIntStringToIntConverter`, `HexStringListToLongListConverter` engolem input malformado (`0` / skip). Typo em `*Addresses.json` ≠ crash; = tracker errado em silêncio.

#### `Features:Debugging: true` no appsettings “de produção”

Se o backend for lançado como sidecar com console, o dashboard ANSI roda por padrão. Pode ser intencional para o produto desktop; vale ter profile Dev/Prod ou default `false` com override local.

#### Exit code em falha fatal

`Program.cs` catch top-level loga `Fatal` mas não seta `Environment.ExitCode` → supervisor pode achar sucesso.

#### Dead code

`player.Bits.ToString(...) ?? "Unknown"` em `DebugConsoleRenderer` — `int.ToString` nunca é null.

---

## 6. Segurança

Contexto: ferramenta **single-user, loopback**. O risco absoluto é baixo; os padrões abaixo importam se o bind/URL mudar ou se o app for embutido de forma mais exposta.

### 6.1. Achados

| # | Achado | Severidade (contexto local) | Severidade (se bind público) |
|---|--------|----------------------------|------------------------------|
| 1 | CORS: `origin.Contains("tauri")` + `AllowCredentials` | Baixa | Alta |
| 2 | Hub sem autenticação (`/gamehub` aberto) | Aceitável no loopback | Alta |
| 3 | `new Uri(origin)` sem try/catch no callback CORS | Baixa (preflight 500) | Baixa |
| 4 | HTTP sem TLS em `127.0.0.1:5000` | OK para local | N/A se permanecer loopback |
| 5 | MMF `duckstation_{pid}` sem validação de conteúdo | Inerente ao propósito | — |
| 6 | `AllowedHosts: "*"` | Irrelevante no loopback | Revisar se expandir |

### 6.2. Comentário sobre CORS

```csharp
policy.SetIsOriginAllowed(origin =>
    new Uri(origin).IsLoopback || origin.Contains("tauri"))
```

Qualquer origem cujo string contenha `"tauri"` passa. Combinado com credentials, é o padrão clássico de misconfig. Para desktop Tauri, preferir allowlist explícita de origins conhecidos (`tauri://localhost`, `http://tauri.localhost`, etc.) em vez de substring.

### 6.3. Superfície de memória

Ler MMF do DuckStation é o core do produto. Não há elevação especial além do que o OS exige para abrir o mapping. Risco de “processo malicioso criando mapping com nome previsível” exige já ter código local — mencionar só por completude.

---

## 7. Concorrência e estado compartilhado

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
| Serilog presente | Sim, mas nível mínimo demasiado alto |
| Correlation / structured fields | Parcial (templates bons em alguns Error) |
| Métricas | Ausentes (ok para o escopo) |
| Debug console | Útil; gated por feature + `!Console.IsOutputRedirected` |
| Erros de conexão tipados | Bom — `EmulatorConnectionErrorCodes` cobrem Config/Process/Mapping/Connection/MemoryRead/StateCompose |
| Falhas soft de bloco/hex | **Sem log** — buraco de diagnóstico |

**Recomendação prioritária:** alinhar Serilog com `appsettings` (ou baixar mínimo para Information em Development) e logar (Warning) quando converters/block reader caírem no fallback `0`.

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

## 11. Achados priorizados

### P0 — Corrigir cedo (impacto alto / esforço baixo)

| ID | Achado | Onde |
|----|--------|------|
| P0-1 | Serilog engole Information; appsettings Logging morto | `Program.cs` |
| P0-2 | Documentar (ou generalizar) `NormalizeMainQuestProgression` no BUSINESS_RULES | `JournalAssembler` + `AI/BUSINESS_RULES.md` |

### P1 — Importante

| ID | Achado | Onde |
|----|--------|------|
| P1-1 | Race Hub × GameLoop no `GameStateStore` | `GameStateStore`, `GameHub`, `ConnectionEventFactory` |
| P1-2 | CORS substring `"tauri"` + credentials | `Program.cs` |
| P1-3 | `MemoryBlockReader` / hex converters silenciosos | Memory readers/converters |
| P1-4 | Invariante Party ≥1 ocupado: decidir se Backend valida ou doc relaxa | `PartyLoader` / BUSINESS_RULES / testes |
| P1-5 | Digievolution filled→empty: guard, log, ou documentar “frontend only” | Differ / BUSINESS_RULES |

### P2 — Qualidade / higiene

| ID | Achado |
|----|--------|
| P2-1 | Retrofit collection expressions em Converters/Diffing |
| P2-2 | Interfaces para `QuestLoader`/`DigimonLoader` (ou documentar exceção) |
| P2-3 | Unificar logging em `ILogger<T>` |
| P2-4 | Ordem de membros em `EventDispatcherService`; remover `_firstRender` |
| P2-5 | Split `OptionalJsonConverter` / Factory |
| P2-6 | `Event` tipado / `Payload` como `IDTO` |
| P2-7 | Exit code ≠ 0 em fatal startup |
| P2-8 | `Features:Debugging` default consciente (Dev vs Release) |
| P2-9 | Renomear typos Memory (`Wisdow`, `Equipaments`) alinhando Domain |
| P2-10 | Remover pasta `Domain/Shared` vazia; alinhar nome arquivo Auction |

### P3 — Evolução / nice-to-have

| ID | Achado |
|----|--------|
| P3-1 | Descoberta automática de quest JSONs no repository |
| P3-2 | Helper anti-boilerplate nos Differs |
| P3-3 | `Optional<T> : IEquatable<Optional<T>>` no hot path |
| P3-4 | Backpressure / await no dispatch SignalR |
| P3-5 | Assert `Slots.Count == 3` no assembler/loader |

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

**Sugestão de governança:** marcar no `BUSINESS_RULES.md` quais invariantes são:

- **(B)** enforced no Backend  
- **(F)** enforced só no Frontend  
- **(G)** garantidas pelo jogo / assumidas  

### 13.2. Consistência geracional do código

Assemblers/Loaders parecem “pós-CODE_RULES”; Converters/Diffing “pré-regra de collection expressions”. O padrão do projeto é bom — falta uma passada de retrofit, não uma reescrita.

### 13.3. Extensão de quests via repository hardcoded

O sucesso dos DRI agents/legendary weapons veio com custo: cada JSON novo toca `AddressesRepository` + (às vezes) loaders. Skills mitigam, mas a estrutura pede descoberta por convenção.

### 13.4. Segurança “adequada ao produto” ≠ “padrão seguro genérico”

Para app desktop local, hub aberto + HTTP loopback é razoável. O CORS com substring é o único ponto que eu trataria como bug mesmo no contexto atual (esforço baixo, benefício claro).

### 13.5. Observabilidade é o elo fraco relativo

A qualidade do código e dos testes está à frente da qualidade do “o que eu vejo quando algo falha de forma soft”. Information silenciado + zeros silenciosos = debugging humano mais caro do que deveria.

---

## 14. Recomendações práticas (ordem sugerida)

1. **Corrigir Serilog** — `MinimumLevel.Information()` ou `ReadFrom.Configuration(builder.Configuration)`; validar que “Connected to DuckStation!” volta ao console.
2. **Documentar cascade da main quest** no `BUSINESS_RULES.md` (e Blast gauge Int16).
3. **Endurecer CORS** com allowlist explícita.
4. **Proteger `GameStateStore`** (lock ou snapshot).
5. **Decidir política de invariantes Party/Digievolution** (assert+log vs doc “Frontend only”).
6. **Logar fallbacks** de `MemoryBlockReader`/hex converters (pelo menos em Debug/Warning).
7. **Passada de estilo** em Converters/Diffing (collection expressions + member order + `_`).
8. **Refator incremental** do `AddressesRepository` se o número de quests continuar crescendo.

---

## 15. Nota final por dimensão

| Dimensão | Nota | Comentário curto |
|----------|------|------------------|
| Arquitetura / fluxo | 9.0 | Clara, alinhada ao BUSINESS_RULES §1 |
| Regras de negócio | 7.0 | Core ok; gaps em Party≥1 e Digievolution; cascade undoc |
| Qualidade / CODE_RULES | 8.5 | Excelente no novo; retrofit pendente em Diff/Convert |
| Segurança | 7.5 | Ok local; CORS e hub aberto são os pontos |
| Concorrência | 6.5 | Race no store; dispatch fire-and-forget |
| Observabilidade | 5.5 | Information morto; fail-soft sem log |
| Testes | 8.5 | Amplos e bons; alguns gaps de corner/segurança |
| Manutenibilidade | 8.0 | Padrão vertical ótimo; repository/diff boilerplate |
| **Geral Backend** | **8.0 / B+** | Base sólida; priorizar ops + invariantes documentadas |

---

## 16. Apêndice — inventário rápido de interfaces

28 interfaces públicas; quase todas com implementação 1:1. Exceções de registro DI sem interface: `QuestLoader`, `DigimonLoader`. Marker: `IDTO`.

Camadas de implementação Windows acopladas por design: `WindowsProcessProvider`, `WindowsMemoryProvider`, `WindowsMemoryAccessor` — adequado ao escopo DuckStation/Windows; abstrações (`IProcessService`, `IMemoryProvider`) já existem se um dia houver outro host.

---

*Fim da review do Backend. Próximo passo natural (fora deste documento): review espelhada do Frontend em `AI_REVIEW/frontend/`.*
