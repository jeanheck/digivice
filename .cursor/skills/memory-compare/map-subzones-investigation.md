# Map Subzones / Encounter Regions — Investigation

Domain: map / encounters  
Status: **in progress** (2026-07-12)  
Goal: explain same-`MapId` areas with different random-encounter pools (and
Asuka Sewers safe vs danger), for Digivice enemy lists per location.

Full write-up for handoff to a fresh chat. Mark RAM fields `(suspected)` until
validated against Makisha grids.

---

## Problem

Several maps share one `MapId` but behave as disconnected encounter regions:

| Map | MapId | Example regions |
|-----|-------|-----------------|
| Plug Cape | `0x0220` (544) | entrance (Betamon/Vegiemon), beach (Shellmon), mountain (Tuskmon) |
| Asuka Sewers | `0x021B` (539) | early **safe** (no randoms) vs mid-game **danger** (hostile); parts do not connect |
| Jungle Grave | `0x023A` (570) | swamp vs graveyard |
| Shell Beach | `0x021F` (543) | beach vs forest |
| Protocol Forest | `0x0225` (549) | forest vs spider web |

Digivice today often stores one enemy list per `MapId` (or empty `enemies: []`
for Asuka). That is wrong when the map has multiple encounter tables.

**Rejected hypothesis (Asuka pair):** a single “hostile map” bit next to MapId /
main-quest `0x4B3xx`. Safe vs danger had **identical** main-quest bytes;
discriminators were PreviousMapId + spawn + loaded content.

---

## Working hypothesis

The game uses **static sub-areas** inside each stage (Makisha dump), selected at
runtime by player position / zone triggers — not a global hostility flag.

```
MapId (stage) → grids (tile zone values)
              → stage_encounter_areas (up to 5 areas × 8 team pointers)
              → enemy_parties → encounters (global digimon battle rows)
```

Runtime RAM candidates likely track **which area is active** and **which
encounter table pointer is loaded**.

---

## RAM candidates (suspected)

DuckStation Memory Viewer: project offset + `0x80000000`
(e.g. `0x48D82` → `80048D82`).

| Address | Form | Role (suspected) | Evidence |
|---------|------|------------------|----------|
| **`0x00048D82`** | u8 | Room / sub-area id (volatile) | Changes on every same-MapId sub-area pair tested; Plug Cape old snaps 8/9/10 sequential; **also changes within same named area** when standing at different points (`new_plug_cape_*`) — not a stable “entrance=8” enum |
| **`0x0004DE30`** | u32 PSX ptr | Loaded zone / encounter resource | Distinct per sub-area; always `0x801Fxxxx`; neighbors **`0x4DE34` / `0x4DE38` fixed globally** (`0x801F8BF4` / `0x801F27E8`) |
| **`0x0000E2E0`** | u8 (0–3) | **Facing / direction (discarded as area)** | Mountain_1..8 same tile, values cycle 0–3; all `*_new` facing-forward snaps = `1`. Not encounter area |
| `0x00048D6D` / `0x00048D71` | u16 | Player tile X / Y | **Corrected** (was wrongly documented as i32 `@ 0x48D6C`/`70`). Use for Makisha grid lookup |
| `0x00048D6C` / `0x00048D70` | i32 | Spawn / transition block | Volatile; low u16 lanes `@+1`/`+5` hold live tile coords |
| `0x0004B3F8` | Int16 | MapId | Same within each investigation pair |
| `0x0004B400` / `0x00048D68` | PreviousMapId | Entry path (useful for Asuka: `0x020B` vs `0x02E0`) | Not a zone enum |

### Cross-map snapshot matrix (key fields)

