# Seabed underwater routing — memory investigation

Domain: map / location  
Status: confirmed — `SeabedRoute` / `IsSubmerged` in `PlayerAddresses.json` + Player pipeline; frontend pending  
Related: [known-patterns.md](known-patterns.md), [memory-regions.md](memory-regions.md)

---

## Gameplay context

Digimon World 2003 reuses the same seabed map IDs (`02Ex`) across multiple
surface-to-surface routes. A dive point on the surface selects which corridor
and which exit destination apply — the current `MapId` alone cannot distinguish
routes while underwater.

`0x48D78` identifies the **corridor** (the dock pair), not the entry dock.
Entering from either end of the same link keeps the same route context; only
the seabed segment order and `PreviousMapId` reverse.

Example routes (confirmed):

| Surface A | Seabed path (A→B) | Surface B | `0x48D78` |
|-----------|-------------------|-----------|-----------|
| `023E` Suzaku City | `02E2` → `02E0` | `0241` Suzaku UG Lake | `0x07` |
| `0227` Divermon's Lake | `02E2` → `02E0` | `0228` Duel Island | `0x08` |
| `0228` Duel Island | `02E0` → `02E2` | `0227` Divermon's Lake | `0x08` *(same corridor, reverse)* |

Location IDs reference `Frontend/src/database/location/location.json`.

---

## Investigation method

RAM was captured before and after each transition using MemoryScanner `compare`.
No persistent snapshot archive is required — the methodology below is what
matters for reproducing or extending this work.

### Step 1 — Isolate dive entry

Compare **surface before dive** vs **first seabed map after dive**.

