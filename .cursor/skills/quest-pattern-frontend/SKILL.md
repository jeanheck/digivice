---
name: quest-pattern-frontend
description: >-
  Integrates journal quest sections into the Digivice frontend — SignalR
  reception through Pinia syncers, static JSON, presenters, and Journal UI.
  Use when the user explicitly asks to wire a new journal category (legendary
  weapons, DRI agents) or add a quest to an existing category on the frontend.
disable-model-invocation: true
---

# Quest Pattern — Frontend Integration

Wire journal quest data from **SignalR reception** through **Journal UI**.
Stops at rendered section + working modal. Does not touch backend C#.

**User responsibility:** backend already emits the new field/quest in
`InitialState` and `JournalChanged`. Do **not** invoke or verify
`quest-pattern-backend` — assume the user confirmed that separately.

**No frontend tests** (project rule).

## Reuse rule (critical)

Quest entities are shared across all journal categories. **Do not** create
per-quest DTOs, models, syncers, or card components.

| Layer | Reuse as-is |
|-------|-------------|
| DTOs | `QuestDTO`, `StepDTO`, `RequisiteDTO` |
| Models | `Quest`, `Step`, `Requisite` |
| Events converters | `QuestConverter`, `StepConverter`, `RequisiteConverter` (`events/converters/`) |
| Syncers | `QuestSyncer`, `StepSyncer`, `RequisiteSyncer` |
| Presenter converters | `QuestConverter` (`presenters/converter/`) |
| UI | `JournalQuestCard`, `JournalQuestsSection`, `QuestModal` tree |

Create new types/files only when adding a **new category slot** on `Journal`
(first Legendary Weapon section, first DRI Agent section, etc.) or when
registering **new static quest JSON** in an existing category.

## Two workflows

| Workflow | When | Scope |
|----------|------|-------|
| **A — New category slot** | First item in `legendaryWeapons`, `driAgents`, etc. | Full pipeline (DTO → UI) |
| **B — Quest in existing category** | New quest id under side quests, legendary weapons, etc. | Static data + repository + i18n |

Use the decision helper at the end to pick A or B.

## Category map

| Category | `Journal` property | Static JSON folder | Section accent | i18n section key |
|----------|-------------------|--------------------|----------------|------------------|
| Main quest | `mainQuest` (single) | `database/quest/main-quest.json` | Yellow (hardcoded in `Journal.vue`) | `journal.mainQuest` |
| Side quests | `sideQuests` | `database/quest/side-quest/` | `cyan` | `journal.sideQuests` |
| Legendary weapons | `legendaryWeapons` | `database/quest/legendary-weapons/` | `purple` | `journal.legendaryWeapons` |
| DRI agents | `driAgents` | `database/quest/dri-agents/` | `rose` | `journal.driAgents` |

**Aside section order (fixed):** Main → Side → Legendary → DRI.

All new collapsible sections use `JournalQuestsSection` with
`defaultExpanded: false` (starts closed, same as side quests today).

All new list sections use `display-mode="side"` and
`calculateNewStatus: true` (locked/done/new card variants).

## Section invariant (mandatory)

A journal section **only exists when it has at least one quest**. An empty
section is invalid — do not wire a category slot, `JournalQuestsSection`, or
repository getter for a section with zero quests.

| Rule | Detail |
|------|--------|
| Workflow A | Do not start until **at least one quest id** is ready (static JSON + backend in `InitialState`) |
| Workflow B | Category already wired; append quest to existing non-empty section |
| UI | No empty-state messaging — if the section renders, it always has cards |

If the user asks to add a section without a quest, **stop and ask** for the
first quest id before proceeding.

## Accent color rule (mandatory)

Before implementing, if the user **did not** specify the section accent color,
**ask which Tailwind color** to use.

| Status | Color |
|--------|-------|
| Occupied — not selectable | `yellow` (main quest, hardcoded) |
| Occupied — not selectable | `cyan` (side quests) |
| Reserved | `purple` (legendary weapons) |
| Reserved | `rose` (DRI agents; replaces unused `red` in palette) |

Rules:

1. Color must be a **standard Tailwind palette name** (e.g. `purple`, `rose`,
   `emerald`). If the user picks a name that is not a Tailwind color, ask them
   to choose another.
2. New accent must differ from **cyan** and **yellow**.
3. Add the color to `journal-section-palette.ts` by **copying the cyan/red
   token pattern** — only the color name changes (`text-{color}-400`,
   `border-{color}-800`, `hover:bg-{color}-900/30`, etc.).
4. Extend `JournalSectionAccentColor` with the new literal.

## Definitions layout (static JSON)

```
Frontend/src/database/quest/
├── main-quest.json
├── side-quest/
├── legendary-weapons/
└── dri-agents/
```

**Ids:** always **camelCase** in JSON (`muramasa`, `driAgentGuilmon`). Must
match backend quest id.

**i18n:** `i18n/locales/{en-US,pt-BR}/quest/{category}/{id}.json` — register
imports in each locale `index.ts`. Section titles live in `journal.json`.

Quest titles/descriptions/steps use i18n. **Digimon and digievolution proper
names stay literal** in JSON — never pass through `$t()` (see `AI/BUSINESS_RULES.md`).

---

## Workflow A — New category slot

Copy and track progress. Skip steps marked **SKIP** when the condition is met.

### 0. Preconditions

- [ ] **At least one quest id** ready to ship with the section (see section invariant)
- [ ] User confirmed backend emits new collection in `InitialState` / `JournalChanged`
- [ ] Property name agreed (`legendaryWeapons`, `driAgents`, …) — camelCase, matches backend
- [ ] Accent color confirmed (see accent color rule)

---

### 1. SignalR handlers *(usually SKIP)*

Files: `events/signalr.handlers.ts`, `events/signalr.service.ts`, `main.ts`

- [ ] `JournalChanged` → `store.syncJournal` already wired
- [ ] `InitialState` → `store.setInitialState` already wired

**SKIP** if handlers unchanged. Only verify `JournalDTO` will carry the new field.

---

### 2. DTO

File: `events/dto/journal.dto.ts`

- [ ] Add `legendaryWeapons?: QuestDTO[]` (or equivalent) — mirror `sideQuests`

---

### 3. Model

File: `models/journal/journal.ts`

- [ ] Add matching property (`Quest[]` for list categories)

---

### 4. Events converter

File: `events/converters/journal.converter.ts`

- [ ] Map new collection with `QuestConverter.convert` — copy `sideQuests` branch

---

### 5. Syncer

File: `stores/syncers/journal.syncer.ts`

- [ ] Add loop: find previous quest by `id`, call `QuestSyncer.sync` — copy side-quest loop

**Sync limitation:** syncers only **patch** quests already present from
`InitialState`. They do not add quests or bootstrap journal alone.

---

### 6. Pinia store *(usually SKIP)*

File: `stores/use-game-store.ts`

- [ ] `setInitialState` / `syncJournal` already delegate to `JournalConverter` / `JournalSyncer`

**SKIP** unless store actions need a new branch (rare).

---

### 7. Static quest JSON (per quest in category)

For each quest id:

- [ ] `database/quest/{category}/{id-kebab}.json` — structure mirrors side-quest JSON
- [ ] `repositories/tables/quest/{category}/{id-kebab}.table.ts` — `type XxxTable = QuestRaw`
- [ ] `i18n/locales/en-US/quest/{category}/{id-kebab}.json`
- [ ] `i18n/locales/pt-BR/quest/{category}/{id-kebab}.json`
- [ ] Register both locale imports in `i18n/locales/{en-US,pt-BR}/index.ts`

---

### 8. Repository

File: `repositories/quest.repository.ts`

- [ ] Import JSON + table types
- [ ] Add `getLegendaryWeaponsRaw(): QuestRaw[]` (or equivalent)
- [ ] Array order = **UI display order** (hardcoded, not backend order)

---

### 9. ViewModel + presenters

| File | Change |
|------|--------|
| `viewmodels/quest/journal.viewmodel.ts` | New collection field |
| `presenters/journal/journal.presenter.ts` | Map repository → `QuestModalPresenter.getQuestViewModel` |
| `presenters/journal/quest-modal.presenter.ts` | Resolve quest id from new section (after main/side checks) |

