# Quest Pattern — Backend Pipeline

Canonical memory layers (Addresses → Loader → Reader → Resource → Assembler):
see `AI/CODE_RULES.md` (Backend). This file is the journal-specific data flow.

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

When adding a tracker in an existing category, drop `{Name}Addresses.json` in
the folder — `AddressesRepository` auto-discovers `*.json`. For a **new**
journal category, add `GetAll…()` + folder wiring (see SKILL.md step 2b) before
placing files under `Quests/{Category}/`.
