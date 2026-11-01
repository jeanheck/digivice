# Journal Quest — Pipeline

## Data flow

```
data/memory-addresses/quests/**/*.json
  → AddressesRepository (getMainQuest / getAll{Category}, import.meta.glob ordered by file name)
  → JournalProvider → JournalReader → QuestReader (snapshot) → JournalResource
  → JournalAssembler → QuestAssembler | MainQuestAssembler | DuelIslandAssembler
  → State.journal (StateComposer, every tick)
  → useGameStore.applyState → StatePatcher (lists keyed by id / number)

useGameState().journal
  → JournalPresenter
  → QuestRepository (static JSON Raw)
  → QuestModalPresenter + presentation/presenters/converter/QuestConverter
  → JournalViewModel
  → Journal.vue → JournalQuestsSection + JournalQuestCard
  → QuestModal (on card click)
```

## Key paths

Paths under `App/src/`.

| Concern | Path |
|---------|------|
| Definitions | `data/memory-addresses/quests/` |
| Addresses types | `memory/addresses/journal.addresses.ts`, `memory/addresses/journals/` |
| Addresses converter | `memory/converters/journals/quest-addresses.converter.ts` |
| Addresses repository | `memory/repositories/addresses.repository.ts` |
| Readers | `memory/readers/journal.reader.ts`, `memory/readers/quest.reader.ts` |
| Resources | `memory/resources/journal.resource.ts`, `memory/resources/journals/` |
| Assemblers | `memory/assemblers/journal.assembler.ts`, `quest.assembler.ts`, `main-quest.assembler.ts`, `duel-island.assembler.ts` |
| Provider | `memory/providers/journal.provider.ts` |
| Patcher list keys | `memory/patch/state.patcher.ts` (`ListKeys`) |
| Models | `state/models/journal/` |
| Static JSON | `data/game/quest/` |
| Static repository | `presentation/repositories/quest.repository.ts` |
| Table types | `presentation/repositories/tables/quest/` |
| Raw types | `presentation/repositories/tables/raws/quest/` |
| Presenters | `presentation/presenters/journal/` |
| Presenter converters | `presentation/presenters/converter/` |
| ViewModels | `presentation/viewmodels/quest/` |
| Journal UI | `ui/components/journal/` |
| Section palette | `ui/components/journal/journal-section-palette.ts` |
| i18n journal | `ui/i18n/{en-US,pt-BR,es-ES}/journal.json` |
| i18n quests | `ui/i18n/{en-US,pt-BR,es-ES}/quest/` |

## Two converter contexts (do not mix)

| Context | Folder | Direction |
|---------|--------|-----------|
| Definitions | `memory/converters/` | definitions `Raw` (camelCase JSON, hex strings) → `*Addresses` |
| Static/UI | `presentation/presenters/converter/` | static `Raw` + `Model` → `ViewModel` |

## Side quests reference (copy pattern)

| Layer | Reference |
|-------|-----------|
| Definitions | `data/memory-addresses/quests/side-quests/` |
| Repository getter | `AddressesRepository.getAllSideQuests()` |
| Reader / assembler | `sideQuests` in `journal.reader.ts` / `journal.assembler.ts` |
| Model field | `Journal.sideQuests` |
| Patcher key | `sideQuests: "id"` |
| Static getter | `getSideQuestsRaw()` |
| Presenter | `journal.presenter.ts` side-quest map |
| Modal resolve | `quest-modal.presenter.ts` side-quest branch |
| UI section | `JournalQuestsSection` + `accent-color="emerald"` |

## Aside section order

1. Main quest (inline yellow section)
2. Auction
3. Side quests
4. Legendary weapons
5. DRI agents
6. Duel Island

## Section invariant

A collapsible section is wired only when the category has **at least one quest**
(definitions + static JSON). Empty sections are not supported.