- Expect `0x4B3F8` (known `MapId`) to change to a `02Ex` value.
- Look for a byte near `0x4B3F8` that stores the **surface map left behind**
  (`0x3E` from Suzaku, `0x27` from Divermon's Lake) → **`0x4B400`**.
- Look for bytes set once on dive and unchanged while underwater → **`0x48D78`**
  and **`0x48D7A`**.

### Step 2 — Walk between seabed segments

Compare **seabed map A** vs **seabed map B** on the same route.

- `0x4B3F8` changes (`E2` → `E0`).
- `0x4B400` rolls forward (previous map becomes the segment just left).
- `0x48D78` must **stay constant** within the same underwater session.
- Filter noise: `0x4B824+` (encounter cache), `0x48D6C–84` (coordinates).

### Step 3 — Cross-route control on shared seabed

Run the same seabed path from **two different surface entries** (Suzaku vs
Divermon's Lake).

- On the **first** seabed segment, `0x4B400` differs (`3E` vs `27`).
- On the **second** seabed segment, `0x4B3F8` and `0x4B400` are **identical**
  across routes (`E0` / `E2`) — route cannot be inferred from those alone.
- `0x48D78` **still differs** (`07` vs `08`) → primary route discriminator.

### Step 4 — Emerge to surface

Compare **last seabed** vs **surface after exit**.

- `0x4B3F8` → exit map (`41` or `28`).
- `0x4B400` rolls to last seabed segment (`E0`).
- `0x48D78` and `0x48D7A` clear to `0x00`.
- Exit destination is not encoded in `0x4B400` at emerge time; it was resolved
  using route context (`0x48D78`) set at dive.

### Step 5 — Reverse-direction control (same corridor)

Run the **same dock pair** starting from the opposite surface dock (e.g.
Duel Island → Divermon's Lake after documenting Divermon's Lake → Duel Island).

- Seabed segment order reverses (`E0` → `E2` instead of `E2` → `E0`).
- On first dive, `0x4B400` equals the new surface entry (`28` from Duel Island).
- `0x48D78` stays the **same** as the forward run (`08`) for the whole session.
- Confirms `D78` is corridor identity, not entry-dock identity.

Snapshots used for the reverse Divermon's ↔ Duel run:
`Tools/MemoryScanner/Snapshots/before_dive_duel_island.bin`,
`first_seabed_after_dive.bin`, `second_seabed_after_dive.bin`,
`divermons_lake_after_emerge.bin`.

---

## Confirmed addresses

### Map block (`+8` stride)

| Address | Field | Type | Behavior |
|---------|-------|------|----------|
| `0x0004B3F8` | Current MapId | u8 | Already in `PlayerAddresses.json`. Physical map the player is on. |
| `0x0004B400` | PreviousMapId | u8 | Rolling: on each transition, receives the map the player **just left**. On first dive, equals surface entry map. |
| `0x0004B410` | MapId mirror | u8 | Tracks current `MapId` (same value as `0x4B3F8` in all observed transitions). |

### Player position block

| Address | Field | Type | Behavior |
|---------|-------|------|----------|
| `0x00048D68` | PreviousMapId mirror | u8 | Mirrors `0x4B400` on every observed transition (~32 bytes before name at `0x48D88`). |
| `0x00048D78` | SeabedRoute | u8 | In `PlayerAddresses.json` as `SeabedRoute`. Set on dive; **persists** for the whole underwater session; cleared on surface emerge. Identifies the **corridor** (dock pair), independent of which end you enter. |
| `0x00048D7A` | IsSubmerged | u8 | In `PlayerAddresses.json` as `IsSubmerged`. `0x01` while underwater; `0x00` on surface. Domain maps to `bool` (`== 0x01`). Indicates session, not which route. |

---

## Evidence — Suzaku City route

| Stage | `B3F8` | `B400` | `D78` | `D7A` |
|-------|--------|--------|-------|-------|
| Suzaku City (before dive) | `3E` | `40` *(stale)* | `00` | `00` |
| First seabed (`02E2`) | `E2` | `3E` | `07` | `01` |
| Second seabed (`02E0`) | `E0` | `E2` | `07` | `01` |
| Suzaku UG Lake (after emerge) | `41` | `E0` | `00` | `00` |

Key compares: dive `3E→E2` with `B400: 40→3E`, `D78: 00→07`; segment
`E2→E0` with `B400: 3E→E2`; emerge `E0→41` with `D78: 07→00`.

---

## Evidence — Divermon's Lake route

| Stage | `B3F8` | `B400` | `D78` | `D7A` |
|-------|--------|--------|-------|-------|
| Divermon's Lake (before dive) | `27` | `00` | `00` | `00` |
| First seabed (`02E2`) | `E2` | `27` | `08` | `01` |
| Second seabed (`02E0`) | `E0` | `E2` | `08` | `01` |
| Duel Island (after emerge) | `28` | `E0` | `00` | `00` |

On second seabed, `B3F8`/`B400` match the Suzaku run (`E0`/`E2`) but `D78`
differs (`08` vs `07`) and exits differ (`28` vs `41`).

---

## Evidence — Duel Island → Divermon's Lake (reverse of `0x08`)

| Stage | `B3F8` | `B400` | `D78` | `D7A` |
|-------|--------|--------|-------|-------|
| Duel Island (before dive) | `28` | `E0` *(stale)* | `00` | `00` |
| First seabed (`02E0`) | `E0` | `28` | `08` | `01` |
| Second seabed (`02E2`) | `E2` | `E0` | `08` | `01` |
| Divermon's Lake (after emerge) | `27` | `E2` | `00` | `00` |

Same `D78` (`08`) as Divermon's Lake → Duel Island. Only segment order and
surface ends reverse. Mirrors (`B410` / `D68`) matched `B3F8` / `B400` on
every stage.

---

## Route context values (partial)

| `0x48D78` | Dock pair (bidirectional) | Status |
|-----------|---------------------------|--------|
| `0x07` | `023E` Suzaku City ↔ `0241` Suzaku UG Lake | confirmed (one direction tested) |
| `0x08` | `0227` Divermon's Lake ↔ `0228` Duel Island | confirmed both directions |

Other dive points not yet mapped. Values may be route indices, not derived from
map IDs directly.

---

## Discarded / secondary

| Address | Reason |
|---------|--------|
| `0x0004B618`–`0x653` | Entity pointer table cleared/rebuilt on map load |
| `0x0004B824`–`0xBB00` | Encounter cache — session noise |
| `0x00048D6C`–`0x84` | Spawn coordinates, facing, direction |
| `0x0004B428`–`0x429` | In-map spawn coordinates |
| `0x0004B3DC` | Incremented `4C→4D` only on Suzaku seabed segment transition `(suspected)` segment counter |

---

## Tracker implications

### Done (backend Player pipeline)

- `SeabedRoute` (`0x48D78`) and `SeabedRouteType` (`0x48D7A`, formerly
  documented as IsSubmerged) are in `PlayerAddresses.json`, read by
  `PlayerReader`, assembled on `Player`, and emitted via `PlayerChanged`
  (`PlayerDTO`).
- `PreviousMapId` (`0x4B400`) is in `PlayerAddresses.json` — same shape as
  `MapId` (Int16 → hex `X4` string) through the Player pipeline and frontend
  syncer (`previousMapId`).

### Still open

When `MapId` is a seabed ID (`02Ex`) and submerged (`SeabedRouteType == 1`):

1. Use `SeabedRoute` + a **corridor table** (static) to resolve dock pair /
   reachable exits — table not built yet; do not assume a single “forward”
   exit from the route byte alone.
2. On the **first** underwater segment only, `PreviousMapId` equals the
   surface entry map (useful to know *which end* you entered from) — wired;
   UI / route logic may still need to consume it.
3. Broader frontend use of seabed fields beyond Map/Seabed panels if needed.

---

## Status

- [x] Confirmed via paired compares (two full corridors; Suzaku one way,
      Divermon's ↔ Duel both ways)
- [x] `0x48D78` confirmed as corridor identity (direction-independent)
- [x] Retrofed to `memory-regions.md` and `known-patterns.md`
- [x] Integrated in `PlayerAddresses.json` / backend (`SeabedRoute`, `SeabedRouteType`)
- [x] Frontend syncer/store for `SeabedRoute` / `SeabedRouteType`
- [x] `PreviousMapId` (`0x4B400`) in backend + frontend syncer
- [ ] Route table complete for all dive points
- [ ] Reverse test for Suzaku City ↔ Suzaku UG Lake (`0x07`)
