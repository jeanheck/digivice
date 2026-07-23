# Digivice — Frontend Review

> **Data:** 22 de julho de 2026  
> **Escopo:** apenas `Frontend/` (com `AI/*.md` como balizador; menções a Backend só quando o contrato SignalR importa)  
> **Referências:** `AI/CODE_RULES.md`, `AI/BUSINESS_RULES.md`  
> **Pedidos especiais deste review:** (1) viabilidade e desenho de **tema de cores por mapa/região**; (2) inventário de **CSS puro vs Tailwind**  
> **Métricas aproximadas:** ~86 `.vue`, ~299 `.ts` de lógica, 1 `.css`, ~37 JSONs de database, 2 locales i18n espelhados

---

## 1. Sumário executivo

O Frontend Digivice é um app Vue 3 + Pinia + TypeScript empacotado com Tauri, alimentado em tempo real por SignalR e enriquecido por dados estáticos (JSON → Repository → Presenter → ViewModel). A arquitetura descrita em `BUSINESS_RULES.md` e `CODE_RULES.md` está **bem materializada no código**: pipeline de eventos limpo, syncers com regra de ouro, camadas de dados estáticos maduras, tooltips/modais padronizados, e quase zero CSS scoped.

O elo fraco — e o que mais trava a feature futura de **trocar o tema azul conforme o mapa** — é a **ausência total de design tokens**. Cores vivem como hex literais (`bg-[#0077ff]`, `#000a2b`, dezenas de variantes próximas) espalhados em ~35 componentes + `style.css`. Não há CSS variables, não há `@theme` do Tailwind v4, não há “fonte única da verdade” cromática. Já existe, porém, um padrão local excelente (`journal-section-palette.ts`) e um gancho de domínio pronto (`LocationRegionConstant` + `LocationService.getRegionByLocationId`).

**Nota geral sugerida: B+ / 8.0** — arquitetura e regras de negócio fortes; styling/theming e higiene (aspas, deps mortas, CDN de bandeiras) são onde mais se ganha com investimento.

---

## 2. Metodologia

| Fonte | Uso |
|-------|-----|
| `AI/BUSINESS_RULES.md` | Fluxo SignalR, Party, Digievolution, Journal, nomes próprios |
| `AI/CODE_RULES.md` (Frontend + dados estáticos + tooltips + modais) | Convenções, camadas, estilo visual |
| `Frontend/src/**` | Arquitetura, padrões, outliers |
| Pedido do usuário | Tema por mapa; CSS vs Tailwind |

Dimensões: arquitetura, negócio, CODE_RULES, **theming**, **CSS×Tailwind**, segurança, i18n, cheiros, manutenibilidade.

---

## 3. Visão arquitetural

### 3.1. Mapa de pastas (`src/`)

```
App.vue / main.ts
style.css                 → único CSS global (Tailwind v4 + utilitários DW3)
config/                   → APP_CONFIG (porta hub, flags de ambiente)
events/                   → SignalR, DTOs, converters DTO→Model
stores/                   → useGameStore + syncers (mutação incremental)
models/                   → domínio reativo
database/ + repositories/ → JSON estático → Raw
presenters/ + converter/ + helper/
viewmodels/
services/                 → regras de domínio reutilizáveis
components/               → UI por feature
composables/              → tooltip position, map frame, blocking error
i18n/                     → en-US / pt-BR
catalogs/                 → ImageCatalog (glob de assets)
constants/
```

### 3.2. Dois pipelines (intencionais e bem separados)

```
Tempo real:
  SignalR → handlers → Store (InitialState via events/converters)
                    → syncers (DTO parcial → Model in-place)
                    → componentes consomem useGameStore diretamente

Dados estáticos:
  Component → Presenter → (Service?) → Repository → Raw
                 └────────→ Converter → ViewModel
```

Isso casa com `CODE_RULES.md` (dois contextos de converter) e com `BUSINESS_RULES.md` §1.

### 3.3. Store única

`useGameStore` concentra conexão (backend/emulador/erros) + `currentState: { player, party, journal }`. Sem controllers intermediários — single source of truth, como a doc pede.

### 3.4. Pontos fortes estruturais

