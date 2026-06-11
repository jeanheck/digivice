# Melhorias opcionais — camada de memória e Duckstation

> Referência de contexto para evoluções futuras no backend (memória, sessão Duckstation, game loop).
> A refatoração principal está **concluída** (jun/2026): `DuckstationSession`, `DuckstationConnector`, `MemoryReader` com `MemoryReadException`, `FlagByteHelper`.

---

## Arquitetura atual (resumo)

```
GameLoopService
         │
         ▼
DuckstationConnector               ← EnsureConnection, ClearSession (Infrastructure)
         │
         ▼
DuckstationSession                 ← IMemoryAccessor? (holder compartilhado)
         │
         ▼
IMemoryReader                      ← ReadInt32 / ReadInt16 / ReadBytes (+ MemoryReadException)
         │
         ▼
Domain readers + FlagByteHelper    ← flags de journal/auction
```

### `DuckstationConnector.EnsureConnection` (jun/2026)

Invariante de conexão válida:

```csharp
HasActiveConnection && !ProcessIdChanged
```

- **`HasActiveConnection`** — `ConnectedProcessId` e `duckstationSession.Accessor` preenchidos.
- **`ProcessIdChanged`** — PID atual do processo (`GetProcessIdByName`) difere do PID conectado (inclui processo encerrado → `null`).

Qualquer outro cenário → `ClearSession()` + tentativa de abrir mapping (`duckstation_{pid}`).

O fast path **não** reabre mapping nem chama `GetProcessIdByName` quando já desconectado (short-circuit).

Invalidação externa (falha de I/O no compose): `GameLoopService` chama `ClearSession()`; no tick seguinte `EnsureConnection` reconecta se o emulador estiver disponível.

---

## 1. Saúde da sessão — alive check em `EnsureConnection`

**Problema:** o alive check valida apenas processo vivo + PID igual. Não verifica se o **mapping/acessor ainda funciona**.

**Cenário:** processo vivo, PID igual, mapping inválido → `EnsureConnection()` retorna `true`, mas leituras falham no compose.

**Estado atual:** falha de I/O é detectada de forma **reativa** via `MemoryReadException` → `ClearSession()` no tick seguinte.

**Opções:**

| Abordagem | Prós | Contras |
|-----------|------|---------|
| Manter como está | Simples; erro real aparece na leitura | Janela de até 1 tick com sessão “falsa-viva” |
| Probe read leve no alive check | Detecta mapping morto antes do compose | Custo extra a cada 1s; duplica lógica de I/O |
| Reconnect proativo após N falhas | Menos flapping | Mais complexidade no coordinator |

**Prioridade:** baixa — comportamento aceitável hoje; decidir só se houver sintomas em runtime.

---

## 2. Conexão — ruído de log em falhas recorrentes

**Problema:** com emulador offline ou mapping indisponível, `EnsureConnection` emite `Log.Error` a **cada tick** (~1s) para processo não encontrado ou accessor nulo.

**Estado anterior:** essas falhas eram silenciosas (só config inválida e exceções logavam).

**Sugestões:**

| Abordagem | Quando usar |
|-----------|-------------|
| `Log.Debug` para processo/mapping ausente | Emulador fechado é estado esperado em dev |
| Log só na transição (desconectado → falha / conectado → falha) | Reduz spam mantendo visibilidade |
| Manter `Log.Error` | Se quiser alerta forte contínuo enquanto offline |

**Prioridade:** baixa — estética/operacional; decidir após observar logs em uso real.

---

## 3. Concorrência — singletons sem sincronização

**Problema:** `DuckstationConnector`, `DuckstationSession` e `MemoryReader` são singletons com estado mutável. Não há lock; funciona porque só o `GameLoopService` orquestra connect/clear hoje.

**Risco futuro:** segundo consumidor (endpoint HTTP, job paralelo) poderia causar race.

**Sugestão (se o modelo crescer):** documentar contrato “single-writer” ou introduzir sincronização no connector.

**Prioridade:** baixa — premissa atual do app (1 loop, 1 sessão) continua válida.

---

## 4. Modelo de recursos — campos nullable pós-`MemoryReadException`

**Problema:** `PlayerResource` (e possivelmente outros resources) ainda expõe `int?`, `short?`, `byte[]?` herdados da era em que leitura falha retornava `null`.

**Estado atual:** falha de I/O **propaga exceção**; leitura bem-sucedida sempre produz valor.

**Sugestão:** revisar resources/assemblers e tornar campos non-nullable onde a leitura é obrigatória no fluxo; manter nullable só onde “sem dado” é semântica de domínio (ex.: slot vazio).

**Prioridade:** baixa — limpeza de contrato; exige revisão cuidadosa do pipeline até o frontend.

---

## 5. `DigimonReader` — validação de bloco curto

**Problema:** após `ReadBytes`, ainda há check `memoryBlock.Length < DigimonMemoryBlockSize` → retorna `null` (domínio), enquanto falha de I/O lança exceção.

**Estado atual:** intencional — bloco incompleto vs. erro de sessão são caminhos distintos.

**Sugestão (opcional):** documentar ou unificar semântica (ex.: bloco curto também como `MemoryReadException` se for impossível na prática).

**Prioridade:** muito baixa — comportamento consciente; `PartyLoader` trata `null` limpando slot.

---

## Priorização sugerida

| Ordem | Tópico | Motivo |
|-------|--------|--------|
| 1 | §2 Ruído de log em falhas recorrentes | Baixo esforço; melhora DX se logs incomodarem |
| 2 | §1 Probe no alive check | Só se houver problema real em runtime |
| 3 | §4 Resources non-nullable | Refactor transversal |
| 4 | §3, §5 | Cosmético ou premissa documental |

---

## Fora de escopo / já resolvido

- Lifetime do `MemoryMappedFile` em `WindowsMemoryProvider` ✅
- Separação sessão (`DuckstationSession`) vs política (`DuckstationConnector`) vs I/O (`MemoryReader`) ✅
- Política de erro unificada (`MemoryReadException` + `ClearSession` no `GameLoopService`) ✅
- `ReadByteSafe` → `FlagByteHelper` na domain layer ✅
- `InvalidateConnection` / `HandleSilentReadFailure` ✅ removidos
- Cache de `EmulatorProcessName` no `DuckstationConnector` ✅
- Log em falhas de conexão (`catch` + config inválida) ✅
- `AuctionReader` — leitura única do byte compartilhado ✅
- **`DuckstationConnector` refactor (jun/2026)** ✅
  - `Disconnect()` → `ClearSession()` (API + `GameLoopService`)
  - `TryConnect` / `Connect` inlined em `EnsureConnection`
  - Invariante explícita: válido ⟺ `HasActiveConnection && !ProcessIdChanged`
  - Reconexão proativa no mesmo tick quando PID muda (mapping disponível)
  - Removido `HasLoggedSuccessfulConnection` — log de sucesso a cada reconexão bem-sucedida (aceitável: só ocorre em transição desconectado→conectado)
  - Removido check redundante `IsNullOrEmpty(EmulatorProcessName)` no alive check
