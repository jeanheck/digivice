# Phase 10 — Final Milestones Configuration

## Context

Milestones are tracked achievements that reflect the player's progress through Digimon World 3. They are triggered by confirmed memory addresses (e.g., obtaining the Gold Card, Platinum Card, key story events).

Before launch, the milestone list needs to be **audited, finalized, and cleaned up** — removing any test/placeholder entries and ensuring every milestone is accurate and functional.

---

## Scope

### Data Review

- Audit all existing milestone entries in the configuration.
- Confirm that each milestone's **memory address trigger** is correct and tested.
- Remove any placeholder, debug, or test milestones.

### Display & UX

- Finalize display names for all milestones.
- Define ordering/grouping (e.g., by story chapter, by category).
- Ensure icons (if any) are consistent.
- Verify the milestone panel renders correctly when all milestones are incomplete, partially complete, and fully complete.

### Coverage

The milestone list at launch should cover at minimum:

- **Gold Card** — obtained at a key story point.
- **Platinum Card** — obtained at a later story point.
- **Critical story events** — boss defeats, area unlocks, major plot triggers.
- Any additional milestones the game data supports via confirmed memory addresses.

---

## Implementation Steps

- [ ] Audit current milestone configuration file(s)
- [ ] Verify each milestone's memory address is correct and triggers reliably
- [ ] Remove placeholder/test milestones
- [ ] Finalize display names, descriptions, and ordering
- [ ] Validate milestone panel UI with real data (empty → partial → full)
- [ ] Confirm the complete launch milestone roster

---

## Notes

- Milestones depend entirely on confirmed memory addresses. Any milestone without a verified trigger must be removed or deferred.
- Coordinate with `FINAL.md` Phase 3 (Quests) to avoid overlap — some quest completions may also be milestones.