- Componentes pequenos (maior `.vue` ~184 linhas).
- Tooltips e modais seguem o fluxo obrigatório quase 100%.
- Presenters/helpers/converters em geral corretos.
- Region já existe no domínio de mapa (`LocationRaw.region`, default `asukaServer`).
- Cerimônia alta ao adicionar campo SignalR (DTO + converter + model + syncer) — **por design**, previsível.

---

## 4. Regras de negócio (`AI/BUSINESS_RULES.md`)

| Regra | Status | Evidência |
|-------|--------|-----------|
| InitialState + sync incremental | ✅ | `setInitialState` + `sync*` |
| Syncer: `undefined` = no-op | ✅ (quase) | Strict `!== undefined` na maioria; `digievolution-slot.syncer` usa `!=` frouxo |
| Party: 3 slots; UI só ocupados | ✅ | `PartyPresenter.getFilledSlots` |
| Slot: digimonId/digimon ambos null ou ambos set | ✅ | Syncer esvazia em par |
| DigimonId no slot, não no Digimon | ✅ | Modelo correto |
| Digievolution: sem filled→empty no syncer | ✅ | Só escreve quando id/objeto definidos |
| Nomes Digimon/Digievolution sem i18n | ✅ | Literais via repository |
| Técnicas com i18n | ✅ | `technique.{id}.name` |
| Level sem clamp | ✅ | Sem Math.min/max no level |
| Blast por Digimon | ✅ | Campo no model Digimon |
| Journal fixo; conclusão no frontend | ✅ | Syncer só `.find`; `isDone` no QuestConverter (presenter path) |
| Player name não rastreado | ✅ | Ausente do model |

**Insight:** no Frontend, as invariantes de Digievolution/Party estão **mais defendidas** do que no Backend (que confia na RAM). O syncer de digievolution simplesmente não aplica null — alinhado à regra “proibido filled→empty”.

**Bug de i18n (negócio/UX):** `Profile.vue` hardcoda `"Nv {{ digimon.level }}"` em vez de chave i18n — usuários `en-US` veem abreviação portuguesa.

---

## 5. Qualidade vs `AI/CODE_RULES.md`

### 5.1. Scorecard

| Regra | Nota | Situação |
|-------|------|----------|
| Converters (nome/arquivo/`convert`) | A | Consistente nos dois contextos |
| DTOs / pastas kebab | A | OK |
| ViewModels 1 tipo/arquivo | A | OK |
| Imports `@/` | A- | Store/events antigos ainda usam relativos |
| Aspas duplas | C+ | ~33 arquivos (pipeline SignalR/models) ainda com aspas simples; Prettier já está `singleQuote: false` |
| Sem testes no Frontend | A | Sem specs; **porém** scaffolding Vitest ainda no `package.json` |
| Camadas estáticas | A- | 1 vazamento de tipo Raw em componente |
| Tooltips | A | Padrão seguido |
| Modais | A | Todos usam `Modal.vue` |
| Coerência visual / tokens | C | `dw3-*` usados, mas hex inventados demais |
| Semicolon | A- | Em geral OK no TS moderno |

### 5.2. Violações / drifts concretos

1. **`SeabedDockLabel.vue`** importa tipo de `@/repositories/tables/raws/...` — deveria usar só ViewModel (o viewmodel já reexporta o tipo).
2. **`LanguageSelector.vue`** — único `<style scoped>` + aspas simples + CDN externo.
3. **Aspas simples** concentradas em `events/`, `stores/`, `models/`, `repositories/index.ts`.
4. **`Constant` god-enum** mistura attrs/elementos/condições/cantos de mapa; enums estreitos já existem (`AttributeConstant`, `ElementConstant`, `SeabedConstant`) mas o legado ainda é importado em Stats/Equipment/Technique/Map/Seabed/Auction.
5. **Barrel `repositories/index.ts`** renomeia (`QuestRepository as JournalRepository`, etc.) — dois nomes para a mesma classe.
6. **Painéis reimplementados** — `Footer.vue` / `Digimon.vue` montam `bg-[#000a2b] border-2 border-[#0033aa]` em vez de estender `dw3-aside`/`dw3-panel` (a regra visual pede preferência aos `dw3-*`).
7. **Deps mortas:** `vue-router` sem uso; scripts/deps Vitest sem testes reais (contraria a decisão documentada de “sem testes no Frontend”).

### 5.3. Áreas limpas (vale registrar)

