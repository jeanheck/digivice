# Análise — `MemoryReader.cs`

> Referência de contexto para refatoração da camada de memória e, indiretamente, da conexão com o Duckstation.
> Data da análise: jun/2026.

---

## Resumo executivo

`MemoryReader` é o **cliente de I/O** sobre a RAM do Duckstation. A **sessão** (connect/disconnect/alive) já foi extraída para `DuckstationConnector`, orquestrada pelo `DuckstationConnectionCoordinator`.

O que ainda concentra complexidade: **política de erro silenciosa** (`null`/`0` + `InvalidateConnection`) e **`ReadByteSafe`** (lógica de domínio na camada de infra). Passos 4 e 5 do plano abaixo endereçam isso.

---

## Papel atual no sistema

```
GameLoopService
         │
         ▼
DuckstationConnectionCoordinator  ◄── connect/disconnect, eventos, HandleSilentReadFailure
         │
         ▼
IDuckstationConnector (DuckstationConnector)  ◄── singleton, sessão Duckstation
         │
    TryConnect / Disconnect / IsConnectionAlive / InvalidateConnection
         │
         ▼
    IMemoryReader (MemoryReader)  ◄── thin reader sobre sessão
         │
    ReadInt32 / ReadInt16 / ReadBytes / ReadByteSafe
         │
         ▼
 PlayerReader, DigimonReader, StepReader, AuctionReader ...
```

Registrados como **singleton** em DI. Todos os domain readers dependem de `IMemoryReader`; lifecycle de conexão ficou em `IDuckstationConnector`, orquestrado pelo `DuckstationConnectionCoordinator`.

---

## Responsabilidades identificadas (SRP)

| # | Responsabilidade | Onde aparece |
|---|------------------|--------------|
| 1 | **Sessão/conexão** — conectar, desconectar, `IsConnected` | `TryConnect`, `Disconnect`, `Dispose` |
| 2 | **Saúde da sessão** — processo ainda existe? PID mudou? | `IsConnectionAlive` |
| 3 | **Descoberta Duckstation** — nome do processo, map `duckstation_{pid}` | `TryConnect` |
| 4 | **Configuração** — lê `EmulatorProcessName` em runtime | `TryConnect`, `IsConnectionAlive` |
| 5 | **I/O bruto** — delegar ao `IMemoryAccessor` | `ReadInt32`, `ReadInt16`, `ReadBytes` |
| 6 | **Política de erro de leitura** — log + `IsConnected = false` + `null` | todos os `Read*` |
| 7 | **Helper de domínio** — endereço zero, bitmask | `ReadByteSafe` |

**Veredicto SRP:** a classe **não tem uma única responsabilidade**. O mínimo natural seria separar **sessão** (conectar/manter/desconectar) de **leitura** (operar sobre uma sessão aberta).

---

## O que está bom

### 1. Testes sólidos

`MemoryReaderTests` cobre bem: connect, disconnect idempotente, alive check, leituras, falhas de accessor, config vazia, bitmask. É uma base confiável para refatorar.

### 2. Abstrações de infraestrutura

Depende de `IProcessService` e `IMemoryProvider`, não de Win32 diretamente. Facilita mock e testes.

### 3. Domain readers desacoplados da conexão

`PlayerReader`, `DigimonReader`, etc. só chamam `Read*`. Não sabem de processo, PID ou mapping — correto.

### 4. Reconnect limpo

`TryConnect()` sempre chama `Disconnect()` antes — evita accessor órfão.

### 5. `Disconnect` idempotente

Pode ser chamado várias vezes sem quebrar.

### 6. Construtor primário

Alinhado com as regras do projeto (`CODE_RULES.md`).

---

## O que está ruim ou problemático

### 1. Falha silenciosa na leitura (problema central)

Em qualquer erro de I/O:

```csharp
IsConnected = false;
return null;  // ou 0 no ReadByteSafe
```

Sem exceção, sem evento, sem callback. Quem consome precisa **inferir** que a conexão morreu:

- `DigimonReader` trata `memoryBlock == null` e retorna `null`
- `PlayerReader` **não trata** — propaga `null` em `Bits`, `NameInBytes`, `MapId`
- `GameLoopService` precisa do `if (!memoryReader.IsConnected)` pós-compose

Isso espalha a responsabilidade de “detectar sessão morta” para camadas acima.

### 2. API inconsistente entre métodos de leitura

| Método | Desconectado | Erro de I/O |
|--------|--------------|-------------|
| `ReadInt32/16/Bytes` | `null` | `null` + desconecta |
| `ReadByteSafe` | `0` | `0` (+ desconecta via `ReadBytes`, mas engole no `catch` externo) |

Quem lê `0` de `ReadByteSafe` não distingue “valor real zero na RAM” de “falha/leitura inválida”. Usado em journal/auctions — risco de falso negativo em flags de quest.

### 3. `IsConnectionAlive()` é incompleto

Só compara PID do processo. **Não valida** se o mapping/acessor ainda funciona. Cenário possível:

- Processo vivo, mapping inválido → `IsConnectionAlive() == true`, leituras falham silenciosamente