| Snapshot | MapId | `0x0E2E0` | `0x4DE30` | `0x48D82` |
|----------|-------|-----------|-----------|-----------|
| plug entrance (old) | `0220` | 0 | `0x801F8F9C` | 8 |
| plug beach (old) | `0220` | 2 | `0x801F2F48` | 9 |
| plug mountain (old) | `0220` | 1 | `0x801F2C6C` | 10 |
| new plug entrance | `0220` | 0 | `0x801F90CC` | 46 |
| new plug beach | `0220` | 2 | `0x801F9120` | 37 |
| new plug mountain | `0220` | **2** | `0x801F2D34` | 53 |
| asuka safe | `021B` | **1** | `0x801F2C8C` | 35 |
| asuka danger | `021B` | **1** | `0x801F94BC` | 41 |
| jungle swamp | `023A` | 3 | `0x801F92E4` | 32 |
| jungle graveyard | `023A` | 0 | `0x801F2D2C` | 29 |
| shell beach | `021F` | 1 | `0x801F8FDC` | 21 |
| shell forest | `021F` | 0 | `0x801F9164` | 16 |
| protocol forest | `0225` | 2 | `0x801F2C78` | 4 |
| protocol spider web | `0225` | 0 | `0x801F8EB4` | 3 |

**Verdict after Makisha grid correlation (2026-07-12):**

- **`0x0E2E0`** tracks the active `stage_encounter_areas` row (**area index − 1**)
  on maps with encounter tables (Plug Cape old set, Shell Beach). Treat as the
  best portable runtime discriminator when subzones differ by encounter table.
- **Makisha `grids` layer 2** (layer 3 on Jungle Grave) holds per-tile zone values;
  layer 0 is mostly uniform (`1`) and is **not** the encounter discriminator.
- Grid tile value alone is **insufficient** (Plug entrance vs beach old both
  `grid2=1`; Protocol forest vs spider web share position). Runtime `0x0E2E0` /
  `0x4DE30` reflect loaded state beyond raw tile.
- **Asuka safe/danger:** identical `grid0=0`, `grid2=1`, `e2e0=1`; only
  `0x4DE30`, `0x48D82`, and PreviousMapId differ. Safe has no random battles
  despite area 1 existing in dump — gating is quest/progress or enable flag, not
  area index alone.

### Grid correlation @ snapshot tile (u16 X `@ 0x48D6D`, Y `@ 0x48D71`)

| Snapshot | X | Y | grid[0] | grid[2] | grid[3] | `0x0E2E0` | Makisha area (e2e0+1) |
|----------|---|---|---------|---------|---------|-----------|------------------------|
| plug entrance (old) | 918 | 770 | 1 | 1 | — | 0 | 1 (teams 39/40) |
| plug beach (old) | 625 | 685 | 1 | 1 | — | 2 | 3 (team 54) |
| plug mountain (old) | 592 | 184 | 1 | 2 | — | 1 | 2 (team 51) |
| plug entrance (new) | 725 | 819 | 1 | 1 | — | 0 | 1 |
| plug beach (new) | 689 | 860 | 1 | 3 | — | 2 | 3 |
| plug mountain (new) | 622 | 410 | 1 | 2 | — | 2 | 3 (boundary w/ beach) |
| asuka safe | 744 | 396 | 0 | 1 | — | 1 | 1 (no randoms in-game) |
| asuka danger | 545 | 170 | 0 | 1 | — | 1 | 1 (teams 85/86) |
| shell beach | 487 | 665 | 1 | 2 | — | 1 | 2 (team 37) |
| shell forest | 533 | 220 | 1 | 1 | — | 0 | 1 (teams 39/40) |
| protocol forest | 705 | 429 | 1 | 1 | — | 2 | 3 (team 45 area 2?) |
| protocol spider | 705 | 429 | 1 | 1 | — | 0 | 1 (team 44) |
| jungle swamp | 844 | 880 | 1 | 0 | 2 | 3 | *(no stage_encounters in tar)* |
| jungle graveyard | 117 | 677 | 1 | 0 | 1 | 0 | *(no stage_encounters in tar)* |

**Verdict after `new_plug_cape_*`:** prefer correlating **grid layer 2/3** tile
values with `0x0E2E0` / encounter tables; do **not** treat `0x48D82` or `0x4DE30`
as stable named-area enums. **`0x0E2E0` ≈ encounter area index − 1** when tables
exist.

---

## Plug Cape five-zone pass (2026-07-12)