Converters, DTOs, ViewModels, tooltips, modais e ausência de `v-html` estão em excelente estado.

---

## 6. Tema de cores por mapa — análise e proposta

> Pedido: no futuro, o tema (hoje azul fixo) deve mudar conforme o mapa/região do jogador. Avaliar estrutura atual + boas práticas de mercado.

### 6.1. O que existe hoje

| Peça | Estado |
|------|--------|
| Cor dominante | Azul DW3 hardcoded (`#0077ff`, `#0033aa`, `#00aaff`, `#000a2b`, `#000e3f`, …) |
| CSS variables / `@theme` | **Inexistentes** |
| Tailwind config clássico | Não há (`tailwind.config.*` ausente — normal no v4 CSS-first, mas `@theme` também não foi usado) |
| Variantes hex “quase iguais” | **40+** literais em ~35 `.vue` + `style.css` |
| Troca de UI por região | **Já existe** para *conteúdo* do mapa: `Map.vue` escolhe `SeabedMap` / `MobiusDesertMap` / `AsukaServerMap` via `LocationRegionConstant` |
| Troca de *cor* por região | Informal no Seabed (classes `cyan-*` em docks/rotas); Journal tem palette por seção (`cyan`/`teal`/`sky`) |
| Fonte da região | `LocationRaw.region` + `LocationService.getRegionByLocationId(player.location)` |

**Conclusão curta:** trocar o *mapa desenhado* já é barato; trocar o *cromático do Digivice inteiro* é caro **hoje**, porque a cor não está indirectionada.

### 6.2. Boas práticas de mercado (relevantes aqui)

Para apps Vue/React + Tailwind em 2025–2026, o padrão maduro de multi-theme é:

1. **Design tokens semânticos**, não nomes de cor literal  
   Preferir `--color-panel`, `--color-border`, `--color-accent`, `--color-aside-bg` a `--blue-500`. O significado (“borda do painel”) sobrevive à troca de tema; “azul 500” não.
2. **Uma camada de tokens → utilitários**  
   No Tailwind v4: bloco `@theme` (ou `@theme inline`) mapeando tokens para classes (`bg-panel`, `border-accent`, `text-accent`). Componentes usam **só** essas classes semânticas / utilitários `dw3-*` baseados em `var(--…)`.
3. **Troca por atributo/classe no root**  
   `data-theme="seabed"` / `data-theme="mobius-desert"` / `data-theme="asuka"` em `<html>` ou `<main>`, com seletores:
   ```css
   :root, [data-theme="asuka"] { --color-accent: #0077ff; ... }
   [data-theme="seabed"] { --color-accent: #22d3ee; ... }
   [data-theme="mobius-desert"] { --color-accent: #f59e0b; ... }
   ```
   Isso é o mesmo modelo usado por shadcn/ui, Radix themes, muitos design systems (CSS variables + data-attribute).
4. **Não ramificar tema com `v-if` de classes em cada componente**  
   Evitar ` :class="region === 'seabed' ? 'border-cyan-400' : 'border-blue-400'"` em dezenas de arquivos — vira explosão combinatória. Palette lookup (como `journal-section-palette.ts`) é aceitável para **acentos locais**; para o tema global do shell, CSS variables vencem.
5. **Transição opcional**  
   `transition: background-color .2s, border-color .2s` nos elementos de chrome suaviza a troca ao mudar de mapa (cuidado com performance em muitos nós; aplicar no shell/`dw3-*` basta).
6. **Granularidade**  
   Começar por **região** (`LocationRegionConstant`: 3 temas) é o sweet spot. Tema por `locationId` individual (centenas de mapas) só se houver motivo forte — custo de design e manutenção explode.

### 6.3. O que NÃO fazer (anti-padrões no contexto Digivice)

- Trocar hex com find/replace manual a cada região sem tokens.
- Duplicar `style.css` inteiro por tema.
- Colocar lógica de cor dentro de Syncers/Models (cor é apresentação).
- Misturar “tema global” com as palettes de seção do Journal sem hierarquia clara (Journal pode manter accents próprios *sobre* o tema base).

### 6.4. Arquitetura recomendada para o Digivice

