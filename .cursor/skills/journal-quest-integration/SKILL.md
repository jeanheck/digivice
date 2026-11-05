---
name: journal-quest-integration
description: >-
  Integrates journal quests end to end in Digivice — quest definitions JSON in
  the TypeScript memory layer, static quest JSON, repository, i18n, presenters
  and Journal UI. Use when the user asks to add a quest to an existing journal
  category (side quest, legendary weapon, DRI agent, Duel Island), to connect
  confirmed quest addresses after memory-compare, or to create a brand-new
  journal category.
---

# Journal Quest Integration

Wires a quest from its RAM definitions (`App/src/data/memory-addresses/quests/`)
to the Journal UI. Product rules of each category: `.cursor/rules/digivice-business.mdc`
(Journal).

**Prerequisite:** quest addresses confirmed (skill `memory-compare`).

## When NOT to use

| Case | Use instead |
|------|-------------|
| Field on Player / Party / Digimon / battle / auctions / NPCs | `memory-field-integration` |
| New non-journal entity on `State` | `memory-entity-integration` |

## Reuse rule (critical)

Quest entities are shared across all categories. **Do not** create per-quest
types, readers, assemblers, presenters or card components.

| Layer | Reuse as-is |
|-------|-------------|
| Addresses | `QuestAddresses`, `StepAddresses`, `RequisiteAddresses` + `QuestAddressesConverter` |
| Read | `QuestReader`, `QuestResource` |
| Assemble | `QuestAssembler` (pass-through), `MainQuestAssembler`, `DuelIslandAssembler` |
| Models | `Quest`, `Step`, `Requisite` |
| Presenter converter | `QuestConverter` (`presentation/presenters/converter/quest.converter.ts`) |
| UI | `JournalQuestCard`, `JournalQuestsSection`, `QuestModal` tree |

## Pick the workflow

```
Quest id in a category already on Journal?  → Workflow A (below)
New property on Journal?
  → At least one quest id ready?
      NO  → Stop; ask for the first quest (no empty sections)
      YES → Workflow B: workflow-new-category.md
  → Accent color given by the user?
      NO  → Ask (Tailwind palette name, not already used)
Product asked for a progression fix-up?  → Normalization (below)
Same byte as an existing tracker?  → Likely another bit; still a separate quest id
```

## Category map (all wired)

| Category | Definitions (`data/memory-addresses/quests/`) | `Journal` property | Assembler | Repository getter | Static JSON (`data/game/quest/`) | Static getter | Accent |
|----------|--------------------------------------------|--------------------|-----------|-------------------|---------------------------------|---------------|--------|
| Main quest | `main-quest-addresses.json` | `mainQuest` (single) | `MainQuestAssembler` (cascade) | `getMainQuest()` | `main-quest.json` | `getMainQuestRaw()` | yellow (hardcoded) |
| Side quests | `side-quests/` | `sideQuests` | `QuestAssembler` | `getAllSideQuests()` | `side-quest/` | `getSideQuestsRaw()` | `emerald` |
| Legendary weapons | `legendary-weapons/` | `legendaryWeapons` | `QuestAssembler` | `getAllLegendaryWeapons()` | `legendary-weapons/` | `getLegendaryWeaponsRaw()` | `teal` |
| DRI agents | `dri-agents/` | `driAgents` | `QuestAssembler` | `getAllDriAgents()` | `dri-agents/` | `getDriAgentsRaw()` | `cyan` |
| Duel Island | `duel-island/` | `duelIsland` | `DuelIslandAssembler` (normalization) | `getAllDuelIsland()` | `duel-island/` | `getDuelIslandRaw()` | `sky` |

Repository getters live in `memory/repositories/addresses.repository.ts`; static
getters in `presentation/repositories/quest.repository.ts`. Current quests: [status.md](status.md).
File map: [pipeline.md](pipeline.md).

**Aside order (fixed):** Main quest → Auction card → Side → Legendary → DRI → Duel Island.

Sections use `JournalQuestsSection` (closed by default), `display-mode="side"`
cards and `calculateNewStatus: true` (locked/done/new variants).

## Definitions schema

```json
{
    "id": "questId",
    "requisites": [
        { "id": "otherQuestOrItemId", "address": "0x00048DC2" }
    ],
    "steps": [
        {
            "number": 1,
            "address": "0x0004B38C",
            "bitMasks": ["0x04"],
            "requisites": [
                { "id": "itemId", "address": "0x00048DD7", "bitMasks": ["0x01"] }
            ]
        }
    ]
}
```

- `requisites` exist at **two levels**: quest root (gate for the whole quest, e.g. `asuka-rematch-addresses.json` → `asukaTrophy`; `dri-agent-patamon-addresses.json` → `submarimon`) and per step (e.g. DRI step 3 → `{rookie}DDNA`). Both are optional.
- `bitMasks` (array) on steps **and** requisites. Empty = raw byte (`!= 0`). Multiple = **all** must be set. Singular `bitMask` → normalize to `"bitMasks": ["0x04"]`.
- Single mask on one line (`"bitMasks": ["0x04"]`); multiline only for many entries.
- **Ids in camelCase** (`muramasa`, `driAgentGuilmon`, `guilmonDDNA`). The same id is used by the definitions, the static quest JSON and i18n.
- Step `number` sequential and unique within the quest; equal to the step `number` in the static JSON.
- `App/src/data/memory-addresses/` is the single source of the addresses. `Tools/MemoryScanner` (`analyze-pair`) reads `quests/main-quest-addresses.json` from there by a fixed path: do not move or rename it.

## Section invariant

A section **only exists when it has at least one quest**. No empty-state UI.

## Accent color rule

