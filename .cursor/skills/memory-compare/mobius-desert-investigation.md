# Mobius Desert cell routing — memory investigation

Domain: map / location  
Status: confirmed — cell-pair discriminator at `0x48D7A` (`MapVariant`, formerly SeabedRouteType)  
Related: [seabed-routing-investigation.md](seabed-routing-investigation.md), [known-patterns.md](known-patterns.md), [memory-regions.md](memory-regions.md)

---

## Gameplay context

Mobius Desert is a wraparound 4×4 maze. Leaving a border loops you to the
opposite side. The game reuses only **two** physical map IDs for all 16
logical cells:

| MapId | `location.json` |
|-------|-----------------|
| `0258` | Mobius Desert |
| `0259` | Mobius Desert 2 |

Same problem as Seabed: `MapId` alone cannot tell *which* of the 16 cells the
player is in. Digivice needs a second discriminator (analogous to
`SeabedRoute`).

Snapshots (all inside Mobius):
`Tools/MemoryScanner/Snapshots/{A,B,C,D}{1,2,3,4}.bin` — one capture per
logical cell. Naming assumes **letter = row (A–D)**, **number = column (1–4)**.

---

## Investigation method

1. Dump known map/player fields across all 16 snapshots (`MapId`,
   `PreviousMapId`, `SeabedRoute` / `MapVariant`, tile X/Y).
2. Exhaustive RAM scan for bytes that match an 8-pair pattern (same value on
   two cells that share a MapId flip).
3. Verify that `(MapId, candidate)` is unique for all 16 cells.
4. Spot-check with MemoryScanner `compare` between same-MapId / different-cell
   and same-pair / different-MapId pairs.

---

## Confirmed addresses

### Map block

| Address | Field | Type | Behavior in Mobius |
|---------|-------|------|--------------------|
| `0x0004B3F8` | Current MapId | u16 LE | Only `0258` or `0259`. Eight cells share each ID. |
| `0x0004B400` | PreviousMapId | u16 LE | Usually the other Mobius map (`0258`↔`0259`). **Exception:** `A3` had `0257` (entry from outside the maze). Rolling history — **not** a stable cell id. |
| `0x00048D68` | PreviousMapId mirror | u8 low of prev | Mirrors `0x4B400` (same as seabed). |

### Cell-pair discriminator (primary find)

| Address | Field (existing JSON name) | Type | Behavior in Mobius |
|---------|----------------------------|------|--------------------|
| `0x00048D7A` | `MapVariant` | u8 | Values **`0x01`–`0x08`**. Each value is shared by **exactly two** cells that form a MapId pair (`0258` + `0259`). |
| `0x00048D78` | `SeabedRoute` | u8 | **Constant `0x01`** on all 16 snapshots. Not the per-cell discriminator here. |

**Identity rule (confirmed):**

```
MobiusCell = (MapId ∈ {0258, 0259}, MapVariant ∈ 1..8)
```

All 16 combinations appear exactly once across the snapshot set.

This is the Seabed pattern with roles partly swapped:

| Area | Corridor / instance id | Segment / asset id |
|------|------------------------|--------------------|
| Seabed | `0x48D78` (route 7, 8, …) | `MapId` `02Ex` + `0x48D7A` = submerged (`0`/`1`) |
| Mobius | `0x48D7A` (pair 1–8) | `MapId` `0258`/`0259` + `0x48D78` stuck at `0x01` |

`0x48D7A` is therefore **overloaded**: binary submerged flag underwater, or
Mobius cell-pair index `1..8` in the desert. Product code that treats
`MapVariant == 1` as “underwater only” will mis-read Mobius.

---

## Evidence — full 16-cell matrix

| Snap | MapId | PrevMapId | `D78` | `D7A` | Identity |
|------|-------|-----------|-------|-------|----------|
| A1 | `0258` | `0259` | `01` | `02` | 0258 + 2 |
| A2 | `0259` | `0258` | `01` | `02` | 0259 + 2 |
| A3 | `0258` | `0257` | `01` | `01` | 0258 + 1 |
| A4 | `0259` | `0258` | `01` | `01` | 0259 + 1 |
| B1 | `0259` | `0258` | `01` | `04` | 0259 + 4 |
| B2 | `0258` | `0259` | `01` | `04` | 0258 + 4 |
| B3 | `0259` | `0258` | `01` | `03` | 0259 + 3 |
| B4 | `0258` | `0259` | `01` | `03` | 0258 + 3 |
| C1 | `0258` | `0259` | `01` | `06` | 0258 + 6 |
| C2 | `0259` | `0258` | `01` | `06` | 0259 + 6 |
| C3 | `0258` | `0259` | `01` | `05` | 0258 + 5 |
| C4 | `0259` | `0258` | `01` | `05` | 0259 + 5 |
| D1 | `0259` | `0258` | `01` | `08` | 0259 + 8 |
| D2 | `0258` | `0259` | `01` | `08` | 0258 + 8 |
| D3 | `0259` | `0258` | `01` | `07` | 0259 + 7 |
| D4 | `0258` | `0259` | `01` | `07` | 0258 + 7 |