```
player.location (Pinia)
  → LocationService.getRegionByLocationId
  → themeId: "asuka" | "seabed" | "mobius-desert"
  → App.vue (ou composable useDigiviceTheme) seta data-theme no <main>
  → style.css: tokens por [data-theme]
  → @theme / dw3-* / utilitários semânticos consomem var(--color-*)
  → componentes já migrados não mudam no tick de mapa
```

**Espelhar o padrão que já funciona:**

`journal-section-palette.ts` prova que o time sabe fazer lookup de classes semânticas. Para o shell global, preferir **CSS variables** (menos churn de strings Tailwind e melhor para `style.css` / animações / scrollbar / tree connectors). Manter palette TS só onde a UI precisa de *conjuntos de classes Tailwind diferentes* (ex.: títulos de seção do journal).

### 6.5. Plano de migração em fases (esforço realista)

#### Fase 0 — Decisão de produto (curta)
- Temas = 3 regiões (recomendado) ou por location?
- Paletas: Asuka = azul atual; Seabed = ciano/teal (já há hint no UI); Mobius = âmbar/areia.
- O que muda: chrome (aside, panel, borders, títulos, scroll, cantos do Map, footer) vs. também cards internos / tooltips / modais.
- O que **não** muda: cores semânticas de status (vermelho erro, amarelo blast gauge, dourado digievolução ativa) — tokens separados (`--color-danger`, `--color-blast`, `--color-gold`).

#### Fase 1 — Canonicalizar (pré-requisito do tema)
Sem isso, tema-by-map é frágil.

1. Definir o set canônico (~6–8 tokens semânticos), alinhado ao que o CODE_RULES já cita:
   - `canvas` / `panel` / `aside` / `border` / `accent` / `accent-bright` / `grid` / `text-muted`
2. Colapsar as 40+ variantes hex para esses tokens (decisão visual humana: `#000a1a` ≈ `#000a2b`?).
3. Introduzir em `style.css`:
   ```css
   @theme {
     --color-dw3-panel: var(--digivice-panel);
     --color-dw3-accent: var(--digivice-accent);
     /* ... */
   }
   :root {
     --digivice-panel: #000a2b;
     --digivice-accent: #0077ff;
     /* tema default = asuka */
   }
   ```
4. Reescrever `.dw3-panel*`, `.dw3-aside*`, `.custom-scroll`, `.bg-grid-pattern`, connectors da tree para usar `var(--digivice-*)`.

#### Fase 2 — Migrar componentes
- Trocar `bg-[#000a2b]` → `bg-dw3-panel` (ou classe utilitária equivalente gerada pelo `@theme`).
- Extrair blocos duplicados (ex.: o trio `MapDetailsFrame` / `SeabedDocks` / `ZoomedLocationMap` com o mesmo `bg-[#00051a] border-cyan-800/50 ...`) para uma classe compartilhada *já tokenizada*.
- Prioridade: `App` shell → `dw3-*` consumers → Map chrome → Footer → Party cards → Modais/Tooltips.

#### Fase 3 — Wire do tema por região
```ts
// composables/use-digivice-theme.ts (esboço)
const region = computed(() =>
  LocationService.getRegionByLocationId(store.currentState?.player?.location ?? null)
);
const theme = computed(() => regionToTheme(region.value)); // asuka | seabed | mobius-desert
// no App.vue: <main :data-theme="theme">
```
```css
[data-theme="seabed"] {
  --digivice-panel: #001018;
  --digivice-aside: #001a22;
  --digivice-border: #0e7490;
  --digivice-accent: #22d3ee;
  --digivice-accent-bright: #67e8f9;
}
[data-theme="mobius-desert"] { /* âmbar / areia */ }
```

#### Fase 4 — Polimento
- Transição suave no chrome.
- Revisar Seabed `cyan-*` hardcoded: ou absorver no tema seabed, ou marcar como “accent local permanente”.
- Documentar tokens + regra “proibido novo `bg-[#hex]` para chrome” no `CODE_RULES.md`.

### 6.6. Estimativa de dificuldade

| Abordagem | Dificuldade hoje | Dificuldade após Fase 1–2 |
|-----------|------------------|---------------------------|
| Só trocar imagem de mapa | Já feito | — |
| Tema global por região via tokens | **Alta** (refactor cromático) | **Baixa** (só CSS + 1 composable) |
| `v-if` de classes por região em cada SFC | Alta e piora com o tempo | Evitar |