### 4. Duplicação de código nos `Read*`

~~O bloco guard + try/catch + log + `IsConnected = false` está copiado 3 vezes. Manutenção frágil.~~

Parcialmente endereçado: `GetConnectedAccessor`, `HandleReadFailure` e `TryRead<T>` centralizam guard + erro em `ReadInt32`/`ReadInt16`; `ReadBytes` reutiliza os helpers. Política de erro ainda é a mesma (passo 4 pendente).

### 5. `TryConnect` engole exceções sem log

```csharp
catch (Exception)
{
    IsConnected = false;
    return false;
}
```

Diferente dos `Read*`, que logam. Falhas inesperadas no connect somem.

### 6. `ReadByteSafe` não pertence a esta camada

Combina:

- regra de domínio (`address == 0 → 0`)
- leitura de 1 byte
- aplicação de bitmask
- duplo tratamento de erro (`ReadBytes` + `catch { return 0; }`)

Isso é lógica de **domain reader** (como `StepReader` já faz composição de bitmasks). `StepReader` e `AuctionReader` seriam o lugar natural.

### 7. Interface `IMemoryReader` inflada

~~Quem só lê memória (`PlayerReader`) vê métodos de conexão. Quem orquestra conexão (`DuckstationConnector`) vê métodos de leitura.~~

Parcialmente resolvido: conexão em `IDuckstationConnector`; `IMemoryReader` ainda expõe `ReadByteSafe` (domínio — passo 5 pendente).

### 8. Estado mutável em serviço singleton

Funciona hoje (um loop, uma sessão), mas:

- toda leitura altera potencialmente `IsConnected`
- não há sincronização (thread-safe) — ok enquanto só o `GameLoop` escreve na sessão, mas frágil se o modelo crescer

### 9. Config relida a cada `IsConnectionAlive()`

`EmulatorProcessName` é buscado no `IConfiguration` a cada chamada. Baixo impacto, mas desnecessário — valor estático por execução.

### 10. Log duplicado de conexão

`DuckstationConnector.TryConnect` loga sucesso ao conectar. Ruído baixo; sem ação pendente.

---

## Problema relacionado fora da classe (mas afeta confiabilidade)

~~Em `WindowsMemoryProvider.OpenExisting` o `MemoryMappedFile` era descartado ao retornar enquanto o accessor continuava em uso.~~

**Corrigido** no passo 1 da refatoração.

---

## Impacto na camada de conexão (por que “começar aqui” faz sentido)

| Sintoma em `DuckstationConnector` / `GameLoop` | Origem em `MemoryReader` |
|------------------------------------------------|--------------------------|
| Check `!IsConnected` após compose | Leitura desconecta silenciosamente |
| `HandleSilentReadFailure` | Política de erro na leitura, não na sessão |
| `IsConnectionAlive` + check pós-leitura redundantes | Alive check fraco vs. falha real de I/O |
| Lógica de disconnect espalhada | `MemoryReader` muda `IsConnected`; connector reage depois |

Enquanto **conexão e leitura** morarem juntos, qualquer mudança de política de erro exige coordenação em 3 lugares (`MemoryReader` → domain readers → `DuckstationConnector`).

---

## Direção de melhoria sugerida (conceitual)

### Separação mínima recomendada

```
IDuckstationConnector              IMemoryReader
├── TryConnect()                   ├── ReadInt32(address)
├── Disconnect()                   ├── ReadInt16(address)
├── IsConnected                    └── ReadBytes(address, length)
├── IsConnectionAlive()
├── InvalidateConnection()
└── Accessor
```

- **Connector** — processo, mapping, PID, lifecycle. Sem métodos `Read*`.
- **MemoryReader** — só I/O sobre sessão ativa. Sem `TryConnect`.
- **`ReadByteSafe`** — sair de `IMemoryReader`; lógica compartilhada via helper de domínio (passo 5).
- **Política de falha** — **opção A escolhida** (jun/2026):
  - ~~opção A: leitura lança `MemoryReadException` → sessão trata~~ **← adotada**
  - opção B: leitura retorna `Result<T>` → sessão decide desconectar
  - opção C (atual, pior): `null`/`0` + flag mutável espalhada

### Ordem de refatoração sugerida

1. ~~Investigar/corrigir lifetime do `MemoryMappedFile` em `WindowsMemoryProvider`~~ ✅
2. ~~Extrair `DuckstationConnector` (connect/disconnect/alive) de `MemoryReader`~~ ✅
3. ~~Deixar `MemoryReader` só como thin reader sobre sessão~~ ✅ (inclui helpers `TryRead`, `GetConnectedAccessor`, `HandleReadFailure`)
4. ~~**Unificar política de erro de I/O** — opção A (`MemoryReadException`)~~ ✅
5. Mover `ReadByteSafe` para domain layer — **próximo passo**
6. ~~Segregar interfaces (`IDuckstationConnector` vs `IMemoryReader`)~~ ✅ (parcial — `DuckstationConnectionCoordinator` orquestra conexão)

---

### Passo 4 — Política de erro (opção A): plano de implementação

