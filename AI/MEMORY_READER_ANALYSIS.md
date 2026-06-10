# Análise — `MemoryReader.cs`

> Referência de contexto para refatoração da camada de memória e, indiretamente, da conexão com o Duckstation.
> Data da análise: jun/2026.

---

## Resumo executivo

`MemoryReader` é o **ponto de acoplamento entre o backend e a RAM do Duckstation**. Hoje ela concentra **lifecycle de conexão**, **descoberta do emulador**, **política de erro de I/O** e **leitura bruta de memória** — tudo numa única classe.

Há responsabilidade demais aqui, e isso explica parte da complexidade que apareceu no `DuckstationConnector` e no `GameLoopService` (checks pós-compose, falhas silenciosas, etc.).

---

## Papel atual no sistema

```
DuckstationConnector / GameLoop
         │
         ▼
    IMemoryReader  ◄── singleton, estado mutável
         │
    ┌────┴────┐
    │         │
 TryConnect   ReadInt32 / ReadBytes / ReadByteSafe ...
 IsConnectionAlive
 Disconnect
         │
         ▼
 IProcessService + IMemoryProvider + IConfiguration
         │
         ▼
 PlayerReader, DigimonReader, StepReader, AuctionReader ...
```

Registrada como **singleton** em DI. Todos os domain readers (`PlayerReader`, `DigimonReader`, etc.) dependem da mesma instância — o que faz sentido enquanto existir **uma** sessão com o emulador por vez.

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

O bloco guard + try/catch + log + `IsConnected = false` está copiado 3 vezes. Manutenção frágil.

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

Quem só lê memória (`PlayerReader`) vê métodos de conexão. Quem orquestra conexão (`DuckstationConnector`) vê métodos de leitura. **Interface Segregation Principle** violado.

### 8. Estado mutável em serviço singleton

Funciona hoje (um loop, uma sessão), mas:

- toda leitura altera potencialmente `IsConnected`
- não há sincronização (thread-safe) — ok enquanto só o `GameLoop` escreve na sessão, mas frágil se o modelo crescer

### 9. Config relida a cada `IsConnectionAlive()`

`EmulatorProcessName` é buscado no `IConfiguration` a cada chamada. Baixo impacto, mas desnecessário — valor estático por execução.

### 10. Log duplicado de conexão

`MemoryReader.TryConnect` loga `"Connected to DuckStation! Mapping found..."` e o `DuckstationConnector` loga `"Connected to DuckStation."` — ruído duplicado na reconexão.

---

## Problema relacionado fora da classe (mas afeta confiabilidade)

Em `WindowsMemoryProvider.OpenExisting`:

```csharp
using var memoryMappedFile = MemoryMappedFile.OpenExisting(mapName);
var accessor = memoryMappedFile.CreateViewAccessor();
return new WindowsMemoryAccessor(accessor);
// memoryMappedFile é disposed aqui ao sair do método
```

O `MemoryMappedFile` é descartado ao retornar, mas o `MemoryMappedViewAccessor` continua em uso. Isso **pode** causar leituras instáveis ou falhas intermitentes — e alimentar exatamente o tipo de erro silencioso que `MemoryReader` trata com `IsConnected = false`.

**Prioridade:** investigar e corrigir antes ou junto da refatoração do `MemoryReader`. Ver discussão em sessão de dev ou documento dedicado quando a correção for feita.

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
IDuckstationSession          IMemoryReader (ou IMemorySessionReader)
├── TryConnect()             ├── ReadInt32(address)
├── Disconnect()             ├── ReadInt16(address)
├── IsConnected              └── ReadBytes(address, length)
├── IsConnectionAlive()
└── expõe/leasing do accessor
```

- **Session** — processo, mapping, PID, lifecycle. Sem métodos `Read*`.
- **Reader** — só I/O sobre sessão ativa. Sem `TryConnect`.
- **`ReadByteSafe`** — mover para helper/reader de domínio (`StepReader`, `AuctionReader`).
- **Política de falha** — decidir num lugar só:
  - opção A: leitura lança `MemoryReadException` → session trata
  - opção B: leitura retorna `Result<T>` → session decide desconectar
  - opção C (atual, pior): `null`/`0` + flag mutável espalhada

### Ordem de refatoração sugerida

1. Investigar/corrigir lifetime do `MemoryMappedFile` em `WindowsMemoryProvider`
2. Extrair `DuckstationSession` (connect/disconnect/alive) de `MemoryReader`
3. Deixar `MemoryReader` só como thin reader sobre sessão
4. Unificar política de erro de I/O
5. Mover `ReadByteSafe` para domain layer
6. Segregar interfaces (`IDuckstationSession` vs `IMemoryReader`)

---

## Avaliação por critério

| Critério | Nota | Comentário |
|----------|------|------------|
| Single Responsibility | ⚠️ Baixo | Conexão + I/O + domínio |
| Testabilidade | ✅ Alto | Bem coberta |
| Consistência de API | ⚠️ Baixo | `null` vs `0`, semântica ambígua |
| Acoplamento | ⚠️ Médio | Config + processo + I/O juntos |
| Impacto na conexão | ⚠️ Alto | Falhas silenciosas propagam complexidade |
| Alinhamento CODE_RULES | ⚠️ Parcial | Readers deveriam ser stateless; esta é stateful por natureza — mas deveria ser **só** session, não reader+session |
| Manutenibilidade | ⚠️ Médio | Duplicação nos `Read*`, interface grande |

---

## Conclusão

`MemoryReader` funciona e é bem testada, mas é um **facade acidental** que mistura três papéis: **gerenciador de sessão Duckstation**, **cliente de I/O** e **utilitário de domínio**. Isso não é só “organização de código” — é a raiz de comportamentos difíceis de raciocinar (falha silenciosa, checks redundantes, estado inconsistente entre leituras).

Refatorar aqui primeiro **desbloqueia** simplificar o `DuckstationConnector` depois: ele passaria a falar com uma sessão clara, com política de erro explícita, em vez de reagir a efeitos colaterais escondidos dentro dos `Read*`.
