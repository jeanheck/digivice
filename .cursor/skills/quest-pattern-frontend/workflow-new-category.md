# Workflow A — New journal category (frontend)

Rare. Use only when `Journal` gains a new property. Template: `duelIsland`
(latest category added). All steps mirror the existing `duelIsland` branch.

## 0. Preconditions

- [ ] **At least one quest id** ready (section invariant — no empty sections)
- [ ] User confirmed backend emits the collection in `InitialState` / `JournalChanged`
- [ ] Property name agreed (camelCase, matches backend JSON)
- [ ] Accent color confirmed (see accent color rule in [SKILL.md](SKILL.md))

## 1. SignalR handlers — SKIP

`JournalChanged` → `store.syncJournal` and `InitialState` → `store.setInitialState`
are already wired in `events/signalr.handlers.ts`.

## 2. Sync pipeline

| File | Change |
|------|--------|
| `events/dto/journal.dto.ts` | `{category}?: QuestDTO[]` |
| `models/journal/journal.ts` | `{category}: Quest[]` |
| `events/converters/journal.converter.ts` | map with `QuestConverter.convert` |
| `stores/syncers/journal.syncer.ts` | loop: find previous quest by `id`, `QuestSyncer.sync` |

`stores/use-game-store.ts`: **SKIP** (already delegates to `JournalConverter` / `JournalSyncer`).

Syncers only **patch** quests present from `InitialState`; they never add quests.

## 3. Static quest data (per quest)

- [ ] `database/quest/{category}/{id-kebab}.json`
- [ ] `repositories/tables/quest/{category}/{id-kebab}.table.ts` → `export type XxxTable = QuestRaw`
- [ ] `i18n/locales/{en-US,pt-BR}/quest/{category}/{id-kebab}.json`
- [ ] Register imports in both `i18n/locales/{en-US,pt-BR}/index.ts`

## 4. Repository

`repositories/quest.repository.ts`: import JSON + table types; add
`get{Category}Raw(): QuestRaw[]`. Array order = **UI display order**.

## 5. ViewModel + presenters

| File | Change |
|------|--------|
| `viewmodels/quest/journal.viewmodel.ts` | new collection field |
| `presenters/journal/journal.presenter.ts` | map `get{Category}Raw()` → `QuestModalPresenter.getQuestViewModel` + filter nulls |
| `presenters/journal/quest-modal.presenter.ts` | new `find(raw => raw.id === questId)` branch with `calculateNewStatus: true` |

`presenters/converter/quest.converter.ts`: **SKIP**.

## 6. UI

- [ ] `components/journal/Journal.vue` — `JournalQuestsSection` in the fixed order, `accent-color="{color}"`, `v-for` over `journalViewModel.{category}`, `display-mode="side"` on each `JournalQuestCard`; `defaultExpanded` omitted (closed)
- [ ] `components/journal/journal-section-palette.ts` — extend `JournalSectionAccentColor` and copy the `teal` entry changing only the color name
- [ ] `i18n/locales/{en-US,pt-BR}/journal.json` — section title key `journal.{category}`

## 7. Manual verification

- [ ] `InitialState` shows cards in the new section when expanded
- [ ] Step/requisite change in emulator updates card + modal
- [ ] Modal opens for quests in the new section
- [ ] Accent distinct from all other sections

## 8. Retrofeed

[frontend-status.md](frontend-status.md): category, accent, quest ids.
