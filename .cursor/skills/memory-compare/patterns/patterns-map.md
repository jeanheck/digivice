# Known Patterns — Map

Seabed routing, Mobius Desert cells, map subzones / encounter regions.

Maintained by the memory-compare skill (retrofeed). Mark `(confirmed)` or `(suspected)`. Append; do not delete without evidence.

---

## Seabed underwater routing (confirmed)

Seabed maps (`02Ex`) are **shared** across surface routes. `MapId` alone cannot
identify which route or exit applies.

Full method and evidence: [seabed-routing-investigation.md](../../../docs/investigations/seabed-routing-investigation.md).

### Rolling PreviousMapId — `0x4B400` (confirmed, in PlayerAddresses)

Wired as `PreviousMapId` on the Player pipeline (`PlayerChanged`) — same type
as `MapId` (`ReadInt16` → domain/DTO string `X4`). Frontend: `previousMapId`.

On every map transition, `0x4B400` receives the map the player **just left**:

- First dive: surface entry map (`0x3E` Suzaku, `0x27` Divermon's Lake).
- Later seabed segments: previous seabed map (`0xE2` after leaving first segment).
- Emerge: last seabed segment (`0xE0`).

Useful as route hint **only on the first underwater segment**. On shared later
segments, `0x4B400` is identical across routes.

`0x48D68` mirrors `0x4B400` in the player block (not in JSON). `0x4B410`
mirrors current `MapId` (`0x4B3F8`).

### SeabedRoute — `0x48D78` (confirmed, in PlayerAddresses)

Wired as `SeabedRoute` on the Player backend pipeline (`PlayerChanged`).

- Set once on dive (`0x00` → route value).
- **Unchanged** while walking between seabed maps on the same session.
- Cleared on surface emerge (`→ 0x00`).
- Identifies the **corridor** (dock pair), not the entry dock — same value
  when diving from either end of the link.
- Primary discriminator when two players share the same `MapId` on seabed.

| `0x48D78` | Dock pair (bidirectional) |
|-----------|---------------------------|
| `0x07` | Suzaku City (`023E`) ↔ Suzaku UG Lake (`0241`) |
| `0x08` | Divermon's Lake (`0227`) ↔ Duel Island (`0228`) *(both directions)* |

Table incomplete — more dive points not yet mapped.

### IsSubmerged / MapVariant — `0x48D7A` (confirmed, in PlayerAddresses)

Wired as `MapVariant` on the Player backend pipeline (`PlayerChanged`).

**Seabed:** `0x01` for the entire underwater session; `0x00` on surface.
Indicates submerged state, not which route. (Investigation notes formerly
called this `IsSubmerged`.)

**Mobius Desert (confirmed):** same byte holds cell-pair index **`0x01`–`0x08`**.
Each value is shared by exactly two cells (`0258` + `0259`). Combined with
`MapId` uniquely identifies all 16 maze cells. See
[mobius-desert-investigation.md](../../../docs/investigations/mobius-desert-investigation.md).

Do not treat `MapVariant == 1` as “underwater only” — Mobius uses `1` as
a valid pair id when `MapId` is `0258`/`0259`.

### How it was found

Paired `compare` across: (1) dive entry, (2) seabed segment walk, (3) two
different surface entries through the same seabed corridor, (4) surface emerge,
(5) reverse direction on the same dock pair. Cross-route diff on step 3
isolated `0x48D78`; step 5 showed `D78` is corridor identity (still `0x08`
for Duel Island → Divermon's Lake).

### Mobius Desert cell-pair — `0x48D7A` (confirmed)

Same address as `MapVariant`. Sixteen snapshots
(`Snapshots/{A–D}{1–4}.bin`): `MapId` only `0258`/`0259`; `0x48D78` stuck at
`0x01`; `0x48D7A` alone matches the 8 horizontal MapId-twin pairs. Exhaustive
RAM scan found no other byte with that topology.

Identity: `(MapId, 0x48D7A)` with `D7A ∈ 1..8`.

---

## Map subzones / encounter regions (suspected)

Same `MapId` can host multiple disconnected encounter pools (Plug Cape,
Asuka Sewers safe vs danger, Jungle Grave, Shell Beach, Protocol Forest).

**Not** a main-quest “hostile bit” on MapId — Asuka safe/danger had identical
`0x4B370`–`0x4B3F0` and same MapId `0x021B`.

Static model (Makisha / dmw3-util): per-stage `grids` +
`stage_encounter_areas` (≤5 areas × 8 teams) → `enemy_parties` → global
`encounters`. Folder `WSTAG345.PRO` = PRO index; Plug Cape `stage_id` =
**544 (`0x0220`)**.

| Address | Status | Notes |
|---------|--------|-------|
| `0x0000E2E0` | suspected area index (0–3) | Changes on Plug/Jungle/Shell/Protocol; **not** Asuka safe↔danger (both 1) |
| `0x0004DE30` | suspected zone resource ptr | Changes with sub-area; `0x4DE34`/`38` globally fixed |
| `0x00048D82` | suspected room/sub-id | Changes every pair; **also** within same named Plug Cape area at boundary points — not a stable enum |

Full evidence, snapshot matrix, Makisha notes, next steps:
[map-subzones-investigation.md](../../../docs/investigations/map-subzones-investigation.md).

---