**Decisão:** falha de I/O na leitura lança `MemoryReadException`. Quem trata desconexão/eventos é a camada de sessão/coordinator — não o reader retornando `null`/`0` silenciosamente.

**Comportamento alvo:**

| Situação | Comportamento |
|----------|---------------|
| Desconectado antes da leitura | `MemoryReadException` (sessão indisponível) |
| Erro de I/O no accessor | `MemoryReadException` (inner exception preservada) |
| Leitura bem-sucedida | retorna valor (`int`, `short`, `byte[]`) — **não nullable** |
| Domain reader recebe exceção | propaga até quem orquestra o tick |

**Arquivos / mudanças:**

1. **`MemoryReadException`** — nova exceção de domínio/infra (ex.: `Backend/Memory/MemoryReadException.cs`).
2. **`MemoryReader`** — remover retorno `null` por falha; remover `InvalidateConnection()` de dentro do reader; lançar `MemoryReadException` em falha/desconectado; manter log de erro antes de lançar.
3. **`IMemoryReader`** — assinaturas passam a retornar tipos não anuláveis (`int`, `short`, `byte[]`); documentar que lança `MemoryReadException`.
4. **Domain readers** (`PlayerReader`, `DigimonReader`, etc.) — tratar exceção ou deixar propagar conforme cada caso (ex.: `DigimonReader` hoje checa `memoryBlock == null`; passará a usar try/catch ou deixar subir).
5. **`DuckstationConnectionCoordinator` / `GameLoopService`** — capturar `MemoryReadException` no tick (substituir/complementar `HandleSilentReadFailure`); chamar `MarkDuckstationDisconnected()` / `Disconnect()` + evento `false`.
6. **`InvalidateConnection`** — revisar se ainda é necessário ou se `Disconnect()` no coordinator basta após opção A.
7. **Testes** — atualizar `MemoryReaderTests` e leitores afetados; cenários: desconectado, I/O error, sucesso.

**Fora de escopo deste passo:** mover `ReadByteSafe` (passo 5); expandir `IsConnectionAlive` com probe de mapping.

---

### Passo 5 — `ReadByteSafe` na domain layer: plano de implementação

**Depende do passo 4.** Só implementar depois que `ReadBytes` tiver política de erro explícita.

**Motivo:** `ReadByteSafe` mistura I/O com regras de domínio (`address == 0`, bitmask, retorno `0` em falha). Não pertence a `MemoryReader`/`IMemoryReader`.

**Consumidores atuais:** `StepReader`, `AuctionReader`, `RequisiteReader`.

**Abordagem — sem duplicar código em três readers:**

```
IMemoryReader.ReadBytes(address, 1)   ← I/O puro (+ MemoryReadException)
        ↓
FlagByteHelper (estático, stateless)    ← address==0, bitmask simples
        ↓
StepReader / AuctionReader / RequisiteReader
```

- **`FlagByteHelper`** — helper compartilhado (ex.: `Backend/Memory/Readers/Helpers/FlagByteHelper.cs`); métodos estáticos; chama `IMemoryReader.ReadBytes`; aplica `address == 0` e bitmask opcional.
- **`StepReader`** — delega casos simples ao helper; mantém `ReadValue` com lógica **específica** de múltiplas bitmasks (AND).
- **`AuctionReader` / `RequisiteReader`** — delegam ao helper.
- **`IMemoryReader`** — remover `ReadByteSafe`.
- **Testes** — mover/adaptar casos de `MemoryReaderTests` para helper + readers de domínio.

**Analogia:** mesmo princípio de separação do `MemoryBlockReader` (I/O vs interpretação), mas sem bloco contíguo — flags espalhadas na RAM usam helper leve, não outra classe de infra.

---

## Avaliação por critério

| Critério | Nota | Comentário |
|----------|------|------------|
| Single Responsibility | ⚠️ Médio | Sessão separada; `ReadByteSafe` ainda mistura domínio |
| Testabilidade | ✅ Alto | Bem coberta |
| Consistência de API | ⚠️ Baixo | `null` vs `0` — passo 4 (opção A) endereça |
| Acoplamento | ✅ Médio-alto | Connector vs reader segregados |
| Impacto na conexão | ⚠️ Médio | `HandleSilentReadFailure` até passo 4 |
| Alinhamento CODE_RULES | ⚠️ Parcial | Reader stateless exceto efeito colateral de `InvalidateConnection` |
| Manutenibilidade | ✅ Médio-alto | `TryRead` centraliza guard/erro; passo 4 unifica política |

---

## Conclusão

A refatoração estrutural (passos 1–3 e 6 parcial) já separou **sessão Duckstation** (`DuckstationConnector`) de **I/O** (`MemoryReader`). O que resta é tornar a **política de erro explícita** (passo 4, opção A) e retirar **lógica de domínio** do `ReadByteSafe` (passo 5).

Com a opção A, o `GameLoopService` deixa de inferir sessão morta via `HandleSilentReadFailure` + `IsConnected`; passa a reagir a `MemoryReadException` num único ponto. Isso simplifica o coordinator e elimina estados intermediários (`InvalidateConnection` com accessor zumbi).