- Must be a standard Tailwind palette name.
- Must differ from every accent in use: yellow, `emerald`, `teal`, `cyan`, `sky`.
- Add to `journal-section-palette.ts` by copying the `teal` entry (only the color name changes) and extend `JournalSectionAccentColor`.

## Naming and i18n

- Static files kebab-case (`dri-agent-patamon.json`); definitions kebab-case with the `-addresses` suffix (`dri-agent-patamon-addresses.json`).
- `ui/i18n/{en-US,pt-BR,es-ES}/quest/{category}/{id-kebab}.json`, registered in each locale `index.ts`. Section titles in `journal.json`.
- Quest texts use i18n; **Digimon/digievolution names stay literal** (proper names).

---

## Workflow A — Quest in an existing category

Paths under `App/src/` unless stated otherwise.

### 1. Definitions

- [ ] `data/memory-addresses/quests/{category-kebab}/{name-kebab}-addresses.json` — mirror a quest of the same folder; normalize the schema
- [ ] **SKIP** repository, reader, assembler, provider, model, patcher — `import.meta.glob` picks up every `*.json` of the folder (ordered by file name) and `StatePatcher` already keys the category list by `id`

### 2. Memory tests

Conventions: skill `frontend-memory-tests`.

- [ ] `tests/memory/repositories/addresses.repository.spec.ts` — bump the category count in "should load every quest folder"; update "should order the quests of each folder by file name"
- [ ] `tests/fixtures/snapshots/field-central-park.expected.ts` — `questIn({Category}, "{id}", { doneSteps, doneRequisites })` in file-name order, with the progress present in the `.bin` (confirm with the user against the game). `battle-against-natsumi.expected.ts` reuses this journal; a new fixture with a different journal needs its own entries
- [ ] `npx vitest run` — only when the user asks

### 3. Static quest data

- [ ] `data/game/quest/{category}/{id-kebab}.json` — mirror a quest in the same folder
- [ ] `presentation/repositories/tables/quest/{category}/{id-kebab}.table.ts` → `export type {Name}Table = QuestRaw`
- [ ] `ui/i18n/en-US/quest/{category}/{id-kebab}.json`
- [ ] `ui/i18n/pt-BR/quest/{category}/{id-kebab}.json`
- [ ] `ui/i18n/es-ES/quest/{category}/{id-kebab}.json`
- [ ] Import + spread in `ui/i18n/{en-US,pt-BR,es-ES}/index.ts`

### 4. Repository

- [ ] `presentation/repositories/quest.repository.ts` — import JSON + table type; append to the category getter in the **desired UI position** (default: end)

**SKIP:** `Journal.vue`, palette, `journal.viewmodel.ts`, `journal.presenter.ts`,
`quest-modal.presenter.ts` — all iterate the full repository array per category.

### 5. Quest JSON details

- `requisites` at quest root and/or per step, ids matching the definitions (e.g. `asukaRematch` → `asukaTrophy`; DRI step 3 → `{rookie}DDNA`).
- Locations: `location` = innermost target map id; `coordinates` = pin on that map.
- The path to a map belongs to the map: omit `innerLocation` in the step to inherit it from `map.json`. A path valid for any quest goes into the map's `innerLocation` in `map.json`, not into the step.
- Step `innerLocation` only when the route depends on the quest (desert cells, a hop that differs per quest — e.g. main quest step 4 into Seiryu City `022E` through Seiryu Tower): hops only, without the target pin. Never use `"innerLocation": []`.
- Coordinates may be provisional `x`/`y` `50` until the user supplies markers — note it in [status.md](status.md).
- i18n: `title`, `description`, `steps.{n}` texts, one `locationTarget` per step (marker label), `requisites.{id}` labels.

Example (DRI agent, all 8 already done): title `"{Rookie} - DRI {Npc} ({levelRange})"`; steps look for agent → defeat boss for DDNA → deliver DNA.

### 6. Manual verification

- [ ] Card appears on the first tick (section collapsed by default)
- [ ] Memory change in the emulator updates the card on the next tick
- [ ] Modal opens for the new id

### 7. Retrofeed

- [ ] Append the quest under its category in [status.md](status.md)
- [ ] Mark the addresses as integrated in `.cursor/skills/memory-compare/memory-regions.md` / `patterns/*.md` when they came from an investigation

---

## Normalization (conditional)

Default is **pass-through** (`QuestAssembler`). Only add a normalization when
the product explicitly asks:

- Main quest cascade lives in `MainQuestAssembler` (later step done → earlier steps done). **Main quest only.**
- Duel Island normalization lives in `DuelIslandAssembler` (requisite not met → all steps false; trophy done → earlier steps true). **Duel Island only.**
- New normalization → new `memory/assemblers/{category-kebab}.assembler.ts` wrapping `QuestAssembler.assemble`, used in `JournalAssembler`, plus its spec. Never reuse another category's normalization silently.

## Debugging

| Symptom | Likely cause |
|---------|--------------|
| Card missing | Definitions JSON outside the category folder, or quest not in the static getter; ids differ in casing |
| Card shows, modal empty | `QuestModalPresenter` id mismatch |
| Wrong step state | Step `number` in the definitions differs from `number` in the static JSON; wrong `bitMasks` |
| Quest order wrong in tests | Definitions are ordered by file name, not by id |
| Missing translations | Locale `index.ts` import omitted |

## Additional resources

- New category: [workflow-new-category.md](workflow-new-category.md)
- Pipeline and file map: [pipeline.md](pipeline.md)
- Status: [status.md](status.md)
- Investigation: `.cursor/skills/memory-compare/`
- Rules: `.cursor/rules/digivice-business.mdc` (Journal), `.cursor/rules/digivice-frontend.mdc`
