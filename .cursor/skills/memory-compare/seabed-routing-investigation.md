# Seabed underwater routing — memory investigation

Domain: map / location  
Status: confirmed (not integrated in backend)  
Related: [known-patterns.md](known-patterns.md), [memory-regions.md](memory-regions.md)

---

## Gameplay context

Digimon World 2003 reuses the same seabed map IDs (`02Ex`) across multiple
surface-to-surface routes. A dive point on the surface selects which corridor
and which exit destination apply — the current `MapId` alone cannot distinguish
routes while underwater.

Example routes (confirmed):

| Surface entry | Seabed path | Surface exit |
|---------------|-------------|--------------|
| `023E` Suzaku City | `02E2` → `02E0` | `0241` Suzaku UG Lake |
| `0227` Divermon's Lake | `02E2` → `02E0` | `0228` Duel Island |

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
| `0x00048D78` | Seabed route context | u8 | Set on dive; **persists** for the whole underwater session; cleared on surface emerge. Route discriminator on shared seabed maps. |
| `0x00048D7A` | Submerged session flag | u8 | `0x01` while underwater; `0x00` on surface. Same for all routes — indicates session, not which route. |

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

## Route context values (partial)

| `0x48D78` | Surface entry | Surface exit | Status |
|-----------|---------------|--------------|--------|
| `0x07` | `023E` Suzaku City | `0241` Suzaku UG Lake | confirmed |
| `0x08` | `0227` Divermon's Lake | `0228` Duel Island | confirmed |

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

## Tracker implications (future integration)

When `MapId` is a seabed ID (`02Ex`) and `0x48D7A == 0x01`:

1. Read `0x48D78` to resolve which route / exit table applies.
2. On the **first** underwater segment only, `0x4B400` also equals the surface
   entry map (redundant with `D78` for confirmed routes).

Not yet added to `PlayerAddresses.json` or backend readers.

---

## Status

- [x] Confirmed via paired compares (two full routes)
- [x] Retrofed to `memory-regions.md` and `known-patterns.md`
- [ ] Integrated in `PlayerAddresses.json` / backend
- [ ] Route table complete for all dive points
