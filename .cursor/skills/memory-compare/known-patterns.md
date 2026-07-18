# Known Patterns

Incrementally maintained by the memory-compare skill. Mark `(confirmed)` or
`(suspected)`.

---

## Sequential bits on one address (confirmed)

Legendary weapons share **0x0004B38E** with power-of-two masks:
`0x01` (Eternally) → `0x02` (Invincible) → `0x04` (Muramasa) → `0x08` (Super Nova) → `0x10` (Punishment).

When prior bits are set: expect `0x03 → 0x07 (+0x04)`, `0x02 → 0x0A (+0x08)`, `0x0A → 0x1A (+0x10)`.

JSON: `Quests/LegendaryWeapons/*Addresses.json` — `BitMasks` array per step.

---

## Shared byte, different steps (confirmed)

Main quest reuses bytes with different masks (e.g. **0x4B3E0**: `0x02`, `0x04`,
`0x40`). A compare may show multi-bit changes — identify which **added** bit
matches the event.

JSON: `Quests/MainQuestAddresses.json` — `BitMasks` array.

---

## Raw byte step (confirmed)

Empty `BitMasks: []` → step complete when `byte != 0`.

Example: side quests in `Quests/SideQuests/*.json`.

---

## Text buffer false positive (confirmed)

Several consecutive bytes become ASCII (`0x20` = space, `0x73` = `s`, etc.).
Not persistent flags — discard.

---

## Encounter cache fingerprint (confirmed)

Repeating ~4-byte pattern in **0x4B824–0x4BB00** with ptr/stage changes.
Session noise after battles or map activity. Always discard for progress flags.

---

## Item possession vs permanent progress (confirmed)

- **Permanent progress** (legendary weapon, quest step): survives reload; bit
  on progress bytes (`~0x4B38x`, `~0x4B3xx`).
- **Current possession** (common item): may clear on sell — e.g. Divine Barrier
  at `0x48EC9`. Test with sell or `intersect-changed`.

## Item quantity vs quest requisite (confirmed)

- **Stack quantity** (how many you carry): one byte per catalog item in
  `0x0004858F+` (sequential by item Val.). TNT Ball = **`0x000485BE`**
  (Val. `0x005A`, code `300485BE`).
- **Quest requisite** (has item for journal): separate byte, raw `!= 0` — e.g.
  TNT Ball requisite in `MainQuestAddresses.json` at **`0x00048E57`** (shared
  with other key items).
- Compare hunting `99 → 98` (`0x63 → 0x62`) must hit the quantity byte; a
  requisite flag at `0x48E57` will not decrement when using one from a stack.

---

## DRI agents

Three steps per agent. Definitions: `Backend/Memory/Definitions/Quests/DriAgents/`.

### DRI step 1 — shared byte (confirmed)

Byte **0x4B38C** — one bit per agent, sequential OR:

| Agent | BitMask | Evidence |
|-------|---------|----------|
| Agumon | `0x01` | `0x02 → 0x03` after talk (Guilmon bit already set) |
| Guilmon | `0x02` | `0x00 → 0x02` after talk |
| Patamon | `0x04` | `0x00 → 0x04` after talk |
| Renamon | `0x08` | `0x00 → 0x08` after talk |
| Kotemon | `0x10` | `0x08 → 0x18` after talk (Renamon bit already set) |
| Kumamon | `0x20` | `0x03 → 0x23` after talk |
| Monmon | `0x40` | `0x23 → 0x63` after talk |
| Veemon | `0x80` | confirmed in Definitions |

### DRI step 2 — shared bytes (confirmed)

Byte **0x4B3B7** — one bit per agent (main quest also uses `0x01`, `0x02` on same byte):

| Agent | BitMask | Evidence |
|-------|---------|----------|
| Agumon | `0x04` | `0x0B → 0x0F` after defeat |
| Guilmon | `0x08` | `0x03 → 0x0B` after Wargrowlmon |
| Patamon | `0x10` | `0x03 → 0x13` after MagnaAngemon |
| Renamon | `0x20` | `0x03 → 0x23` after Taomon |
| Kotemon | `0x40` | `0x23 → 0x63` after Kyukimon |
| Monmon | `0x80` | `0x0F → 0x8F` after Armormon |