User labels (not in-game names): **Entrance, Mountain, Beach, Bridge, Forest**.
15 snapshots: 5 center refs + 10 border pairs (`plug_cape_{A}_{B}_border[2].bin`).

### Reference snapshots — coordinate caveat

| Snapshot | X | Y | `0x0E2E0` | area |
|----------|---|---|-----------|------|
| `plug_cape_mountain` | 625 | 685 | 2 | 3 |
| `plug_cape_beach` | 625 | 685 | 0 | 1 |
| `plug_cape_entrance` | 625 | 685 | 2 | 3 |

All three refs share **identical** tile `(625,685)` but different `0x0E2E0` (0 vs 2).
Treat these three as **unreliable for position**; prefer `forest`, `bridge`, and border
pairs (distinct coordinates).

| Snapshot | X | Y | `0x0E2E0` | grid[2] |
|----------|---|---|-----------|---------|
| `plug_cape_forest` | 320 | 517 | 1 | 1 |
| `plug_cape_bridge` | 503 | 701 | 1 | 1 |

Forest and Bridge centers both read **area 2** (`e2e0=1`, team 51).

### Border pairs — `0x0E2E0` flips (validates area byte)

| Side A | e2e0 | Side B | e2e0 |
|--------|------|--------|------|
| `entrance_bridge_border` | 1 | `entrance_bridge_border2` | 0 |
| `bridge_entrance_border` | 1 | `bridge_entrance_border2` | 2 |
| `forest_beach_border` | 0 | `beach_forest_border` | 2 |
| `forest_bridge_border` | 0 | `bridge_forest_border` | 3 |
| `forest_mountain_border` | 2 | `mountain_forest_border` | 3 |

`0x0E2E0` changes on every tested frontier. Values **0–3** observed (= Makisha areas 1–4).

### User zone → Makisha encounter area (best fit)

| User zone | Typical `0x0E2E0` | Makisha area | Team (slot 0) |
|-----------|-------------------|--------------|---------------|
| Beach (center ref) | 0 | 1 | 39 / 40 |
| Forest, Bridge | 1 | 2 | 51 |
| Entrance, Mountain (refs) | 2 | 3 | 54 |
| Border strips (mixed) | 3 | 4 | 327 / 328 / 49 / 54 |

**Five visual zones collapse to four encounter tables** (+ area 0 empty). Forest and
Bridge share area 2; Entrance and Mountain share area 3 on the ref snaps.

Grid layer 2 at borders is often **3** (vs **1** in zone centers); one mountain↔forest
border tile reads grid[0]=**6** (only occurrence).

### `_new` center refs (enemy-confirmed attempt) — failed

User expected pools: Forest+Entrance = Betamon/Vegiemon; Bridge+Beach = Shellmon;
Mountain = Tuskmon. Snapshots `plug_cape_{zone}_new.bin`:

| Snapshot | X | Y | `0x0E2E0` |
|----------|---|---|-----------|
| entrance_new | 240 | 205 | 1 |
| forest_new | 593 | 592 | 1 |
| beach_new | 240 | 205 | 1 |
| bridge_new | 240 | 205 | 1 |
| mountain_new | 240 | 205 | 1 |

Four of five share **identical** tile `(240,205)`; all five have `e2e0=1`. Cannot
validate zone→pool mapping. Retake with distinct positions after seeing the enemy.

### Mountain series `mountain_1`..`8` — `0x0E2E0` is NOT a stable zone id

All 8 snaps: MapId `0220`, tile **`(625,685)` identical**, `0x48D82` = 8 or 9.

| Snap | `0x0E2E0` | `0x4DE30` |
|------|-----------|----------|
| 1 | 1 | `0x801F2C4C` |
| 2 | 0 | `0x801F2C4C` |
| 3 | 2 | `0x801F9058` |
| 4 | 0 | `0x801F9058` |
| 5 | 3 | `0x801F9058` |
| 6 | 1 | `0x801F9058` |
| 7 | 0 | `0x801F9058` |
| 8 | 2 | `0x801F2C4C` |

