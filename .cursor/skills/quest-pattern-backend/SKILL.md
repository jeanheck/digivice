---
name: quest-pattern-backend
description: >-
  Integrates quest-pattern memory trackers into the Digivice backend — main
  quest, side quests, legendary weapons, DRI agents, Duel Island. Use when the
  user asks to integrate a Quests/** *Addresses.json into the backend, add a
  new quest to an existing journal category, create a new journal category, or
  connect confirmed quest addresses after memory-compare investigation.
---

# Quest Pattern — Backend Integration

Wire `Quests/**/*Addresses.json` into the backend pipeline up to **event
generation** (`JournalChanged` via `StateEventFactory`). Stops before SignalR /
frontend.

**Prerequisite:** addresses confirmed (skill `memory-compare`).

## When NOT to use

| Case | Use instead |
|------|-------------|
| Scalar field on Player / Party / Digimon / Battle / Auctions / Npcs | `address-field-backend` |
| Brand-new non-journal entity on `State` | `memory-entity-backend` |
| Frontend journal UI | `quest-pattern-frontend` |

## Reuse rule (critical)

Each new tracker reuses existing quest types. **Do not** create per-quest
classes like `MuramasaResource`.

| Layer | Reuse as-is |
|-------|-------------|
| Addresses | `QuestAddresses`, `StepAddresses`, `RequisiteAddresses` |
| Readers | `QuestReader`, `StepReader`, `RequisiteReader` |
| Resources | `QuestResource`, `StepResource`, `RequisiteResource` |
| Domain | `Quest`, `Step`, `Requisite` |
| Assemblers | `QuestAssembler`, `StepAssembler`, `RequisiteAssembler` |
| Events | `QuestDiffer`, `StepDiffer`, `RequisiteDiffer`, `QuestConverter`, `StepConverter`, `RequisiteConverter`, `QuestDTO`, `StepDTO`, `RequisiteDTO` |

Create new types only when adding a **new category** on `Journal`, or a
category-specific normalization assembler (see step 5).

## Category map

| Category | Definitions folder | Journal property | Assembler | Repository getter |
|----------|-------------------|------------------|-----------|-------------------|
| Main quest | `Quests/MainQuestAddresses.json` | `MainQuest` (single) | `MainQuestAssembler` (cascade) | `GetMainQuest()` |
| Side quests | `Quests/SideQuests/` | `SideQuests` | `QuestAssembler` | `GetAllSideQuests()` |
| Legendary weapons | `Quests/LegendaryWeapons/` | `LegendaryWeapons` | `QuestAssembler` | `GetAllLegendaryWeapons()` |
| DRI agents | `Quests/DriAgents/` | `DriAgents` | `QuestAssembler` | `GetAllDriAgents()` |
| Duel Island | `Quests/DuelIsland/` | `DuelIsland` | `DuelIslandAssembler` (normalization) | `GetAllDuelIsland()` |

All categories are wired. Current trackers: [backend-status.md](backend-status.md).

## JSON schema

```json
{
    "Id": "questId",
    "Requisites": [
        { "Id": "otherQuestOrItemId", "Address": "0x00048DC2" }
    ],
    "Steps": [
        {
            "Number": 1,
            "Address": "0x0004B38C",
            "BitMasks": ["0x04"],
            "Requisites": [
                { "Id": "itemId", "Address": "0x00048DD7", "BitMasks": ["0x01"] }
            ]
        }
    ]
}
```

- `Requisites` exist at **two levels**: quest root (gate for the whole quest, e.g. `SunTrophyAddresses.json` → `asukaTrophy`; `DriAgentPatamonAddresses.json` → `submarimon`) and per step (e.g. DRI step 3 → `{rookie}DDNA`). Both are optional.
- `BitMasks` (array) on steps **and** requisites. Empty = raw byte (`!= 0`). Multiple = **all** must be set. Singular `BitMask` → normalize to `"BitMasks": ["0x04"]`.
- Single mask on one line (`"BitMasks": ["0x04"]`); multiline only for many entries.
- **Ids in camelCase** (`muramasa`, `driAgentGuilmon`, `guilmonDDNA`). `Id` must match the frontend quest id.
- Step `Number` sequential and unique within the quest.

---

## Workflow A — Tracker in an existing category (common case)

`AddressesRepository` auto-discovers `*.json` in each category folder (sorted
by file name). No C# edit needed.

- [ ] Create `Backend/Memory/Definitions/Quests/{Category}/{Name}Addresses.json`
- [ ] Normalize schema (camelCase ids, `BitMasks` arrays)
- [ ] **SKIP** `AddressesRepository`, `IAddressesRepository`, `QuestLoader`, `JournalLoader`, assemblers, differs, DTOs
- [ ] Tests (see skill `backend-tests` for conventions):
  - `Tests/Integration/Memory/Repositories/AddressesRepositoryDefinitionsTests.cs` — bump the category count in `RealDefinitions_ShouldLoadEveryQuestFolder`
  - `Tests/Integration/Application/Loaders/QuestLoaderTests.cs` — `Load{Category}_*`: `Count++`, mock new addresses, assert id / step values / requisites (lookup by `Id`, not list index)
  - `Tests/Unit/Application/Loaders/JournalLoaderTests.cs` — only if the category list shape changed (it mocks `IQuestLoader`)
- [ ] Retrofeed [backend-status.md](backend-status.md)

Example: the 8 DRI agents (`driAgentGuilmon` … `driAgentPatamon`) were all added this way — 3 steps (talk on `0x4B38C`, boss on `0x4B3B7`/`0x4B3B8`, deliver on a per-agent byte) and a `{rookie}DDNA` requisite on step 3.

**Stop.** Do not touch frontend.

---

## Workflow B — New journal category

Only when a new `Journal` property is needed. Use `DuelIsland` (latest) as the template.

### 1. Definitions + repository

- [ ] Folder `Backend/Memory/Definitions/Quests/{Category}/` with at least one JSON
- [ ] `AddressesRepository.GetAll{Category}()` via `LoadAllQuestAddressesFromFolder(ref cache, "Quests/{Category}")` + cache field
- [ ] Expose on `IAddressesRepository`

### 2. Loaders

| File | Change |
|------|--------|
| `Application/Loaders/Interfaces/IQuestLoader.cs` | `List<QuestResource> Load{Category}()` |
| `Application/Loaders/QuestLoader.cs` | `[.. addressesRepository.GetAll{Category}().Select(questReader.Read)]` |
| `Memory/Resources/JournalResource.cs` | `List<QuestResource> {Category}` |
| `Application/Loaders/JournalLoader.cs` | populate from `questLoader.Load{Category}()` |

DI: **SKIP** — `QuestReader` / `QuestLoader` already registered in `Backend/Infrastructure/DependencyInjection.cs`.

### 3. Domain + events

| File | Change |
|------|--------|
| `Domain/Models/Journal.cs` | `List<Quest> {Category}` + `Equals` / `GetHashCode` |
| `Domain/Assemblers/JournalAssembler.cs` | `[.. resource.{Category}.Select(QuestAssembler.Assemble)]` (or category assembler) |
| `Events/DTO/JournalDTO.cs` | `Optional<List<QuestDTO>> {Category}` |
| `Events/Converters/JournalConverter.cs` | map collection |
| `Events/Diffing/JournalDiffer.cs` | `GenerateQuestsDtos(newJournal.{Category}, previousJournal.{Category})` + `dto with { ... }` |

`JournalEventFactory` / `StateEventFactory` / `JournalProvider` / `StateComposer`: **no change**.

### 4. Tests

- [ ] `AddressesRepositoryDefinitionsTests` — new category count
- [ ] `QuestLoaderTests.Load{Category}_*`
- [ ] `Tests/Unit/Application/Loaders/JournalLoaderTests.cs` (mock the new `IQuestLoader` method)
- [ ] `JournalAssemblerTests`, `JournalDifferTests`, `ModelEqualityTests` (Journal equality)

---

## Step 5 — Normalization (conditional)

Default is **pass-through** (`QuestAssembler`). Only add a normalization when
the product explicitly asks:

- Main quest cascade lives inline in `MainQuestAssembler.Assemble` (later step done → earlier steps done). **Main quest only.**
- Duel Island normalization lives in `DuelIslandAssembler` (requisite not met → all steps false; trophy done → earlier steps true). **Duel Island only.**
- New normalization → new `Domain/Assemblers/Journals/{Category}Assembler.cs` wrapping `QuestAssembler.Assemble`, plus `{Category}AssemblerTests`. Never reuse another category's normalization silently.

---

## Decision helper

```
New Quests/** JSON ready?
  → Category already on Journal (see category map)?
      YES → Workflow A (JSON + tests + status)
      NO  → Workflow B (repository + loaders + domain + events + tests)
  → Product asked for progression fix-up?
      YES → Step 5 (category assembler)
  → Same byte as existing tracker?
      → Likely another bit on the same byte; still a separate quest id
```

## Additional resources

- Pipeline and file map: [backend-pipeline.md](backend-pipeline.md)
- Integration status: [backend-status.md](backend-status.md)
- Investigation: `.cursor/skills/memory-compare/`
- Test conventions: `.cursor/skills/backend-tests/`
- Rules: `.cursor/rules/digivice-backend.mdc`, `.cursor/rules/digivice-tests.mdc`, `.cursor/rules/digivice-business.mdc` (Journal)
