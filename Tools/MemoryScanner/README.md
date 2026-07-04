# MemoryScanner

Utilitário para capturar snapshots da RAM do DuckStation e investigar endereços de memória do Digimon World 2003.

## Requisitos

- DuckStation em execução (`duckstation-qt-x64-ReleaseLTCG`)
- .NET SDK

```powershell
cd Tools/MemoryScanner
```

## Snapshot

```powershell
dotnet run -- snapshot Snapshots/estado.bin
```

Salva 2 MiB de RAM PS1 (`0x00000000`–`0x001FFFFF`).

## Qual comando usar?

| Investigação | Comando | size |
|--------------|---------|------|
| Flag de quest (bit flip) | `compare` | byte (padrão) |
| Missão reversível (3 estados) | `intersect-changed` | 1 |
| Contador absoluto (blast, EXP) | `chain-match` | 4 |
| Valor mudou para X | `compare-changed` | 1/2/4 |
| Valor mudou de A para B | `compare-changed` + `--old-val A` | 1/2/4 |
| Incremento conhecido | `compare-delta` | 1/2/4 |
| Inspecionar candidato | `dump` | — |
| Quest + encounter report | `analyze-pair` | — |

## Tamanhos de valor

| size | Tipo | Exemplos no jogo |
|------|------|------------------|
| 1 | byte | flags de quest, quantidade de itens |
| 2 | Int16 | level, HP, MapId, stats Digimon |
| 4 | Int32 | EXP, Bits, blast gauge (0–1000) |

Little-endian (PS1/DuckStation).

## Regiões

| Preset | Range | Uso |
|--------|-------|-----|
| `full` | RAM inteira | contadores, blast gauge |
| `quest` | `0x48000`–`0x4D000` | flags de missão (default do `compare`) |
| `stats` | `0x494000`–`0x498000` | blocos Digimon |
| `inventory` | `0x4858F`–`0x486FF` | quantidades de itens |

```powershell
dotnet run -- compare Snapshots/a.bin Snapshots/b.bin --region quest
dotnet run -- search-value Snapshots/a.bin 100 --size 2 --region stats
dotnet run -- chain-match ... --region full
```

Range customizado: `--range 0x48000,0x4D000`

## Comandos

### compare (byte a byte)

Ideal para flags com análise de bits (`+ flag` / `- flag`):

```powershell
dotnet run -- compare Snapshots/antes.bin Snapshots/depois.bin
dotnet run -- compare Snapshots/antes.bin Snapshots/depois.bin 1 --region quest
```

### chain-match (multi-snapshot)

Encontra endereços com valores exatos em cada snapshot:

```powershell
dotnet run -- chain-match Snapshots/0-blast.bin Snapshots/200-blast.bin Snapshots/400-blast.bin --values 0,200,400 --size 4 --region full
```

### compare-changed

Endereços que mudaram para um valor. Com `--old-val`, exige valor anterior:

```powershell
dotnet run -- compare-changed Snapshots/a.bin Snapshots/b.bin 200 --size 4 --old-val 0 --region full
```

### compare-delta

Endereços onde o valor mudou exatamente N:

```powershell
dotnet run -- compare-delta Snapshots/a.bin Snapshots/b.bin 200 --size 4
```

### intersect-changed

Endereços que mudaram no meio e voltaram ao original (`f1 == f3`, `f1 != f2`):

```powershell
dotnet run -- intersect-changed Snapshots/antes.bin Snapshots/durante.bin Snapshots/depois.bin --size 1 --region quest
```

### search-value

```powershell
dotnet run -- search-value Snapshots/atual.bin 100 --size 2 --region stats
```

### dump

```powershell
dotnet run -- dump Snapshots/atual.bin 0x42B74 8
```

### analyze-pair

Relatório de main quest + encounter cache:

```powershell
dotnet run -- analyze-pair Snapshots/antes.bin Snapshots/depois.bin
```

## Flags comuns

| Flag | Descrição |
|------|-----------|
| `--size N` | 1, 2 ou 4 bytes |
| `--region NAME` | full, quest, stats, inventory |
| `--range 0xSTART,0xEND` | range hex customizado |
| `--max-results N` | limite de linhas exibidas (default 200) |
| `--old-val N` | valor anterior exigido (`compare-changed`) |
| `--max-val N` | filtro superior (`intersect-changed`) |
| `--values v1,v2,...` | valores por snapshot (`chain-match`) |

Posicionais legados (`[size] [rangeStart] [rangeEnd] [oldVal]`) continuam funcionando.
