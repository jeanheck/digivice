# Melhorias opcionais — camada de memória e Duckstation

> Referência de contexto para evoluções futuras no backend (memória, sessão Duckstation, game loop).
> A refatoração principal está **concluída** (jun/2026): `DuckstationConnector`, `MemoryReader` com `MemoryReadException`, `FlagByteHelper`, `DuckstationConnectionCoordinator`.

---

## Arquitetura atual (resumo)

```
GameLoopService
         │
         ▼
DuckstationConnectionCoordinator   ← connect/disconnect, eventos, HandleMemoryReadFailure
         │
         ▼
IDuckstationConnector              ← TryConnect, Disconnect, IsConnectionAlive
         │
         ▼
IMemoryReader                      ← ReadInt32 / ReadInt16 / ReadBytes (+ MemoryReadException)
         │
         ▼
Domain readers + FlagByteHelper    ← flags de journal/auction
```

---

## 1. Saúde da sessão — `IsConnectionAlive`

**Problema:** `IsConnectionAlive()` valida apenas se o processo do emulador ainda existe e se o PID coincide. Não verifica se o **mapping/acessor ainda funciona**.

**Cenário:** processo vivo, PID igual, mapping inválido → `IsConnectionAlive() == true`, mas leituras falham no compose.

**Estado atual:** falha de I/O é detectada de forma **reativa** via `MemoryReadException` → `HandleMemoryReadFailure()` no tick seguinte.

**Opções:**

| Abordagem | Prós | Contras |
|-----------|------|---------|
| Manter como está | Simples; erro real aparece na leitura | Janela de até 1 tick com sessão “falsa-viva” |
| Probe read leve no alive check | Detecta mapping morto antes do compose | Custo extra a cada 1s; duplica lógica de I/O |
| Reconnect proativo após N falhas | Menos flapping | Mais complexidade no coordinator |

**Prioridade:** baixa — comportamento aceitável hoje; decidir só se houver sintomas em runtime.

---

## 2. Conexão — ruído de log no sucesso

**Problema:** `TryConnect` loga `"Connected to DuckStation! Mapping found..."` a cada reconexão bem-sucedida.

**Sugestão:** manter (útil em dev), reduzir para `Log.Debug`, ou logar só na **primeira** conexão da sessão.

**Prioridade:** muito baixa — estética/ruído apenas.

---

## 3. Concorrência — singletons sem sincronização

**Problema:** `DuckstationConnector` e `MemoryReader` são singletons com estado mutável (`IsConnected`, accessor). Não há lock; funciona porque só o `GameLoopService` orquestra connect/disconnect hoje.

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
| 1 | §1 Probe em `IsConnectionAlive` | Só se houver problema real em runtime |
| 2 | §4 Resources non-nullable | Refactor transversal |
| 3 | §2, §3, §5 | Cosmético ou premissa documental |

---

## Fora de escopo / já resolvido

- Lifetime do `MemoryMappedFile` em `WindowsMemoryProvider` ✅
- Separação sessão (`DuckstationConnector`) vs I/O (`MemoryReader`) ✅
- Política de erro unificada (`MemoryReadException` + `HandleMemoryReadFailure`) ✅
- `ReadByteSafe` → `FlagByteHelper` na domain layer ✅
- `InvalidateConnection` / `HandleSilentReadFailure` ✅ removidos
- Cache de `EmulatorProcessName` no `DuckstationConnector` ✅
- Log em falhas de `TryConnect` ✅
- `AuctionReader` — leitura única do byte compartilhado ✅