Unique `e2e0`: **0,1,2,3** (not 8 distinct). Same tile, same named zone (Mountain),
byte still cycles. **Weakens** “`0x0E2E0` = active encounter area from position.”
Must find another discriminator (or explain why this byte churns in-place).

### `_v3` enemy-confirmed snaps (facing fixed = 1)

| Snapshot | X | Y | g0 | g1 | g2 | Expected pool |
|----------|---|---|----|----|----|---------------|
| entrance_v3 | 918 | 610 | 1 | 230 | 1 | Betamon/Vegiemon |
| forest_v3 | 594 | 592 | 1 | 0 | 1 | Betamon/Vegiemon |
| beach_v3 | 594 | 592 | 1 | 0 | 1 | Shellmon |
| bridge_v3 | 534 | 595 | 1 | 0 | 1 | Shellmon |
| mountain_v3 | 625 | 685 | 1 | 0 | 1 | Tuskmon |

Facing controlled (all `0x0E2E0=1`). Forest and Beach share **identical** tile
`(594,592)` — one position invalid for pool contrast.

**Grid under feet does not separate the 3 pools:** layers 0 and 2 are `1` for all
five; only entrance differs on layer 1 (`230`). Whole-map layer 2 only uses values
`1/2/3` in large geographic blobs, but all five player samples landed on `1`.

Conclusion: exportable Makisha grids work; **tile value at X/Y ≠ confirmed enemy
pool** yet. Need either snaps standing on layer2=`2` and `3` regions, or another
link from position → `stage_encounter_areas`.

---

## Snapshots on disk

Under `Tools/MemoryScanner/Snapshots/`:

- Asuka: `asuka_sewers_safe.bin`, `asuka_sewers_danger.bin`
- Plug Cape: `plug_cape_{entrance,beach,mountain,bridge,forest}.bin` + `*_border*.bin` (2026-07-12 five-zone set); legacy `new_plug_cape_*`
- Jungle: `jungle_grave_{swamp,graveyard}.bin`
- Shell Beach: `shell_beach_{beach,forest}.bin`
- Protocol Forest: `protocol_forest_{forest,spider_web}.bin`

---

## Makisha dump (static structures)