**Insight:** o investimento certo não é “feature de tema” primeiro — é **design system mínimo (tokens)**. A feature de mapa vira um byproduct barato.

### 6.7. Encaixe com a estrutura atual (por que encaixa bem)

- `player.location` já está na store e sincroniza via SignalR.
- `LocationService.getRegionByLocationId` já resolve região.
- `Map.vue` já reage à região para escolher submapa — o mesmo `computed` alimenta o `data-theme`.
- `journal-section-palette` é template mental para accents locais.
- Tailwind v4 + único `style.css` é o lugar ideal para tokens (não precisa de novo build pipeline).

---

## 7. CSS puro vs Tailwind — inventário e diretrizes

> Pedido: componentes devem usar Tailwind salvo exceções reais; listar CSS onde Tailwind resolveria.

### 7.1. Panorama

| Artefato | Qtd | Nota |
|----------|-----|------|
| Arquivos `.css` | **1** (`src/style.css`, ~440 linhas) | Única folha global |
| `.vue` com `<style>` | **1** (`LanguageSelector.vue`) | Único outlier scoped |
| Estilo nos demais 85 `.vue` | Tailwind inline + classes `dw3-*` / utilitários globais | Disciplina alta |

O projeto **já está** muito próximo da meta “só Tailwind + utilitários globais”. O problema não é proliferação de CSS scoped — é (a) hex arbitrários no Tailwind e (b) trechos em `style.css` que poderiam ser utilitários ou tokens.

### 7.2. `LanguageSelector.vue` — CSS que Tailwind resolve 100%

```33:40:Frontend/src/components/footer/LanguageSelector.vue
<style scoped>
button {
  background: none;
  border: none;
  padding: 2px 4px;
  line-height: 1;
}
</style>
```

**Substituir por** classes nos botões: `bg-transparent border-0 px-1 py-0.5 leading-none` (e remover o bloco `<style>`).

Este é o **único arquivo de componente** com CSS onde Tailwind resolve de forma direta.

### 7.3. `style.css` — o que deve permanecer CSS (exceções legítimas)

Estes padrões **não** são bons candidatos a “jogar tudo no class string do Vue”; manter em CSS global é correto:

| Bloco | Linhas (aprox.) | Por quê CSS |
|-------|-----------------|-------------|
| `html, body` reset + font + bg animado | 4–31 | Global app shell; `@keyframes slideBackground`; `image-rendering` |
| `.dw3-beveled` (`clip-path`) | 34–36 | clip-path poligonal — possível via arbitrary Tailwind, mas frágil; utilitário nomeado é melhor |
| `.text-dw3-gold` (gradient text clip) | 80–85 | `-webkit-background-clip` + transparent — CSS dedicado é mais claro |
| Digievolution tree (`.connector*`, `.fork-*`, `::before`/`::after`) | 123–250 | Pseudo-elementos, gradientes de linha, posicionamento percentual — Tailwind sozinho fica ilegível |
| `.custom-scroll::-webkit-scrollbar*` | 252–269 | Pseudo-elementos de scrollbar (WebKit) — CSS only |
| Vue transitions `.fade-*`, `.modal-fade-*` | 271–290 | Convenção de nomes do `<Transition>` |
| `@keyframes` modal / auction / blast-gauge | 292–384 | Animações nomeadas referenciadas por classes curtas |
| Classes de animação (`.auction-card-active`, `.blast-gauge-fill-high`, …) | 332–384 | Couplings com keyframes |

**Regra prática:** keyframes, pseudo-elementos complexos, clip-path de marca e scrollbar = CSS global OK.

### 7.4. `style.css` — trechos onde Tailwind (ou tokens + utilitário curto) resolveria melhor

