---
name: map-subzone-investigate
description: >-
  Continues Digimon World 2003 investigation of map subzones and encounter
  regions (same MapId, different enemy pools). Use when the user mentions Plug
  Cape zones, Asuka Sewers safe vs danger, Makisha grids / stage_encounter_areas,
  0x0E2E0 / 0x4DE30 / 0x48D82, or Digivice location enemies per sub-area.
---

# Map Subzone Investigate

Resume the **same-MapId encounter region** investigation. Do not re-derive
basics from scratch — read the handoff doc first.

## Required reading (in order)

1. [`.cursor/skills/memory-compare/map-subzones-investigation.md`](../memory-compare/map-subzones-investigation.md) — full evidence, snapshot matrix, Makisha model, next steps
2. [`.cursor/skills/memory-compare/SKILL.md`](../memory-compare/SKILL.md) — compare workflow if analyzing new `.bin` diffs
3. Optional: Makisha `dmw3-tools` `src/data.rs` + `src/pages/maps.rs` for `stage_encounter_areas` UI shape

## Current stance

| Claim | Status |
|-------|--------|
| Subzones via Makisha `grids` + `stage_encounter_areas` | **Confirmed** — layer 2/3 tile values + area tables |
| Global “hostile MapId” bit | Rejected for Asuka pair |
| `0x0E2E0` as encounter area index − 1 | **Rejected** — facing/direction (0–3); mountain series + `_new` facing-forward |
| `0x4DE30` / `0x48D82` as named-area enums | Rejected — volatile / session-specific |
| Player tile coords `@ 0x48D6D` / `0x48D71` (u16) | **Confirmed** for grid lookup |

## When user brings new snapshots

1. Confirm MapId identical within the pair/group (`0x4B3F8`)
2. Dump `0x0E2E0`, `0x4DE30` (+ `34`/`38`), `0x48D82`, X/Y `0x48D6C`/`70`, PreviousMapId
3. Append results to [map-subzones-investigation.md](../memory-compare/map-subzones-investigation.md) and retrofeed `known-patterns.md` / `memory-regions.md` only when confirmed
4. Prefer correlating with Makisha **grid cell** at player position over inventing new quest-region flags

## Digivice product work

Do **not** wire new Player fields or change `location.json` enemy phases until a
discriminator is validated. Investigation-only unless the user explicitly asks
to integrate.

## Cheat Engine / DuckStation

Project offset + `0x80000000`:

- Byte `0x0E2E0` → `8000E2E0`
- UInt32 LE `0x4DE30` → `8004DE30`
- Byte `0x48D82` → `80048D82`
- Player X u16 `0x48D6D` → `80048D6D` (Value Type: 2 Bytes)
- Player Y u16 `0x48D71` → `80048D71` (Value Type: 2 Bytes)
