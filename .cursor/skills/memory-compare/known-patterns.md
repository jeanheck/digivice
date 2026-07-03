# Known Patterns

Incrementally maintained by the memory-compare skill. Mark `(confirmed)` or
`(suspected)`.

---

## Sequential bits on one address (confirmed)

Legendary weapons share **0x0004B38E** with power-of-two masks:
`0x01` (Old Wand) → `0x02` (Old Claw) → `0x04` (Rusty Katana) → …

When prior bits are set: expect `0x03 → 0x07 (+0x04)`.

JSON: `Quests/LegendaryWeapons/*Addresses.json` — singular `BitMask` per step.

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

### DRI step 2 — shared byte (confirmed)

Byte **0x4B3B7** — one bit per agent (main quest also uses `0x01`, `0x02` on same byte):

| Agent | BitMask | Evidence |
|-------|---------|----------|
| Agumon | `0x04` | `0x0B → 0x0F` after defeat |
| Guilmon | `0x08` | `0x03 → 0x0B` after Wargrowlmon |

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

### Agumon (`DriAgentAgumon`)

| Step | Event (gameplay) | Address | BitMask | Status |
|------|------------------|---------|---------|--------|
| 1 | Talk to DRI agent | `0x4B38C` | `0x01` | confirmed |
| 2 | Defeat MetalGreymon + DNA | `0x4B3B7` | `0x04` | confirmed |
| 2 | DNA possession (requisite) | `0x48DB6` | raw `!= 0` | confirmed |
| 3 | Deliver DNA to agent | `0x4A028` | `0x06` | confirmed |

Snapshots: `Tools/MemoryScanner/Snapshots/investigation_agumon/`

---

## Validation checklist

- **Permanent progress**: reload save — flag persists
- **Possession**: sell/drop — flag clears?
- **Isolated compare**: only the target action, minimal side effects
- **Map control**: same MapId in before/after when hunting non-map flags