Byte **0x4B3B8** (adjacent):

| Agent | BitMask | Evidence |
|-------|---------|----------|
| Kumamon | `0x01` | `0x40 → 0x41` after GrapLeomon |
| Veemon | `0x02` | confirmed in Definitions |

### Guilmon (`DriAgentGuilmon`)

| Step | Event (gameplay) | Address | BitMask | Status |
|------|------------------|---------|---------|--------|
| 1 | Talk to DRI agent | `0x4B38C` | `0x02` | confirmed |
| 2 | Defeat Wargrowlmon + DNA | `0x4B3B7` | `0x08` | confirmed |
| 2 | DNA possession (requisite) | `0x48DD2` | raw `!= 0` | confirmed |
| 3 | Deliver DNA to agent | `0x4A7E0` | `0x08` | confirmed |

Snapshots: `Tools/MemoryScanner/Snapshots/investigation_guilmon/`

### DRI step 3 — per-agent byte (confirmed)

| Agent | Address | BitMask | Evidence |
|-------|---------|---------|----------|
| Guilmon | `0x4A7E0` | `0x08` | `0x00 → 0x08` after delivery |
| Agumon | `0x4A028` | `0x06` | `0x00 → 0x06` after delivery (`0x02 \| 0x04`) |
| Veemon | `0x4A404` | `0x07` | confirmed in Definitions |
| Kumamon | `0x49870` | `0x04` | `0x00 → 0x04` after delivery |
| Monmon | `0x49C4C` | `0x05` | `0x00 → 0x05` after delivery |
| Kotemon | `0x49494` | `0x03` | `0x00 → 0x03` after delivery |
| Renamon | `0x4ABBC` | `0x09` | `0x00 → 0x09` after delivery |
| Patamon | `0x4AF98` | `0x0A` | `0x00 → 0x0A` after delivery |

### Agumon (`DriAgentAgumon`)

| Step | Event (gameplay) | Address | BitMask | Status |
|------|------------------|---------|---------|--------|
| 1 | Talk to DRI agent | `0x4B38C` | `0x01` | confirmed |
| 2 | Defeat MetalGreymon + DNA | `0x4B3B7` | `0x04` | confirmed |
| 2 | DNA possession (requisite) | `0x48DB6` | raw `!= 0` | confirmed |
| 3 | Deliver DNA to agent | `0x4A028` | `0x06` | confirmed |

Snapshots: `Tools/MemoryScanner/Snapshots/investigation_agumon/`

### Veemon (`DriAgentVeemon`)

| Step | Event (gameplay) | Address | BitMask | Status |
|------|------------------|---------|---------|--------|
| 1 | Talk to DRI agent | `0x4B38C` | `0x80` | confirmed |
| 2 | Defeat Paildramon + DNA | `0x4B3B8` | `0x02` | confirmed |
| 2 | DNA possession (requisite) | `0x48DD3` | raw `!= 0` | confirmed |
| 3 | Deliver DNA to agent | `0x4A404` | `0x07` | confirmed |

### Kumamon (`DriAgentKumamon`)

| Step | Event (gameplay) | Address | BitMask | Status |
|------|------------------|---------|---------|--------|
| 1 | Talk to DRI agent | `0x4B38C` | `0x20` | confirmed |
| 2 | Defeat GrapLeomon + DNA | `0x4B3B8` | `0x01` | confirmed |
| 2 | DNA possession (requisite) | `0x48F3B` | raw `!= 0` | confirmed |
| 3 | Deliver DNA to agent | `0x49870` | `0x04` | confirmed |

Snapshots: `Tools/MemoryScanner/Snapshots/kumamon_*.bin`

### Monmon (`DriAgentMonmon`)

| Step | Event (gameplay) | Address | BitMask | Status |
|------|------------------|---------|---------|--------|
| 1 | Talk to DRI agent | `0x4B38C` | `0x40` | confirmed |
| 2 | Defeat Armormon + DNA | `0x4B3B7` | `0x80` | confirmed |
| 2 | DNA possession (requisite) | `0x48F18` | raw `!= 0` | confirmed |
| 3 | Deliver DNA to agent | `0x49C4C` | `0x05` | confirmed |

