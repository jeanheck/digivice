# Quest Pattern — Backend Pipeline

## Data flow

```
*Addresses.json
  → AddressesRepository
  → QuestReader → StepReader → MemoryReader
  → QuestResource / JournalResource
  → JournalLoader / QuestLoader
  → JournalAssembler → QuestAssembler
  → Journal (State)
  → JournalDiffer → QuestDiffer → StepDiffer
  → JournalDTO
  → JournalEventFactory → Event(JournalChanged)
  → StateEventFactory
```

## Key paths

| Concern | Path |
|---------|------|
| Quest definitions | `Backend/Memory/Definitions/Quests/` (`SideQuests/`, `LegendaryWeapons/`, `DriAgents/`, `MainQuestAddresses.json`) |
| Other definitions | `Backend/Memory/Definitions/` (Player, Party, Auctions, …) |
| Address types | `Backend/Memory/Addresses/Journals/` |
| Readers | `Backend/Memory/Readers/Journals/` |
| Resources | `Backend/Memory/Resources/Journals/` |
| Repository | `Backend/Memory/Repositories/AddressesRepository.cs` |
| Loaders | `Backend/Application/Loaders/` |
| Models | `Backend/Domain/Models/Journal.cs`, `Journals/Quest.cs` |
| Assemblers | `Backend/Domain/Assemblers/JournalAssembler.cs` |
| Events | `Backend/Events/Diffing/`, `Converters/`, `DTO/`, `Factory/` |
| DI | `Backend/Infrastructure/DependencyInjection.cs` |
| Tests | `Tests/Integration/Application/Loaders/`, `Tests/Unit/Events/Diffing/` |

## Side quest reference (copy pattern)

| Layer | Reference |
|-------|-----------|
| Repository | `GetAllSideQuests()` in `AddressesRepository.cs` |
| Loader | `QuestLoader.LoadSideQuests()` |
| Journal model | `Journal.SideQuests` |
| Differ | loop in `JournalDiffer.cs` (side quests section) |
| Integration test | `QuestLoaderTests.LoadSideQuests_ShouldIntegrateSideQuestAddressesAndReaderPipeline` |

## First item in new category

When `LegendaryWeapons` or `DriAgents` is not yet on `Journal`, mirror the
`SideQuests` slot across all layers listed in SKILL.md step 3 before registering
individual `*Addresses.json` files under `Quests/LegendaryWeapons/` or
`Quests/DriAgents/`.
