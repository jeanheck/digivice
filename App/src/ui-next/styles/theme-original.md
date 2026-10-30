# ui-next — configuração de estilo original

Registro da paleta da tela nova **antes** do ajuste com as cores dos menus do jogo (out/2026). Serve para comparar ou reverter.

## Tokens do tema (`theme.css`)

Asuka é o padrão (`@theme`); Amaterasu sobrepõe via `[data-server="amaterasu"]` no `<html>`.

| Token | Asuka | Amaterasu | Classes | Uso |
|---|---|---|---|---|
| `surface` | `#000021` | `#2b0018` | `bg-surface`, `ring-offset-surface` | Fundo do `MainPanel`, `Footer`, `EmulatorSlot`, abas inativas, interior dos cartões (vitals, digievoluções, stats, equipamentos) |
| `surface-active` | `#000036` | `#400022` | `bg-surface-active` | Aba ativa, fundo do `Digimon` e da área do `MapPanel` |
| `frame` | `#0033aa` | `#aa0055` | `border-frame` | Bordas estruturais: `MainPanel` (esquerda), `Footer` (topo), `PartyTabs`/`PartyTab` |
| `progress-frame` | `#00154a` | `#55002b` | `bg-progress-frame`, `border-progress-frame` | Moldura chanfrada dos cartões e do mapa (1.5px), borda das barras, `TrainingPoints`, `DigievolutionsButton`, caixa do level |
| `highlight` | `oklch(97.7% 0.013 236.62)` (`sky-50`) | `oklch(97.1% 0.014 343.198)` (`pink-50`) | `ring-highlight` | Anel do botão ativo do `Footer` |
| `track` | `#000e3f` | `#3f0022` | `bg-track` | Trilho das barras, `TrainingPoints`, `DigievolutionsButton`, caixa do level |
| `track-hover` | `#002a77` | `#77002a` | `hover:bg-track-hover` | Hover do `DigievolutionsButton` |

## Utilitários globais usados (`App/src/style.css`, fora da `ui-next`)

- `text-dw3-gold`: degradê vertical `#ffcc00` → `#ff6600` (bônus em `PartyStat`, digievolução ativa em `PartyDigievolution`).
- `shadow-text` (`1px 1px 0 #000`), `shadow-text-dark`, `text-outline-black`, `text-outline-black-glow`.
- `dw3-beveled` (chanfro 4px).
- Texto padrão branco (`color: white` no `body`).

## Cores fixas nos componentes

| Componente | Cor | Uso |
|---|---|---|
| `MapTitle` | `text-gray-200` + `drop-shadow-[0_0_5px_rgba(0,170,255,0.8)]` | Título do mapa |
| `MobiusDesertMap` | `text-sky-300` + `drop-shadow-[0_0_5px_rgba(0,170,255,0.8)]` | Zona segura |
| `MobiusDesertExit` | `text-yellow-300` | Saídas |
| `SeabedMap` | `text-cyan-100/200/300` | Saídas e rotas |
| `MapFrame` / `MapPanel` | `bg-black`, degradê `from-black/85 via-black/50` | Fundo e topo do mapa |
| `MapPanel` | `oklch(92.4% 0.12 95.746)` / `oklch(88.2% 0.059 254.128)` / `oklch(92.5% 0.084 155.995)` | Origem dos inimigos (andando / pesca / árvore) |
| `MapEnemyGroup` | `#f87171` | Nome dos inimigos |
| `DigimonBattleMap` | `#00B6BF` / `#A3D956` / `#CB9200` / `#760F08` | Status (healthy / injured / debuffed / ko); `bg-black/80` no cabeçalho |
| `ExperienceProgressBar` | `from-orange-600 to-yellow-500` | Barra de EXP |
| `HpProgressBar` | `bg-green-500` / `bg-red-500` (≤ 30%) | Barra de HP |
| `MpProgressBar` | `bg-blue-600` / `bg-yellow-400` (≤ 30%) | Barra de MP |
| `BlastProgressBar` | `bg-yellow-200`, `border-black` | Barra de Blast |
| `PartyDigievolution` | `bg-blue-400` (DVEXP); tiers `gray-400` / `green-500` / `blue-500` / `purple-500` / `amber-400`; DNA `bg-red-500`; estrelas `text-yellow-300`; vazio `text-white/60` | Cartões de digievolução |
| `PartyStat` | `text-green-400` (buff), `text-red-400` (debuff) | Valores |
| `PartyStats` | Cor por emoji (ex.: força `text-yellow-300`, fogo degradê vermelho→amarelo) | Ícones dos stats |
| `PartyEquipment` | `text-white` / `text-white/25` (vazio) | Ícones |
| `DigimonVitals` | `text-gray-300` | Rótulo "Lv" |
| `TrainingPoints` | `text-red-500` | Luva de boxe |
| `DigievolutionsButton` | degradê `from-pink-400 to-green-400` | Ícone de DNA |
| `EmulatorSlot` | `text-red-400` | Mensagem de erro |