Snapshots: `Tools/MemoryScanner/Snapshots/monmon_*.bin`

### Kotemon (`DriAgentKotemon`)

| Step | Event (gameplay) | Address | BitMask | Status |
|------|------------------|---------|---------|--------|
| 1 | Talk to DRI agent | `0x4B38C` | `0x10` | confirmed |
| 2 | Defeat Kyukimon + DNA | `0x4B3B7` | `0x40` | confirmed |
| 2 | DNA possession (requisite) | `0x48DC3` | raw `!= 0` | confirmed |
| 3 | Deliver DNA to agent | `0x49494` | `0x03` | confirmed |

Snapshots: `Tools/MemoryScanner/Snapshots/kotemon_*.bin`

### Renamon (`DriAgentRenamon`)

| Step | Event (gameplay) | Address | BitMask | Status |
|------|------------------|---------|---------|--------|
| 1 | Talk to DRI agent | `0x4B38C` | `0x08` | confirmed |
| 2 | Defeat Taomon + DNA | `0x4B3B7` | `0x20` | confirmed |
| 2 | DNA possession (requisite) | `0x48DD6` | raw `!= 0` | confirmed |
| 3 | Deliver DNA to agent | `0x4ABBC` | `0x09` | confirmed |

Snapshots: `Tools/MemoryScanner/Snapshots/renamon_*.bin`

### Patamon (`DriAgentPatamon`)

| Step | Event (gameplay) | Address | BitMask | Status |
|------|------------------|---------|---------|--------|
| 1 | Talk to DRI agent | `0x4B38C` | `0x04` | confirmed |
| 2 | Defeat MagnaAngemon + DNA | `0x4B3B7` | `0x10` | confirmed |
| 2 | DNA possession (requisite) | `0x48DD7` | raw `!= 0` | confirmed |
| 3 | Deliver DNA to agent | `0x4AF98` | `0x0A` | confirmed |

Snapshots: `Tools/MemoryScanner/Snapshots/patamon_*.bin`

---

## Blast gauge Int32 (confirmed)

**Domain (in-game, validated):** each Digimon has its **own** blast gauge (0–1000).
Not a single global bar. With 3 Digimons in party, each keeps its own value even
out of battle (no HUD display, but values persist per slot).

**Formula:** `address = 0x00042B74 + (2 × rookieId)` — Int32 LE, range 0–1000.
`rookieId` matches `DigimonsAddresses.json` (`Id` 0–7).

| Address | Digimon | Rookie `Id` | Evidence |
|---------|---------|-------------|----------|
| `0x00042B74` | Kotemon | 0 | `chain-match` `0-blast` / `200-blast` / `400-blast` |
| `0x00042B76` | Kumamon | 1 | derived from formula |
| `0x00042B78` | Monmon | 2 | derived from formula |
| `0x00042B7A` | Agumon | 3 | derived from formula |
| `0x00042B7C` | Veemon | 4 | derived from formula |
| `0x00042B7E` | Guilmon | 5 | `chain-match` `guilmon_0` … `guilmon_999` |
| `0x00042B80` | Renamon | 6 | `chain-match` `renamon_0` … `renamon_999` |
| `0x00042B82` | Patamon | 7 | `chain-match` `patamon_0` … `patamon_999` |

Snapshots: `guilmon_*.bin`, `patamon_*.bin`, `renamon_*.bin` under
`Tools/MemoryScanner/Snapshots/`.

Use `chain-match --size 4 --region full` for counter hunts; byte-only `compare`
will not find these (addresses are below quest region `0x48000`).

---

## Seabed underwater routing (confirmed)

Seabed maps (`02Ex`) are **shared** across surface routes. `MapId` alone cannot
identify which route or exit applies.

Full method and evidence: [seabed-routing-investigation.md](seabed-routing-investigation.md).

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
[mobius-desert-investigation.md](mobius-desert-investigation.md).

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
[map-subzones-investigation.md](map-subzones-investigation.md).

---

## Validation checklist

- **Permanent progress**: reload save — flag persists
- **Possession**: sell/drop — flag clears?
- **Isolated compare**: only the target action, minimal side effects
- **Map control**: same MapId in before/after when hunting non-map flags
