# Quest Pattern — Frontend Pipeline

## Data flow

```
Backend SignalR
  → signalr.handlers.ts (JournalChanged / InitialState)
  → useGameStore.setInitialState | syncJournal
  → JournalConverter (events/converters/)     [full bootstrap]
  → JournalSyncer → QuestSyncer chain         [incremental patch]
  → currentState.journal (Model)

currentState.journal
  → JournalPresenter
  → QuestRepository (static JSON Raw)
  → QuestModalPresenter + presenters/converter/QuestConverter
  → JournalViewModel
  → Journal.vue → JournalQuestsSection + JournalQuestCard
  → QuestModal (on card click)
```

## Key paths

| Concern | Path |
|---------|------|
| SignalR bridge | `Frontend/src/events/signalr.handlers.ts` |
| Event types | `Frontend/src/events/events.map.ts` |
| Journal DTO | `Frontend/src/events/dto/journal.dto.ts` |
| Quest DTOs | `Frontend/src/events/dto/journals/` |
| Events converters | `Frontend/src/events/converters/journal.converter.ts`, `journals/` |
| Models | `Frontend/src/models/journal/` |
| Store | `Frontend/src/stores/use-game-store.ts` |
| Syncers | `Frontend/src/stores/syncers/journal.syncer.ts`, `journals/` |
| Static JSON | `Frontend/src/database/quest/` |
| Repository | `Frontend/src/repositories/quest.repository.ts` |
| Raw types | `Frontend/src/repositories/tables/raws/quest/` |
| Presenters | `Frontend/src/presenters/journal/` |
| Presenter converters | `Frontend/src/presenters/converter/` |
| ViewModels | `Frontend/src/viewmodels/quest/` |
| Journal UI | `Frontend/src/components/journal/` |
| Section palette | `Frontend/src/components/journal/journal-section-palette.ts` |
| i18n journal | `Frontend/src/i18n/locales/{en-US,pt-BR}/journal.json` |
| i18n quests | `Frontend/src/i18n/locales/{en-US,pt-BR}/quest/` |

## Two converter contexts (do not mix)

| Context | Folder | Direction |
|---------|--------|-----------|
| Realtime | `events/converters/` | `DTO` → `Model` |
| Static/UI | `presenters/converter/` | `Raw` + `Model` → `ViewModel` |

## Side quests reference (copy pattern)

| Layer | Reference |
|-------|-----------|
| DTO field | `JournalDTO.sideQuests` |
| Model field | `Journal.sideQuests` |
| Events converter | `sideQuests` map in `journal.converter.ts` |
| Syncer | `sideQuests` loop in `journal.syncer.ts` |
| Repository | `getSideQuestsRaw()` |
| Presenter | `journal.presenter.ts` side-quest map |
| Modal resolve | `quest-modal.presenter.ts` side-quest branch |
| UI section | `JournalQuestsSection` + `accent-color="cyan"` |

## Planned category accents

| Category | Accent |
|----------|--------|
| Side quests | `cyan` (live) |
| Legendary weapons | `purple` |
| DRI agents | `rose` |

## Aside section order

1. Main quest (inline yellow section)
2. Side quests
3. Legendary weapons
4. DRI agents

## Section invariant

A collapsible section is wired only when the category has **at least one quest**
(static JSON + backend `InitialState`). Empty sections are not supported.
