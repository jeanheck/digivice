# Phase 11 — Quest Configuration (Side Quests + Main Quest)

## Context

The Digivice app tracks the player's quest progress in real-time. Quests have **prerequisites** (e.g., owning a key item) and **steps** that are completed via in-game triggers (memory flags: 0 → 1).

The Folder Bag side quest already exists as a reference implementation. This phase adds more side quests and investigates the quest trigger system in memory.

---

## Scope

### New Side Quests

| # | Quest | Prerequisite | Item Memory State |
|---|---|---|---|
| 2 | Tree Boots | Folder Bag (owned) | ✅ Discovered (`0x00048DB4`) |
| 3 | Fishing Pole | Folder Bag (owned) | Needs discovery |

### Architecture (already in place)

- `Quest.cs` → base model with `Prerequisites` and `Steps`
- `SideQuest.cs` → extends `Quest`
- `QuestAddressStep.cs` → maps step ID to memory address
- `Requisite.cs` → prerequisite with `Description` and `IsDone`
- `GameStateService.cs` → builds journal, reads memory triggers

### Memory Investigation Required

1. **Item addresses**: ~~Find the real memory addresses for Tree Boots~~ ✅ Done (`0x00048DB4`) and Fishing Pole (currently mocked to FolderBag address `0x00048F42`).
2. **Quest triggers**: Investigate whether NPC conversation triggers use a similar 0→1 byte flag system, or a different mechanism. These determine individual quest step completion.

---

## Implementation Steps

### Tree Boots Quest
- [x] Discover real memory address for the Tree Boots item
- [x] Investigate quest step triggers (talk to NPC, etc.)
- [x] Create `TreeBoots.cs` side quest definition
- [x] Create `TreeBootsAddress.cs` with memory step addresses
- [x] Wire into `GameStateService.GetJournal()`
- [ ] Test with real game data

### Fishing Pole Quest
- [ ] Discover real memory address for the Fishing Pole item
- [ ] Investigate quest step triggers
- [ ] Create `FishingPole.cs` side quest definition (mocked data for now)
- [ ] Create `FishingPoleAddress.cs` with memory step addresses (mocked)
- [ ] Wire into `GameStateService.GetJournal()`

### Cleanup
- [ ] Update `ImportantItemsAddresses.cs` with real addresses once discovered
- [ ] Remove Gold ID and Platinum ID milestones from frontend and backend

---

## Notes

- Quest triggers may use a different memory mechanism than key items. Need to compare save states before/after talking to specific NPCs.
- Fishing Pole quest data is mocked for now — memory investigation deferred.
- The `Prerequisites` system already supports checking `ImportantItems` ownership ("Do you have Folder Bag?").
