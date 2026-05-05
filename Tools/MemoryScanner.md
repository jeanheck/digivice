# Guia do MemoryScanner

Este utilitário é utilizado para capturar "snapshots" da memória RAM do emulador DuckStation e comparar as mudanças entre diferentes estados do jogo. É especialmente útil para identificar endereços de memória que controlam o progresso de missões, status de personagens e flags globais.

## Localização do Projeto
O código fonte está em: `Tools/MemoryScanner/`

## Requisitos
1. **DuckStation** deve estar em execução.
2. O nome do processo deve ser `duckstation-qt-x64-ReleaseLTCG` (padrão da versão Windows).
3. O emulador compartilha a memória automaticamente via Memory Mapped Files.

## Como Usar

Navegue até a pasta do projeto:
```powershell
cd Tools/MemoryScanner
```

### 1. Salvar um Estado da Memória (Snapshot)
Para salvar o estado atual da RAM em um arquivo:
```powershell
dotnet run -- snapshot Snapshots/missao_inicio.bin
```
*Dica: Salve os arquivos na pasta `Snapshots` para organização.*

### 2. Comparar Mudanças Simples
Para ver o que mudou de 0 para 1 (comum em flags de missão):
```powershell
dotnet run -- compare Snapshots/antes.bin Snapshots/depois.bin
```

### 3. Detectar Missões (Método de Intersecção)
Para missões que ativam e depois resetam (ou mudam um valor específico e depois voltam), use o `intersect-changed`. 
Este comando procura por endereços onde: `f1 == f3` (voltou ao original) mas `f1 != f2` (mudou no meio).

```powershell
dotnet run -- intersect-changed Snapshots/antes.bin Snapshots/durante.bin Snapshots/depois.bin
```

### 4. Procurar por um Valor Específico
Se você sabe que um valor (ex: ID de um item) mudou para `50`:
```powershell
dotnet run -- compare-changed Snapshots/antes.bin Snapshots/depois.bin 50
```

### 5. Buscar Valor na Memória
Para encontrar todos os endereços que possuem um valor agora:
```powershell
dotnet run -- search-value Snapshots/atual.bin 100
```

## Resumo de Comandos
| Comando | Descrição |
| :--- | :--- |
| `snapshot <file>` | Salva a RAM do PS1 (2MB) no arquivo especificado. |
| `compare <f1> <f2>` | Mostra diferenças (padrão 0 -> 1). |
| `compare-changed <f1> <f2> <val>` | Mostra endereços que mudaram para o valor `<val>`. |
| `intersect-changed <f1> <f2> <f3>` | Encontra endereços que mudaram em `f2` mas voltaram ao estado de `f1` em `f3`. |
| `search-value <f1> <val>` | Busca o valor `<val>` no snapshot. |
| `dump <file> <hex> <len>` | Faz um dump hexadecimal de uma região específica. |
