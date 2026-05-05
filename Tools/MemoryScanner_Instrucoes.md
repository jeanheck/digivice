# Guia do MemoryScanner

Este utilitário é utilizado para capturar "snapshots" da memória RAM do emulador DuckStation e comparar as mudanças entre diferentes estados do jogo. É a ferramenta principal para detectar quais endereços de memória controlam o progresso de missões.

## Localização
O projeto está em: `Tools/MemoryScanner/`

## Como Usar (Passo a Passo)

1.  **Prepare o Emulador**: Abra o DuckStation e carregue o jogo.
2.  **Abra o Terminal**: Vá para a pasta do scanner.
    ```powershell
    cd Tools/MemoryScanner
    ```
3.  **Salve o Estado Inicial (Snapshot 1)**: Antes de realizar a ação da missão.
    ```powershell
    dotnet run -- snapshot Snapshots/antes.bin
    ```
4.  **Realize a Ação no Jogo**: Avance na missão ou faça o passo que deseja detectar.
5.  **Salve o Estado Intermediário (Snapshot 2)**: Durante ou logo após a ação.
    ```powershell
    dotnet run -- snapshot Snapshots/durante.bin
    ```
6.  **Salve o Estado Final (Snapshot 3 - Opcional)**: Se a flag de missão for temporária ou resetar depois.
    ```powershell
    dotnet run -- snapshot Snapshots/depois.bin
    ```

## Comandos de Análise

### Comparar Mudanças (0 -> 1)
Ideal para flags que são ativadas:
```powershell
dotnet run -- compare Snapshots/antes.bin Snapshots/durante.bin
```

### Detectar Mudança para Valor Específico
Se você suspeita que o valor mudou para `50`:
```powershell
dotnet run -- compare-changed Snapshots/antes.bin Snapshots/durante.bin 50
```

### Técnica de Intersecção (Para Missões)
Este é o comando mais poderoso. Ele encontra endereços que mudaram no "durante" mas voltaram ao valor original no "depois" (ou que mantiveram uma relação específica entre os 3 arquivos).
```powershell
dotnet run -- intersect-changed Snapshots/antes.bin Snapshots/durante.bin Snapshots/depois.bin
```

### Buscar Valor na RAM
```powershell
dotnet run -- search-value Snapshots/antes.bin <valor>
```

---
*Nota: Certifique-se de que a pasta `Snapshots` existe dentro de `Tools/MemoryScanner` antes de rodar os comandos.*
