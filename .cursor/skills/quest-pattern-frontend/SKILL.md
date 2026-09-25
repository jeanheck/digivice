---
name: quest-pattern-frontend
description: >-
  Integrates journal quests into the Digivice frontend — static quest JSON,
  repository, i18n, presenters and Journal UI, plus SignalR sync when a new
  journal category is created. Use when the user asks to add a quest to an
  existing journal category (side quest, legendary weapon, DRI agent, Duel
  Island) or to wire a brand-new journal category on the frontend.
---

# Quest Pattern — Frontend Integration

Wire journal quest data from static JSON through **Journal UI**. Does not touch
backend C#.

**User responsibility:** backend already emits the quest in `InitialState` and
`JournalChanged`. Do **not** invoke or verify `quest-pattern-backend`.

**No frontend tests** (project rule).

## When NOT to use

| Case | Use instead |
|------|-------------|
| Player / Party / Digimon field sync | `address-field-frontend` |
| New non-journal entity in the store | `memory-entity-frontend` |
| Backend definitions / loaders | `quest-pattern-backend` |

## Reuse rule (critical)

Quest entities are shared across all categories. **Do not** create per-quest
DTOs, models, syncers or card components.

| Layer | Reuse as-is |
|-------|-------------|
| DTOs | `QuestDTO`, `StepDTO`, `RequisiteDTO` |
| Models | `Quest`, `Step`, `Requisite` |
| Events converters | `QuestConverter`, `StepConverter`, `RequisiteConverter` (`events/converters/journals/`) |
| Syncers | `QuestSyncer`, `StepSyncer`, `RequisiteSyncer` (`stores/syncers/journals/`) |
| Presenter converter | `QuestConverter` (`presenters/converter/quest.converter.ts`) |
| UI | `JournalQuestCard`, `JournalQuestsSection`, `QuestModal` tree |

## Pick the workflow

```
New quest id, category already on Journal?  → Workflow B (below)
New property on Journal?
  → At least one quest id ready?
      NO  → Stop; ask for the first quest (no empty sections)
      YES → Workflow A: workflow-new-category.md
  → Accent color given by the user?
      NO  → Ask (Tailwind palette name, not already used)
```

## Category map (all wired)

| Category | `Journal` property | Static JSON folder | Accent | Repository getter |
|----------|-------------------|--------------------|--------|-------------------|
| Main quest | `mainQuest` (single) | `database/quest/main-quest.json` | yellow (hardcoded) | `getMainQuestRaw()` |
| Side quests | `sideQuests` | `database/quest/side-quest/` | `emerald` | `getSideQuestsRaw()` |
| Legendary weapons | `legendaryWeapons` | `database/quest/legendary-weapons/` | `teal` | `getLegendaryWeaponsRaw()` |
| DRI agents | `driAgents` | `database/quest/dri-agents/` | `cyan` | `getDriAgentsRaw()` |
| Duel Island | `duelIsland` | `database/quest/duel-island/` | `sky` | `getDuelIslandRaw()` |

**Aside order (fixed):** Main quest → Auction card → Side → Legendary → DRI → Duel Island.

Sections use `JournalQuestsSection` (closed by default), `display-mode="side"`
cards and `calculateNewStatus: true` (locked/done/new variants).

## Section invariant

A section **only exists when it has at least one quest**. No empty-state UI.

## Accent color rule

- Must be a standard Tailwind palette name.
- Must differ from every accent in use: yellow, `emerald`, `teal`, `cyan`, `sky`.
- Add to `journal-section-palette.ts` by copying the `teal` entry (only the color name changes) and extend `JournalSectionAccentColor`.

## Naming and i18n

- Quest ids **camelCase** in JSON, matching backend. Files kebab-case (`dri-agent-patamon.json`).
- `i18n/locales/{en-US,pt-BR}/quest/{category}/{id-kebab}.json`, registered in each locale `index.ts`. Section titles in `journal.json`.
- Quest texts use i18n; **Digimon/digievolution names stay literal** (proper names).

---

## Workflow B — Quest in an existing category

### 1. Static quest data

- [ ] `database/quest/{category}/{id-kebab}.json` — mirror a quest in the same folder
- [ ] `repositories/tables/quest/{category}/{id-kebab}.table.ts` → `export type {Name}Table = QuestRaw`
- [ ] `i18n/locales/en-US/quest/{category}/{id-kebab}.json`
- [ ] `i18n/locales/pt-BR/quest/{category}/{id-kebab}.json`
- [ ] Import + spread in `i18n/locales/{en-US,pt-BR}/index.ts`

### 2. Repository

- [ ] `repositories/quest.repository.ts` — import JSON + table type; append to the category getter in the **desired UI position** (default: end)

**SKIP:** DTO, model, events converter, syncer, `Journal.vue`, palette,
`journal.viewmodel.ts`, `journal.presenter.ts`, `quest-modal.presenter.ts` —
all iterate the full repository array per category.

### 3. Quest JSON details

- `requisites` at quest root and/or per step, ids matching backend (e.g. `sunTrophy` → `asukaTrophy`; DRI step 3 → `{rookie}DDNA`).
- Locations: `location` = innermost target map id; `coordinates` = pin on that map.
- Omit `innerLocation` to inherit the path from `location.json`; `"innerLocation": []` skips the canonical path; custom routes (desert cells, unique doors) keep hop `innerLocation` without the target pin.
- Coordinates may be provisional `x`/`y` `50` until the user supplies markers — note it in the status file.
- i18n: `title`, `description`, `steps.{n}` texts, one `locationTarget` per step (marker label), `requisites.{id}` labels.

Example (DRI agent, all 8 already done): title `"{Rookie} - DRI {Npc} ({levelRange})"`; steps look for agent → defeat boss for DDNA → deliver DNA.

### 4. Manual verification

- [ ] Card appears after `InitialState` (section collapsed by default)
- [ ] Memory change updates the quest via `JournalChanged`
- [ ] Modal opens for the new id

### 5. Retrofeed

Append the quest id under its category in [frontend-status.md](frontend-status.md).

---

## Debugging

| Symptom | Likely cause |
|---------|--------------|
| Card missing | Quest id not in `InitialState` (backend) or not in repository getter; ids differ in casing |
| Card shows, modal empty | `QuestModalPresenter` id mismatch |
| Live updates ignored | Delta id not in store; `syncJournal` no-op (`journal` null) |
| Wrong step state | Step `number` mismatch between JSON and backend |
| Missing translations | Locale `index.ts` import omitted |

## Additional resources

- New category: [workflow-new-category.md](workflow-new-category.md)
- Pipeline and file map: [frontend-pipeline.md](frontend-pipeline.md)
- Status: [frontend-status.md](frontend-status.md)
- Rules: `.cursor/rules/digivice-business.mdc` (Journal), `.cursor/rules/digivice-frontend-*.mdc`