### Grid layout (snapshot names → `D7A` / MapId)

Horizontal twins share `D7A` and flip MapId:

```
        col1          col2          col3          col4
row A   A1 58/02      A2 59/02      A3 58/01      A4 59/01
row B   B1 59/04      B2 58/04      B3 59/03      B4 58/03
row C   C1 58/06      C2 59/06      C3 58/05      C4 59/05
row D   D1 59/08      D2 58/08      D3 59/07      D4 58/07
```

(`MapId` abbreviated as `58`/`59`, `D7A` as decimal.)

Pair numbering pattern (from this naming):

| Row | Left pair (cols 1–2) `D7A` | Right pair (cols 3–4) `D7A` |
|-----|----------------------------|-----------------------------|
| A | 2 | 1 |
| B | 4 | 3 |
| C | 6 | 5 |
| D | 8 | 7 |

i.e. left = `(rowIndex+1)*2`, right = `(rowIndex+1)*2 - 1` with `rowIndex` 0–3.

### Compare spot-checks

| Pair | Expectation | Observed |
|------|-------------|----------|
| A1 → A2 | Same `D7A` (`02`), MapId `58→59` | `0x4B3F8` flips; `0x48D7A` unchanged |
| A1 → A3 | Same MapId `0258`, `D7A` `02→01` | `0x48D7A: 0x02 → 0x01` |
| A1 → C1 | Same MapId `0258`, `D7A` `02→06` | `0x48D7A: 0x02 → 0x06` |
| A2 → A4 | Same MapId `0259`, `D7A` `02→01` | `0x48D7A: 0x02 → 0x01` |

### Exhaustive uniqueness

Full-RAM byte scan: **`0x48D7A` is the only address** whose values form the
eight exact pairs above (each value shared by exactly one MapId twin). No
other byte encodes the same 8-pair Mobius topology.

`(MapId, 0x48D7A)` → 16 unique keys (verified).

---

## Discarded / secondary

| Address / signal | Reason |
|------------------|--------|
| `0x48D78` (`SeabedRoute`) | Constant `0x01` on all 16 cells — marks Mobius session-ish state, not which cell |
| Tile X/Y `0x48D6D` / `0x48D71` | Position *inside* the current map asset; changes with walking; not a stable maze-cell enum |
| `0x0E2E0`, `0x4DE30`, `0x48D82` | Volatile / facing / zone pointers — not a clean 1–8 enum (see map-subzones) |
| Encounter cache `0x4B824+` | Session noise; many unique bytes but not semantic cell ids |
| `PreviousMapId` alone | Ambiguous after wrap transitions; entry from outside (`A3`←`0257`) breaks Mobius-only assumptions |

---

## Product implications (not integrated yet)

- Backend already reads `0x48D7A` as `MapVariant`. No new address needed for
  the discriminator.
- Frontend location resolve for Mobius should key on **`mapId` + `mapVariant`**,
  similar to Seabed’s `route + mapId + mapVariant`.
- Static table: 16 entries `(0258|0259, 1..8)` → logical cell / exits / labels.
- Do **not** treat `mapVariant === 1` as “underwater” without also checking
  MapId (Mobius uses `1` as a valid cell-pair id).

Suggested guard sketch:

```text
if MapId in {0258, 0259}:
    mobiusCell = (MapId, MapVariant)   // MapVariant ∈ 1..8
elif SeabedRoute != 0:
    seabedCorridor = SeabedRoute            // MapVariant 0/1 submerged
```

---

## Status

- [x] Confirmed across all 16 Mobius snapshots
- [x] Retrofed to memory-regions.md / known-patterns.md
- [ ] Integrated as Mobius-aware field / static cell table (pending product work)
- [ ] Manual live check: walk A1↔A2 (D7A stable), A1→A3 (D7A changes), leave maze (D78/D7A clear?)