| Bloco | Situação | Sugestão |
|-------|----------|----------|
| `.dw3-panel` / `-border` / `-inner` / `-content` | Hex fixos + layout absoluto | Manter classes (API boa), mas **cores via `var(--digivice-*)`**; opcionalmente expressar parte do layout em `@apply` se quiser |
| `.dw3-aside` / `-header` / `-title` | Quase 100% layout+borda expressável em Tailwind | Candidato a `@apply` **ou** documentar como “componente CSS intencional”; hoje viola um pouco o “só Tailwind nos componentes”, mas como **utilitário compartilhado** ainda é aceitável — o importante é tokenizar cores |
| `.family-tree-container`, `.empty-state`, `.family-row`, `.branch-row`, … | Muitos `display:flex` / padding genéricos | Vários poderiam ser Tailwind nos SFCs da tree; connectors devem ficar CSS |
| `.map-info-panel`, `.map-info-panel-fit`, `-action`, `:hover` | Layout + borda + blur repetidos | **Fortes candidatos a Tailwind** (`bg-black/80 border border-sky-400/40 rounded backdrop-blur-sm …`) **ou** uma classe tokenizada única; há duplicação panel vs panel-fit |
| `.bg-grid-pattern` | Gradientes de grid | Pode ficar CSS (ok) desde que `#0055ff` vire token `--digivice-grid` |
| `.shadow-text`, `.text-outline-black`, `.shadow-text-dark` | text-shadow / drop-shadow | Manter utilitários nomeados (melhor DX que strings longas) |

### 7.5. “CSS disfarçado”: hex arbitrários no Tailwind

Mesmo sem `<style>`, muitos componentes usam CSS “puro” via arbitrary values. Para a meta do usuário (“Tailwind, não CSS puro”), o espírito inclui: **preferir tokens/utilitários nomeados a `bg-[#aabbcc]`**.

Arquivos com hex hardcoded (amostra priorizada para migração de tema / limpeza de palette):

| Arquivo | Papel |
|---------|-------|
| `style.css` | Maior concentração (~25 hits) |
| `Map.vue` | Cantos HUD `#00aaff` |
| `SearchBar.vue` | Vários blues de input/panel |
| `JournalQuestCard.vue` | Mapa status→hex inline |
| `EnemyModal.vue` / `EnemyProfile.vue` | `#000a1a` |
| `QuestModal.vue` / `StepPanel.vue` | `#000a1a` / vermelho escuro |
| `Modal.vue` / `Tooltip.vue` | Fundos/bordas próprios |
| `Footer.vue` / `Digimon.vue` / `Profile.vue` | Chrome party/footer |
| `Equipment.vue` / `Technique.vue` / `EquipmentsTooltip.vue` / `StatsTooltip.vue` | Acentos |
| `DigievolutionTechniques.vue` / `Node.vue` | Palettes locais |
| `MapDetailsFrame.vue` / `SeabedDocks.vue` / `ZoomedLocationMap.vue` | Bloco duplicado `#00051a` |
| Auction cards (`AuctionAvailable`, `AuctionCardAvailable`, …) | `#001a2a` repetido |
| `LanguageSelector.vue` | `ring-offset-[#000a2b]` |
| `AppErrorScreen.vue`, `ProgressBar.vue`, `Icon.vue`, `TrainingPoints.vue`, `DigievolutionsButton.vue`, `Enemies.vue`, `Location.vue` (glow), `MobiusDesertArea.vue`, `SeabedRouteLines.vue` | Hits pontuais |

**Lista completa de `.vue` com match de `#hex`:** ~40 arquivos sob `components/` (ver grep do review). Nenhum além de `LanguageSelector` tem bloco `<style>`; o “CSS indevido” aqui é **arbitrary hex**, não stylesheet separado.

### 7.6. Diretriz recomendada (para `CODE_RULES.md` futuro)

1. **Proibido** novo `<style scoped>` salvo pseudo-elemento impossível em utilitário.
2. **Proibido** novo `bg-[#...]` / `border-[#...]` / `text-[#...]` para chrome DW3 — usar token (`bg-dw3-panel`, `border-dw3-accent`, …).
3. **Permitido** CSS em `style.css` para: keyframes, scrollbar, clip-path de marca, tree connectors, Vue transition class names, utilitários `dw3-*` tokenizados.
4. **Permitido** Tailwind palette padrão (`cyan-400`, `red-500`) para significado semântico que **não** é tema de mapa (erro, sucesso, accents de journal section via palette TS).

---

## 8. Segurança e robustez

| # | Achado | Severidade |
|---|--------|------------|
| 1 | Bandeiras via `https://flagcdn.com` em app desktop/Tauri | Média (offline + privacy + CSP futuro) |
| 2 | Ingress SignalR tipado como `any`; sem validação de shape | Baixa (localhost); risco de UI crash |
| 3 | `startConnection()` sem `.catch` explícito em `main.ts` | Baixa |
| 4 | `(window as any).__TAURI_INTERNALS__` / `IS_TAURI` pouco usado | Higiene |
| 5 | Sem `v-html` / `innerHTML` | Positivo — superfície XSS ~zero |

