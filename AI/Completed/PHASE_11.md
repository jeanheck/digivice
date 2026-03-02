# Phase 11 — Quest Configuration (Side Quests + Main Quest)

## Context

The Digivice app tracks the player's quest progress in real-time. Quests have **prerequisites** (e.g., owning a key item) and **steps** that are completed via in-game triggers (memory bit fields in RAM).

The Folder Bag side quest already exists as a reference implementation. This phase adds more side quests using memory card save comparison for reliable address discovery.

---

## Scope

### New Side Quests

| # | Quest | Prerequisite | Item Memory State |
|---|---|---|---|
| 2 | Tree Boots | Folder Bag (owned) | ✅ Discovered (`0x048DB4`) |
| 3 | Fishing Pole | Folder Bag (owned) | ✅ Discovered (`0x048DB5`) |

### Key Discovery: Bit Field Quest System

Quest triggers are stored as **bit fields** in RAM, NOT as individual 0→1 byte flags:
- Multiple quests share the same byte (e.g., `0x04B3B0`)
- Each step activates a specific bit
- Some steps use **rotating bits** (only the latest stays on)
- Discovered via **memory card save comparison** (RAM snapshots were unreliable)
- Save→RAM mapping formula: `RAM = save_offset + 0x46A34`

### Architecture

- `QuestAddressStep.cs` → maps step ID to RAM address + `BitMask` (nullable)
  - BitMask set → `(byte & mask) != 0` (bit field mode)
  - BitMask null → `byte == 1` (legacy mode)
- `GameStateService.ApplyQuestSteps()` → centralized step reader, supports both modes

---

## Implementation Steps

### Tree Boots Quest ✅
- [x] Discover real memory address for the Tree Boots item
- [x] Investigate quest step triggers via memory card comparison
- [x] Create `TreeBoots.cs` side quest definition (7 steps)
- [x] Create `TreeBootsAddress.cs` with bit field addresses
- [x] Wire into `GameStateService.GetJournal()`
- [x] Test with real game data — verified steps 1-6 work correctly

### Fishing Pole Quest ✅
- [x] Discover real memory address for the Fishing Pole item (`0x048DB5`)
- [x] Investigate quest step triggers via memory card comparison
- [x] Create `FishingPole.cs` side quest definition (3 steps)
- [x] Create `FishingPoleAddress.cs` with bit field + item flag addresses
- [x] Wire into `GameStateService.GetJournal()`
- [x] Test with real game data

### UI Improvements ✅
- [x] Locked quest style (🔒) for quests with unmet prerequisites
- [x] New quest available style (❗) for quests ready to start
- [x] Prerequisites section in quest details modal
- [x] Milestone tooltips

### Cleanup ✅
- [x] Update `ImportantItemsAddresses.cs` with real addresses
- [x] Remove Gold ID and Platinum ID milestones
- [x] Rename Kicking Boots → Tree Boots across codebase

---

## Step-Level Prerequisites & Inventory Reading ✅

Some quests (e.g., Fishing Pole step 3) require specific items to complete a step:
- [x] `QuestStep.Prerequisites` (optional list of `Requisite`) — added to model
- [x] `Requisite.ItemKey` + `Requisite.ItemType` — links prerequisite to item address
- [x] `ConsumableItemsAddresses.cs` — addresses for stackable items (Power Charge, Spider Web, Bamboo Spear)
- [x] `GameStateService.ApplyStepPrerequisites()` — reads RAM and sets `IsDone` for "consumable" (qty > 0) and "important" (flag == 1)
- [x] UI step prerequisites checklist in `QuestDetailsModal.vue`
- [x] `EventDispatcherService` — step prerequisite change detection for real-time updates

### Discovered Addresses

| Item | RAM Address | Type | Status |
|---|---|---|---|
| Power Charge | `0x048DDB` | Consumable | ✅ |
| Spider Web | `0x048E09` | Consumable | ✅ |
| Bamboo Spear | `0x048E57` | Consumable (weapon) | ✅ |
| Red Snapper | `0x048DB7` | Important Item | ✅ |
