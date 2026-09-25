# Quest Pattern — Backend Pipeline

Canonical memory layers: `.cursor/rules/digivice-backend.mdc`. This file is the
journal-specific data flow.

## Data flow

```
Quests/**/*Addresses.json
  → AddressesRepository (GetMainQuest / GetAll{Category})
  → QuestLoader → QuestReader → StepReader / RequisiteReader → MemoryReader
  → QuestResource → JournalResource (JournalLoader)
  → JournalProvider → JournalAssembler
      → MainQuestAssembler | QuestAssembler | DuelIslandAssembler
  → Journal (State, via StateComposer)
  → JournalDiffer (GenerateQuestsDtos) → QuestDiffer → StepDiffer / RequisiteDiffer
  → JournalDTO
  → JournalEventFactory → Event(JournalChanged)
  → StateEventFactory
```

## Key paths

| Concern | Path |
|---------|------|
| Quest definitions | `Backend/Memory/Definitions/Quests/` (`MainQuestAddresses.json`, `SideQuests/`, `LegendaryWeapons/`, `DriAgents/`, `DuelIsland/`) |
| Address types | `Backend/Memory/Addresses/Journals/` (`QuestAddresses`, `Quests/StepAddresses`, `Quests/RequisiteAddresses`) |
| Readers | `Backend/Memory/Readers/` (`QuestReader`, `StepReader`, `RequisiteReader` — flat folder) |
| Resources | `Backend/Memory/Resources/Journals/`, `Backend/Memory/Resources/JournalResource.cs` |
| Repository | `Backend/Memory/Repositories/AddressesRepository.cs` |
| Loaders | `Backend/Application/Loaders/QuestLoader.cs`, `JournalLoader.cs` (flat folder) |
| Provider | `Backend/Application/Providers/JournalProvider.cs` |
| Models | `Backend/Domain/Models/Journal.cs`, `Journals/Quest.cs`, `Journals/Quests/` |
| Assemblers | `Backend/Domain/Assemblers/JournalAssembler.cs`, `Journals/` |
| Events | `Backend/Events/Diffing/JournalDiffer.cs` + `Journals/`, `Converters/`, `DTO/`, `Factory/JournalEventFactory.cs` |
| DI | `Backend/Infrastructure/DependencyInjection.cs` |
| Tests | `Tests/Integration/Application/Loaders/QuestLoaderTests.cs`, `Tests/Unit/Application/Loaders/JournalLoaderTests.cs`, `Tests/Integration/Memory/Repositories/AddressesRepositoryDefinitionsTests.cs`, `Tests/Unit/Domain/Assemblers/Journals/`, `Tests/Unit/Events/Diffing/JournalDifferTests.cs` |

## Category template (Duel Island, latest added)

| Layer | Reference |
|-------|-----------|
| Repository | `GetAllDuelIsland()` |
| Loader | `QuestLoader.LoadDuelIsland()` |
| Resource / model | `JournalResource.DuelIsland`, `Journal.DuelIsland` |
| Assembler | `DuelIslandAssembler.Assemble` in `JournalAssembler` |
| Differ | `GenerateQuestsDtos(newJournal.DuelIsland, previousJournal.DuelIsland)` |
| Tests | `QuestLoaderTests.LoadDuelIsland_*`, `DuelIslandAssemblerTests` |