**Ação clara:** bundlar SVGs/PNGs de bandeiras em `assets/` como o resto dos ícones.

---

## 9. i18n

- `vue-i18n` Composition API; locales `en-US` / `pt-BR` espelhados.
- Nomes próprios corretamente fora de i18n.
- **Gap:** `"Nv"` hardcoded em `Profile.vue`.
- Locale só no client (`localStorage`) — adequado.

---

## 10. Cheiros e inconsistências adicionais

1. **Dois “tamanhos canônicos” de mapa:** `MAP_DISPLAY_WIDTH_PX = 512` (quest pins) vs `MAP_FRAME_WIDTH_PX = 600` (frames de detalhe). Provavelmente intencional, mas acopla JSON de coordenadas ao 512 — documentar a distinção.
2. **Footer.vue** — 4 trios idênticos show/move/hide tooltip (~60 linhas) → helper parametrizado.
3. **Map.vue** — 4 cantos HUD idênticos → classe `.dw3-scan-corner` ou componente.
4. **Imports relativos** na store/events vs `@/` no resto.
5. **Digievolution-slot syncer** com `!= undefined` frouxo vs strict no restante.
6. **Cerimônia SignalR** (4–6 arquivos por campo) — custo aceito; skills do repo mitigam.

---

## 11. Achados priorizados

### P0

| ID | Achado |
|----|--------|
| P0-1 | Bundlar bandeiras (remover CDN) |
| P0-2 | i18n do label de level (`Nv` → chave) |

### P1 — Pré-requisitos do tema por mapa + qualidade

| ID | Achado |
|----|--------|
| P1-1 | Introduzir design tokens + `@theme` + reescrever `dw3-*` com `var(--)` |
| P1-2 | Canonicalizar hex duplicados → tokens |
| P1-3 | Composable `useDigiviceTheme` + `data-theme` no `App.vue` (após P1-1) |
| P1-4 | Remover `<style scoped>` do `LanguageSelector` (Tailwind) |
| P1-5 | Documentar no CODE_RULES: tokens obrigatórios; proibir novos hex de chrome |

### P2

| ID | Achado |
|----|--------|
| P2-1 | `npm run format` nos arquivos com aspas simples |
| P2-2 | Aposentar `Constant` god-enum |
| P2-3 | Remover `vue-router` e scaffolding Vitest (ou documentar exceção) |
| P2-4 | `SeabedDockLabel` não importar Raw |
| P2-5 | Extrair bloco duplicado MapDetailsFrame/SeabedDocks/ZoomedLocationMap |
| P2-6 | Preferir `dw3-aside`/`dw3-panel` em Footer/Digimon |
| P2-7 | Tokenizar `.map-info-panel*` ou migrar para Tailwind |
| P2-8 | Normalizar syncer digievolution para `!== undefined` |

### P3

| ID | Achado |
|----|--------|
| P3-1 | Helper de tooltips no Footer |
| P3-2 | Classe/componente para cantos do Map |
| P3-3 | Runtime validation leve na borda SignalR |
| P3-4 | Unificar naming do barrel de repositories |
| P3-5 | Documentar 512 vs 600 map widths |

---

## 12. Pontos fortes

1. Arquitetura em camadas **realmente** seguida (não só no papel).
2. Syncers + Optional semantics alinhados ao Backend.
3. Digievolution/Party rules bem defendidas no cliente.
4. Quase zero CSS scoped — disciplina rara.
5. Modais/tooltips padronizados.
6. Region/map switching de *conteúdo* já pronto — base do tema futuro.
7. `journal-section-palette` como prova de conceito de theming local.
8. Componentização saudável (arquivos pequenos).

---

## 13. Insights transversais

### 13.1. O Frontend é mais “guardião de domínio” que o Backend
O Backend espelha RAM; o Frontend aplica regras de sync (não esvaziar digievolution, parear slots, calcular `isDone`). Isso é saudável para UX — desde que documentado na matriz (B)/(F)/(G) sugerida no review de Backend.

