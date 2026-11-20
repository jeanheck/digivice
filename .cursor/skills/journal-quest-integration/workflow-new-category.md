# Workflow B — New journal category

Rare. Use only when `Journal` gains a new property. Template: `duelIsland`
(latest category added). Every step mirrors the existing `duelIsland` branch.
Paths under `App/src/` unless stated otherwise.

## 0. Preconditions

- [ ] **At least one quest id** ready (section invariant — no empty sections)
- [ ] Property name agreed: `{category}` (camelCase, `Journal` property) `{Category}` (PascalCase, TS names) and `{category-kebab}` (definitions folder)
- [ ] Accent color confirmed (see the accent color rule in [SKILL.md](SKILL.md))
- [ ] Normalization: pass-through unless the product asked otherwise

## 1. Definitions + repository

- [ ] Folder `data/memory-addresses/quests/{category-kebab}/` with at least one `{name-kebab}-addresses.json`
- [ ] `memory/repositories/addresses.repository.ts`:
  - `const {Category}Files = import.meta.glob<QuestAddressesRaw>("@/data/memory-addresses/quests/{category-kebab}/*.json", { eager: true, import: "default" })`
  - `private static {category}Addresses: QuestAddresses[] | null = null`
  - `getAll{Category}()` through `this.loadQuestFolder("{category-kebab}", {Category}Files)`

## 2. Memory pipeline

| File | Change |
|------|--------|
| `memory/addresses/journal.addresses.ts` | `{category}: QuestAddresses[]` |
| `memory/providers/journal.provider.ts` | `{category}: AddressesRepository.getAll{Category}()` in `getAddresses` |
| `memory/resources/journal.resource.ts` | `{category}: QuestResource[]` |
| `memory/readers/journal.reader.ts` | `{category}: JournalReader.readQuests(snapshot, addresses.{category})` |
| `state/models/journal/journal.ts` | `{category}: Quest[]` |
| `memory/assemblers/journal.assembler.ts` | `resource.{category}.map((quest) => QuestAssembler.assemble(quest))` (or the category assembler) |
| `memory/patch/state.patcher.ts` | `{category}: "id"` in `ListKeys` |

`StateComposer`, `GameLoop`, `useGameStore`: **SKIP**.

## 3. Memory tests

Conventions: skill `frontend-memory-tests`.

- [ ] `tests/memory/repositories/addresses.repository.spec.ts` — new folder in "should load every quest folder" and in the file-name ordering test
- [ ] `tests/memory/readers/journal.reader.spec.ts`, `tests/memory/assemblers/journal.assembler.spec.ts` — the new collection
- [ ] `tests/fixtures/snapshots/field-central-park.expected.ts` — `const {Category} = AddressesRepository.getAll{Category}()` and `{category}: [questIn(...)]` in the journal
- [ ] `tests/memory/patch/state.patcher.spec.ts` — only if the list needs a case beyond the existing `id` lists
- [ ] Category assembler spec when there is a normalization
- [ ] `npx vitest run` — only when the user asks

## 4. Static quest data (per quest)

- [ ] `data/game/quest/{category-kebab}/{id-kebab}.json`
- [ ] `presentation/repositories/tables/quest/{category-kebab}/{id-kebab}.table.ts` → `export type XxxTable = QuestRaw`
- [ ] `ui/i18n/{en-US,pt-BR,es-ES}/quest/{category-kebab}/{id-kebab}.json`
- [ ] Register imports in every `ui/i18n/{en-US,pt-BR,es-ES}/index.ts`

## 5. Repository

`presentation/repositories/quest.repository.ts`: import JSON + table types; add
`get{Category}Raw(): QuestRaw[]`. Array order = **UI display order**.

## 6. ViewModel + presenters

| File | Change |
|------|--------|
| `presentation/viewmodels/quest/journal.viewmodel.ts` | new collection field |
| `presentation/presenters/journal/journal.presenter.ts` | map `get{Category}Raw()` → `QuestModalPresenter.getQuestViewModel` + filter nulls |
| `presentation/presenters/journal/quest-modal.presenter.ts` | new `find(raw => raw.id === questId)` branch with `calculateNewStatus: true` |

`presentation/presenters/converter/quest.converter.ts`: **SKIP**.

## 7. UI

- [ ] `ui/components/journal/Journal.vue` — `JournalQuestsSection` in the fixed order, `accent-color="{color}"`, `v-for` over `journalViewModel.{category}`, `display-mode="side"` on each `JournalQuestCard`; `defaultExpanded` omitted (closed)
- [ ] `ui/components/journal/journal-section-palette.ts` — extend `JournalSectionAccentColor` and copy the `teal` entry changing only the color name
- [ ] `ui/i18n/{en-US,pt-BR,es-ES}/journal.json` — section title key `journal.{category}`

## 8. Manual verification

- [ ] First tick shows the cards in the new section when expanded
- [ ] Step/requisite change in the emulator updates card + modal on the next tick
- [ ] Modal opens for quests in the new section
- [ ] Accent distinct from all other sections

## 9. Docs

- [ ] [status.md](status.md): category, assembler, accent, quest ids
- [ ] Category map in [SKILL.md](SKILL.md) and [pipeline.md](pipeline.md)
- [ ] Journal categories in `.cursor/rules/digivice-business.mdc`
