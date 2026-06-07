---
name: quest-pattern-backend
description: >-
  Integrates quest-pattern memory trackers into the Digivice backend — main
  quest, side quests, legendary weapons, DRI agents. Use when the user asks to
  integrate *Addresses.json into the backend, wire journal loaders, or connect
  confirmed memory addresses after memory-compare investigation.
---

# Quest Pattern — Backend Integration

Wire `*Addresses.json` files into the backend pipeline up to **event
generation** (`JournalChanged` via `StateEventFactory`). Stops before SignalR /
frontend.

**Prerequisite:** addresses confirmed and JSON file exists (see
`memory-compare` skill). User may say: "integrate this in the backend".

## Reuse rule (critical)

Each new tracker (Muramasa, Agumon step 1, Folder Bag) reuses existing quest
types and pipelines. **Do not** create per-quest classes like `MuramasaResource`.

| Layer | Reuse as-is |
|-------|-------------|
| Addresses | `QuestAddresses`, `StepAddresses` |
| Readers | `QuestReader`, `StepReader`, `RequisiteReader` |
| Resources | `QuestResource`, `StepResource` |
| Domain | `Quest`, `Step` |
| Assemblers | `QuestAssembler` |
| Events | `QuestDiffer`, `StepDiffer`, `QuestConverter`, `StepConverter`, `QuestDTO`, `StepDTO` |

Create new types only when adding a **new category slot** on `Journal` (first
Legendary Weapon, first DRI Agent, etc.).

## Category map

| Category | Definitions folder | Journal collection | Reference implementation |
|----------|-------------------|--------------------|--------------------------|
| Main quest | `Quests/MainQuestAddresses.json` | `MainQuest` | Already wired |
| Side quests | `Quests/SideQuests/` | `SideQuests` | FolderBag, TreeBoots, FishingPole |
| Legendary weapons | `LegendaryWeapons/` | `LegendaryWeapons` *(create on first item)* | Side quests pattern |
| DRI agents | `DriAgents/` | `DriAgents` *(create on first item)* | Side quests pattern |

## JSON schema note

`StepAddresses` expects `BitMasks` (array). If the file uses singular
`BitMask`, normalize to `"BitMasks": ["0x04"]`. Empty array = raw byte (`!= 0`).

`Id` in JSON must match the quest id used across backend and (later) frontend.

---

## Integration checklist

Copy and track progress. Skip steps marked **SKIP** when the condition is met.

### 0. Preconditions

- [ ] `*Addresses.json` exists with `Id`, `Steps`, `Address`, bitmasks filled
- [ ] Identify **category** (side quest / legendary weapon / DRI agent)
- [ ] Identify **reference**: existing tracker in same category to mirror

---

### 1. Definitions JSON

- [ ] File in correct folder under `Backend/Memory/Definitions/`
- [ ] Naming: `{Name}Addresses.json` (e.g. `MuramasaAddresses.json`)
- [ ] `BitMask` → `BitMasks` array if needed
- [ ] Step `Number` values sequential and unique within the quest

**SKIP** if user already created the file via memory-compare.

---

### 2. AddressesRepository

File: `Backend/Memory/Repositories/AddressesRepository.cs`
Interface: `IAddressesRepository.cs`

**2a. Tracker in an existing category**

- [ ] Add cached field + private loader method (mirror `GetSideQuestFolderBag`)
- [ ] Expose via `GetXxx()` or append to `GetAllSideQuests()` / `GetAllLegendaryWeapons()`

**2b. First tracker in a new category**

- [ ] Add `GetAllLegendaryWeapons()` or `GetAllDriAgents()` returning `List<QuestAddresses>`
- [ ] Register each JSON file in that list method

**Reference:** `GetAllSideQuests()` + three private loaders.

---

### 3. Journal category slot *(conditional)*

**SKIP** if `Journal` / `JournalResource` / `JournalDTO` already expose this
category (e.g. side quests already exist).

**CREATE** when first item in category (e.g. first Legendary Weapon):

Mirror `SideQuests` in each file:

| File | Change |
|------|--------|
| `Memory/Resources/JournalResource.cs` | `List<QuestResource> LegendaryWeapons` |
| `Domain/Models/Journal.cs` | `List<Quest> LegendaryWeapons` + Equals/GetHashCode |
| `Application/Loaders/JournalLoader.cs` | populate from `QuestLoader` |
| `Application/Loaders/Journals/QuestLoader.cs` | `LoadLegendaryWeapons()` |
| `Domain/Assemblers/JournalAssembler.cs` | assemble list via `QuestAssembler` |
| `Events/DTO/JournalDTO.cs` | `Optional<List<QuestDTO>> LegendaryWeapons` |
| `Events/Converters/JournalConverter.cs` | map collection |
| `Events/Diffing/JournalDiffer.cs` | diff loop (copy side-quest loop) |

Use side quests as the template; keep naming consistent (PascalCase property,
plural list).

---

### 4. Loaders

- [ ] `QuestLoader` — method loads all addresses in category via `questReader.Read`
- [ ] `JournalLoader` — include new collection in `JournalResource`

**SKIP** reader/loader DI registration if reusing `QuestReader` / `QuestLoader`
(already in `DependencyInjection.cs`).

---

### 5. Assembly → State

- [ ] `JournalAssembler` maps new collection (if step 3 was needed)
- [ ] Main quest cascade (`NormalizeMainQuestProgression`) — **do not** apply to side quests / LW / DRI unless explicitly requested

Pipeline: `JournalProvider` → `JournalLoader` → `JournalAssembler` → `State.Journal`
(already wired via `StateComposer`; no change unless new provider needed — usually no).

---

### 6. Events (end of backend scope)

- [ ] `JournalDiffer` emits delta for new/changed quests in the category
- [ ] `JournalEventFactory` — no change if `JournalDiffer` returns DTO
- [ ] `StateEventFactory` — no change if `JournalEventFactory` already included

Verify: step value change → `JournalChanged` event with nested `QuestDTO` /
`StepDTO` delta.

**Stop here.** Do not touch frontend syncers, Vue, or static JSON.

---

### 7. Tests (backend only)

**Integration** — `Tests/Integration/Application/Loaders/`:

- [ ] Loader test: mock `IMemoryReader` for new addresses; assert `QuestResource` id and step values
- [ ] If new category: extend `JournalLoaderTests` to assert collection count

**Unit** — if differ logic changed:

- [ ] `JournalDifferTests`: delta when step value changes in new category

**Reference:** `QuestLoaderTests.LoadSideQuests_ShouldIntegrateSideQuestAddressesAndReaderPipeline`

Mirror corner cases from existing tests (empty requisites, multiple bitmasks, raw byte steps).

---

### 8. Retrofeed *(when applicable)*

Update [backend-status.md](backend-status.md):

- Category wired: yes/no
- Trackers integrated: list ids
- Extension points still open

Append; do not remove entries without reason.

---

## Decision helper

```
New *Addresses.json ready?
  → Category already on Journal?
      NO  → Step 3 (create slot) + Steps 2, 4–7
      YES → Steps 2, 4, 7 only (register + test)
  → Same address byte as existing tracker?
      → Likely new bit on same step; still valid as separate quest id
```

## Additional resources

- File map and pipeline diagram: [backend-pipeline.md](backend-pipeline.md)
- Integration status: [backend-status.md](backend-status.md)
- Prior skill: `.cursor/skills/memory-compare/`
- Code rules: `AI/CODE_RULES.md` (Backend + Tests sections)