### 13.2. Theming é dívida estrutural, não cosmético
Enquanto a cor for literal, qualquer feature de “ Digivice muda com o mapa” será um rewrite cromático disfarçado de feature. Tokens primeiro.

### 13.3. Tailwind-first já é a cultura — falta a regra escrita e os tokens
O time quase não usa CSS de componente. Formalizar “hex de chrome = token” no CODE_RULES evita regressão enquanto o tema por mapa é implementado.

### 13.4. Duas gerações de código
Pipeline SignalR/models (aspas simples, imports relativos) vs presenters/map/modais novos (`@/`, aspas duplas, palette TS). Mesmo padrão geracional visto no Backend (Converters/Diffing legados).

### 13.5. Offline-first incompleto
App de emulador desktop ainda depende de CDN para bandeiras — desalinhado com o restante dos assets locais.

---

## 14. Recomendações práticas (ordem)

1. Bundlar bandeiras + i18n do “Nv”.
2. Definir paleta canônica (Fase 0–1 do tema) e tokens em `style.css` / `@theme`.
3. Migrar `dw3-*` + top offenders de hex para tokens.
4. Ligar `data-theme` à região do jogador.
5. Limpar LanguageSelector CSS; formatar aspas; podar deps mortas.
6. Atualizar `CODE_RULES.md` com tokens + regra anti-hex de chrome + nota sobre tema por região.

---

## 15. Nota final por dimensão

| Dimensão | Nota | Comentário |
|----------|------|------------|
| Arquitetura / fluxo | 9.0 | Dois pipelines claros e corretos |
| Regras de negócio | 8.5 | Fortes no cliente; gap i18n level |
| CODE_RULES (estrutura) | 8.5 | Camadas/tooltips/modais excelentes |
| CODE_RULES (visual/tokens) | 6.0 | dw3-* ok; hex caótico |
| Tema por mapa (prontidão) | 4.5 | Domínio pronto; styling não |
| CSS×Tailwind higiene | 8.0 | Quase só Tailwind; 1 scoped + style.css misto |
| Segurança / offline | 7.0 | CDN bandeiras é o ponto |
| i18n | 8.0 | Sólido; “Nv” quebra |
| Manutenibilidade | 8.0 | Previsível; cerimônia SignalR alta |
| **Geral Frontend** | **8.0 / B+** | Base excelente; tokens desbloqueiam o futuro |

---

## 16. Apêndice A — Esboço mínimo de tokens (referência)

Sugestão inicial alinhada ao azul atual (Asuka) e às duas outras regiões:

| Token | Asuka (default) | Seabed | Mobius Desert |
|-------|-----------------|--------|---------------|
| `--digivice-canvas` | `#000030` | `#000a12` | `#1a1008` |
| `--digivice-panel` | `#000a2b` | `#001018` | `#1a120a` |
| `--digivice-aside` | `#000e3f` | `#001a22` | `#24180c` |
| `--digivice-border` | `#0033aa` | `#0e7490` | `#b45309` |
| `--digivice-accent` | `#0077ff` | `#22d3ee` | `#f59e0b` |
| `--digivice-accent-bright` | `#00aaff` | `#67e8f9` | `#fbbf24` |
| `--digivice-grid` | `#0055ff` | `#0891b2` | `#d97706` |

(Valores ilustrativos — calibrar com design/pixel art real.)

Tokens **fora** do tema de mapa (fixos):

| Token | Uso |
|-------|-----|
| `--digivice-gold` | digievolução ativa / destaque |
| `--digivice-blast` | fúria |
| `--digivice-danger` | erros / steps falhos |

---

## 17. Apêndice B — Checklist rápido “pronto para tema por mapa”

- [ ] Tokens semânticos em `:root`
- [ ] `@theme` expondo cores ao Tailwind
- [ ] `dw3-*` / scroll / grid / tree accents usam `var(--)`
- [ ] Zero (ou quase) `bg-[#` de chrome nos componentes
- [ ] `useDigiviceTheme` + `:data-theme` no shell
- [ ] Três blocos `[data-theme="…"]` calibrados
- [ ] CODE_RULES atualizado
- [ ] Journal accents documentados como camada acima do tema base
- [ ] Blast/gold/danger não mudam com o mapa

---

*Fim da review do Frontend. Documento irmão: `AI_REVIEW/backend/BACKEND_REVIEW.md`.*
