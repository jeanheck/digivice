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

## 2. Configuração — cache de `EmulatorProcessName`

**Problema:** `DuckstationConnector` relê `EmulatorProcessName` do `IConfiguration` em `TryConnect` e `IsConnectionAlive`. O valor não muda durante a execução.

**Onde:** `Backend/Infrastructure/Duckstation/DuckstationConnector.cs`

**Sugestão:** campo `readonly` inicializado no construtor primário, ou método privado `GetEmulatorProcessName()` para DRY entre os dois métodos.

**Impacto:** baixo (micro-otimização + legibilidade).

**Prioridade:** baixa.

---

## 3. Conexão — log em falhas de `TryConnect`

**Problema:** o `catch` de `TryConnect` seta `IsConnected = false` e retorna `false` **sem log**, diferente das leituras que logam antes de lançar `MemoryReadException`.

**Sugestão:** adicionar `Log.Error` (ou `Log.Warning`) no catch, alinhado ao padrão do `MemoryReader`.

**Impacto:** melhora diagnóstico quando connect falha por exceção inesperada (não só “processo não encontrado”).

**Prioridade:** baixa.

---

## 4. Conexão — ruído de log no sucesso

**Problema:** `TryConnect` loga `"Connected to DuckStation! Mapping found..."` a cada reconexão bem-sucedida.

**Sugestão:** manter (útil em dev), reduzir para `Log.Debug`, ou logar só na **primeira** conexão da sessão.

**Prioridade:** muito baixa — estética/ruído apenas.

---

## 5. Concorrência — singletons sem sincronização

**Problema:** `DuckstationConnector` e `MemoryReader` são singletons com estado mutável (`IsConnected`, accessor). Não há lock; funciona porque só o `GameLoopService` orquestra connect/disconnect hoje.

**Risco futuro:** segundo consumidor (endpoint HTTP, job paralelo) poderia causar race.

**Sugestão (se o modelo crescer):** documentar contrato “single-writer” ou introduzir sincronização no connector.

**Prioridade:** baixa — premissa atual do app (1 loop, 1 sessão) continua válida.

---

## 6. Domain readers — leituras repetidas no mesmo endereço

**Problema:** `AuctionReader` chama `FlagByteHelper.Read` (e portanto `ReadBytes(address, 1)`) **uma vez por leilão**, todos no **mesmo endereço** compartilhado.

**Sugestão:** ler o byte bruto uma vez (`FlagByteHelper.Read` sem mask ou `ReadBytes` direto) e aplicar bitmasks no loop — mesmo padrão mental do `MemoryBlockReader` (fetch once, parse many).

**Impacto:** menos I/O por tick quando há vários leilões; código ligeiramente mais explícito.

**Prioridade:** baixa — otimização; comportamento externo idêntico.

---

## 7. Modelo de recursos — campos nullable pós-`MemoryReadException`

**Problema:** `PlayerResource` (e possivelmente outros resources) ainda expõe `int?`, `short?`, `byte[]?` herdados da era em que leitura falha retornava `null`.

**Estado atual:** falha de I/O **propaga exceção**; leitura bem-sucedida sempre produz valor.

**Sugestão:** revisar resources/assemblers e tornar campos non-nullable onde a leitura é obrigatória no fluxo; manter nullable só onde “sem dado” é semântica de domínio (ex.: slot vazio).

**Prioridade:** baixa — limpeza de contrato; exige revisão cuidadosa do pipeline até o frontend.

---

## 8. `DigimonReader` — validação de bloco curto

**Problema:** após `ReadBytes`, ainda há check `memoryBlock.Length < DigimonMemoryBlockSize` → retorna `null` (domínio), enquanto falha de I/O lança exceção.

**Estado atual:** intencional — bloco incompleto vs. erro de sessão são caminhos distintos.

**Sugestão (opcional):** documentar ou unificar semântica (ex.: bloco curto também como `MemoryReadException` se for impossível na prática).

**Prioridade:** muito baixa — comportamento consciente; `PartyLoader` trata `null` limpando slot.

---

## Priorização sugerida

| Ordem | Tópico | Motivo |
|-------|--------|--------|
| 1 | §3 Log em `TryConnect` | Diagnóstico fácil, diff pequeno |
| 2 | §2 Cache `EmulatorProcessName` | DRY simples |
| 3 | §6 `AuctionReader` single read | Ganho concreto sem mudar API |
| 4 | §1 Probe em `IsConnectionAlive` | Só se houver problema real em runtime |
| 5 | §7 Resources non-nullable | Refactor transversal |
| 6 | §4, §5, §8 | Cosmético ou premissa documental |

---

## Fora de escopo / já resolvido

- Lifetime do `MemoryMappedFile` em `WindowsMemoryProvider` ✅
- Separação sessão (`DuckstationConnector`) vs I/O (`MemoryReader`) ✅
- Política de erro unificada (`MemoryReadException` + `HandleMemoryReadFailure`) ✅
- `ReadByteSafe` → `FlagByteHelper` na domain layer ✅
- `InvalidateConnection` / `HandleSilentReadFailure` ✅ removidos