Tools: `C:\Users\Jean\Desktop\Makisha\dmw3-tools\`  
Structs: [dmw3-util `structs`](https://github.com/markisha64/dmw3-util/blob/master/structs/src/lib.rs)  
Loader: `dmw3-tools/src/data.rs` (`MapObject`, `init_maps`)

### Naming

| Concept | Example (Plug Cape) |
|---------|---------------------|
| Folder `WSTAG345.PRO` | **PRO file index 345** (not MapId) |
| `stage_id` | **544 (`0x0220`)** |
| UI name | `screen_name_mapping` → `esstname.toml` idx 29 `"Plug Cape"` |

### Per-map files (complete maps in `maps.tar` / extracted)

| File | Role |
|------|------|
| `stage_id` | u16 in-game map id |
| `grids` | Collision / zone tile hierarchy (strong subzone candidate) |
| `stage_encounter_areas` | `StageEncounterArea`: `steps_index` + 8 team pointers (~5 areas) |
| `stage_encounters` | `StageEncounter`: `team_id`, `stage`, `music` (chunks of 8) |
| `entities` | `EntityData` 20 B: NPC **and** invisible triggers / objects |
| `entity_logics`, `scripts_conditions`, … | Dialogue, flags, spawn conditions |

UI (`maps.rs`): table **Area 0–4** × **Team 0–7** + counters-per-step.

Script flag type **`AreaVisited` (c_type 32)** — game already tracks numbered areas.

### Global encounter tables

| File | Layout |
|------|--------|
| `encounters` | 522 × 12 B `EncounterData` (digimon_id, lv, hp, mp, multiplier) |
| `encounters_pointer` | u32 `0x8002021C` — base of encounters blob in PSX RAM |
| `enemy_parties` | 335 × `PartyData` — 3 ptrs into `encounters` + immunities |

Index formula (`parties.rs`):
`(ptr - (null_encounter_ptr + 0xC)) / 0xC`

### Plug Cape entities (not “7 NPCs”)

`entities` = **7 `EntityData` records** (140 bytes). In-game there is **~1 talking
NPC** (Soccer Kid Hide / Tree Boots). Other slots are engine entities
(triggers, exits, invisible volumes). Four records share sprite **283** and
coords **(967, 901)** — likely non-NPC triggers.

Note: `extracted_maps/WSTAG345.PRO` on disk may only have `stage_id` +
`entities`; full bundle (grids, stage_encounter_*) lives in `maps.tar`. **Done
2026-07-12:** extracted complete bundles for WSTAG320/340/345/370/480 into
`dump/dmw2003/extracted_maps/` (11 files each).

### Stage ↔ WSTAG folder

| MapId | Stage | WSTAG folder |
|-------|-------|--------------|
| 539 `021B` | Asuka Sewers | WSTAG320.PRO |
| 543 `021F` | Shell Beach | WSTAG340.PRO |
| 544 `0220` | Plug Cape | WSTAG345.PRO |
| 549 `0225` | Protocol Forest | WSTAG370.PRO |
| 570 `023A` | Jungle Grave | WSTAG480.PRO |

### Plug Cape encounter areas (from `maps.tar`)

| Area | `steps_index` | Team ids | Notes |
|------|---------------|----------|-------|
| 0 | 0 | — | No random encounters |
| 1 | 3 | 39 / 40 | Entrance pool (`e2e0=0`) |
| 2 | 3 | 51 | Mountain (`e2e0=1` old snap) |
| 3 | 3 | 54 | Beach (`e2e0=2` old snap) |
| 4 | 0 | 327, 328, 49, 54 | Special / card slots |

Shell Beach: area 1 → teams 39/40 (`e2e0=0` forest); area 2 → team 37
(`e2e0=1` beach).

Asuka Sewers: only **area 1** populated (teams 85/86, `steps_index=4`). Areas
2–4 empty. Safe zone has no random battles despite `e2e0=1` in RAM.

Jungle Grave (`WSTAG480.PRO`): `grids` present (4 layers) but `stage_encounters`
and `stage_encounter_areas` blobs are **empty** in `maps.tar` — encounter
tables may live elsewhere or the dump slice is incomplete.

### Analysis tooling

`Tools/MemoryScanner/map_grid_correlate.py` — reads `maps.tar`, samples Makisha
grid layers at snapshot tile coords, prints `e2e0` / `de30` correlation.

`Tools/MemoryScanner/dump_encounters.py` — dumps non-zero rows from
`stage_encounters` with party decode.

---

## Digivice implications (future)

- Location enemies may need **subzone discriminator** (grid value, area index,
  or PreviousMapId for Asuka) — not MapId alone.
- Pattern exists elsewhere: `location.json` phases via `lastMainQuestStepDone`
  (e.g. Wire Forest). Asuka safe/danger is **not** that pattern in the compare.
- Do not wire backend addresses until a candidate is validated against grids.

---

## Next steps

1. ~~Extract full map bundles from `maps.tar`~~ **Done** for all five WSTAG folders.
2. Resolve Jungle Grave encounter source (empty `stage_encounters` in tar).
3. Confirm Asuka safe-zone gating: which quest flag / `steps_index` disables
   area 1 randoms while `e2e0` still reads `1`.
4. Protocol Forest: same tile, different `e2e0` — find non-position trigger
   (PreviousMapId, script flag, facing).
5. Walk Plug Cape mountain → beach slowly; confirm `0x0E2E0` flip at grid2
   boundary (`2` vs `3`).
6. Optional Digivice integration: expose `0x0E2E0` as encounter area index for
   location enemy lookup (user must request explicitly).

---

## Related Digivice docs

- [memory-regions.md](memory-regions.md) — RAM anchors
- [known-patterns.md](known-patterns.md) — patterns (incl. this section)
- [seabed-routing-investigation.md](seabed-routing-investigation.md) — same MapId,
  different routes (related idea, different mechanism)
- Skill: `.cursor/skills/map-subzone-investigate/`