Presenter converter (`presenters/converter/quest.converter.ts`): **SKIP** — pass
`{ calculateNewStatus: true }` for list categories.

---

### 10. UI

File: `components/journal/Journal.vue`

- [ ] Add `JournalQuestsSection` block in fixed order (after side quests, before DRI if applicable)
- [ ] `accent-color` from palette; `defaultExpanded` omitted (defaults to `false`)
- [ ] `v-for` over `journalViewModel.legendaryWeapons` (or equivalent)
- [ ] `display-mode="side"` on each `JournalQuestCard`

File: `components/journal/journal-section-palette.ts`

- [ ] Add palette entry if accent not yet defined (copy cyan pattern)

File: `i18n/locales/{en-US,pt-BR}/journal.json`

- [ ] Section title key (e.g. `journal.legendaryWeapons`)

---

### 11. Manual verification

- [ ] `InitialState` shows cards in new section when expanded
- [ ] Step/requisite change in emulator updates card + modal
- [ ] Modal opens for quests in new section (`QuestModalPresenter` finds id)
- [ ] Accent color distinct from cyan (side) and yellow (main)

---

### 12. Retrofeed

Update [frontend-status.md](frontend-status.md): category wired, quest ids, accent color.

---

## Workflow B — Quest in existing category

Minimal checklist when the **category slot already exists** on `Journal`.

### 0. Preconditions

- [ ] Category already on frontend (`sideQuests`, `legendaryWeapons`, …)
- [ ] User confirmed backend includes new quest id in `InitialState`
- [ ] Quest id in camelCase, matches backend

---

### 1. Static quest JSON

- [ ] `database/quest/{category}/{id-kebab}.json`
- [ ] `repositories/tables/quest/{category}/{id-kebab}.table.ts`
- [ ] i18n `en-US` + `pt-BR` under `quest/{category}/`
- [ ] Register imports in both locale `index.ts` files

---

### 2. Repository

File: `repositories/quest.repository.ts`

- [ ] Import new JSON + table type
- [ ] Append to the category array in **desired UI position**

**SKIP** DTO, model, events converter, syncer, `Journal.vue` section shell,
`journal.viewmodel.ts`, palette — category already wired.

---

### 3. Presenters *(conditional)*

- [ ] `quest-modal.presenter.ts` — **SKIP** if it already resolves any id via
  `getXxxRaw().find(raw => raw.id === questId)` for that category
- [ ] `journal.presenter.ts` — **SKIP** if it already maps full repository array

---

### 4. Manual verification

- [ ] New card appears after `InitialState` (section collapsed by default)
- [ ] Memory change updates quest via `JournalChanged`
- [ ] Modal works for new quest id

---

### 5. Retrofeed

Append quest id under the category in [frontend-status.md](frontend-status.md).

---

## Decision helper

```
New quest id only (category already on Journal)?
  → Workflow B

New property on Journal (first item in category)?
  → At least one quest id ready?
      NO  → Stop; ask for first quest (do not wire empty section)
      YES → Workflow A
  → Accent color specified by user?
      NO  → Ask (Tailwind name; not cyan/yellow)
      YES → Validate Tailwind + palette entry
  → Backend sends collection in InitialState?
      (user confirms — do not run backend skill)
```

## Debugging (mid-integration)

| Symptom | Likely cause |
|---------|----------------|
| Section empty after connect | Misconfiguration: `InitialState` missing collection, repository array empty, or section wired without quests (invalid) |
| Card shows but modal empty | `QuestModalPresenter` missing branch or id mismatch |
| Live updates ignored | `JournalChanged` delta id not in store; or `syncJournal` no-op (`journal` null) |
| Wrong step state | Step `number` mismatch between JSON and backend |
| Missing translations | Locale `index.ts` import omitted |

## Additional resources

- Pipeline diagram and file map: [frontend-pipeline.md](frontend-pipeline.md)
- Integration status: [frontend-status.md](frontend-status.md)
- Business rules: `AI/BUSINESS_RULES.md` (Journal §2.3)
- Code rules: `AI/CODE_RULES.md` (Frontend sections)
