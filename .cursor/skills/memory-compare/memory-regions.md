# RAM Regions

PS1 save RAM: **2 MiB** snapshot (`0x0`–`0x1FFFFF`). MemoryScanner `compare` defaults to
**quest** region (`0x48000`–`0x4D000`); typed commands default to **full** RAM.
Use `--size 4` for Int32 counters (blast gauge, EXP, Bits).

This file is **incrementally maintained** by the memory-compare skill after
each investigation. Append new entries; do not remove without strong evidence.

---

## Player

| Address | Field | Source |
|---------|-------|--------|
| 0x00048D88 | Player name buffer | PlayerAddresses.json |
| 0x00048DA0 | Player bits (money etc.) | PlayerAddresses.json — **volatile** |
| 0x0004B3F8 | MapId | PlayerAddresses.json — changes on map transition |
| 0x00048D78 | SeabedRoute | PlayerAddresses.json — corridor / dock pair; set on dive |
| 0x00048D7A | IsSubmerged | PlayerAddresses.json — `0x01` underwater |

See also **Map / location** for seabed routing fields (including investigation-only mirrors).

## Map / location

| Address | Field | Source |
|---------|-------|--------|
| 0x0004B3F8 | Current MapId | PlayerAddresses.json |
| 0x0004B400 | PreviousMapId (rolling) | seabed-routing investigation — map just left on each transition |
| 0x0004B410 | MapId mirror | seabed-routing investigation — tracks current MapId |
| 0x00048D68 | PreviousMapId mirror (player block) | seabed-routing investigation — mirrors `0x4B400` |
| 0x00048D78 | SeabedRoute (corridor / dock pair) | PlayerAddresses.json — set on dive, persists underwater; same from either entry |
| 0x00048D7A | IsSubmerged (`0x01` = underwater) | PlayerAddresses.json |

Details: [seabed-routing-investigation.md](seabed-routing-investigation.md).

## Party

| Address | Field | Source |
|---------|-------|--------|
| 0x00048DA4 | Party slot 1 | PartyAddresses.json |
| 0x00048DA8 | Party slot 2 | PartyAddresses.json |
| 0x00048DAC | Party slot 3 | PartyAddresses.json |

## Quest / progress flags

Definitions root: `Backend/Memory/Definitions/Quests/`

| Range / Address | Purpose | Source |
|-----------------|---------|--------|
| 0x0004B370 | Main quest + auction story window | `Quests/MainQuestAddresses.json`, DivineBarrierAddresses.txt |
| 0x0004B38A | Auction instance consumed | DivineBarrierAddresses.txt |
| 0x0004B38E | Legendary weapons | `Quests/LegendaryWeapons/` — Eternally 0x01, Invincible 0x02, Muramasa 0x04 |
| 0x0004B3B6 – 0x0004B3FF | Main quest steps | `Quests/MainQuestAddresses.json` |
| 0x00048F3x – 0x00048F4x | Side quest flags | `Quests/SideQuests/*.json` |

## DRI agents

Definitions: `Backend/Memory/Definitions/Quests/DriAgents/`

| Address | Purpose | Source |
|---------|---------|--------|
| 0x0004B38C | Step 1 — talk to agent (per-agent bit) | Guilmon `0x02`, Agumon `0x01` |
| 0x0004B3B7 | Step 2 — defeat target (per-agent bit) | Guilmon `0x08`, Agumon `0x04` |
| 0x00048DD2 | Guilmon DNA — important item possession | Guilmon step 2 (persists after delivery) |
| 0x00048DB6 | Agumon DNA — important item possession | Agumon step 2 (persists after delivery) |
| 0x0004A7E0 | Guilmon step 3 — deliver DNA | Guilmon `0x08` |
| 0x0004A028 | Agumon step 3 — deliver DNA | Agumon `0x06` |

| Agent | Status | Notes |
|-------|--------|-------|
| Agumon | Mapped | `AgumonAddresses.json` |
| Guilmon | Mapped | `GuilmonAddresses.json` |

Snapshots: `Tools/MemoryScanner/Snapshots/investigation_agumon/`,
`investigation_guilmon/`.

## Inventory / items

| Address | Purpose | Source |
|---------|---------|--------|
| 0x0004858F – 0x000486FF | Consumable/equipment **quantity** table (1 byte per item ID) | GameFAQs item guide; TNT Ball compare investigation |
| 0x000485BE | TNT Ball quantity (item Val. `0x005A`) | GameFAQs `dmw3.i05`; snapshots `99-tnt-ball` / `98-tnt-ball` |
| 0x00048EC9 | Divine Barrier — current possession | DivineBarrierAddresses.txt |
| 0x00048DD2 | Guilmon DNA — important item (permanent after obtain) | DriAgents/Guilmon investigation |
| 0x00048DB6 | Agumon DNA — important item (permanent after obtain) | DriAgents/Agumon investigation |

Common items: possession may **clear on sell** — not the same as permanent
progress flags. Important items (DRI DNA) may persist after quest hand-in.

## Digimon runtime stats

| Range | Purpose | Notes |
|-------|---------|-------|
| ~0x000494xxx | Digimon stat blocks | Battle noise — level, HP, EXP changes |
| `0x00042B74` + `2 × rookieId` | Blast gauge (Int32, 0–1000, per Digimon) | Confirmed — see known-patterns.md |
| Offsets | See DigimonStatusAddresses.json | Relative to each digimon base |

Diffs here are expected after battles; usually not quest flags.

---

## Noise — discard unless specifically investigating

| Range | Purpose | Source |
|-------|---------|--------|
| 0x0004B824 – 0x0004BB00 | Encounter cache (session pointers) | Program.cs analyze-pair |
| 0x00044xxx | Coordinates / animations | MemoryScanner compare filter |
| 0x00048D6C – 0x00048D84 | Player spawn / facing on map transition | seabed-routing investigation |
| 0x0004B618 – 0x0004B653 | Entity pointer table (map load) | seabed-routing investigation |
| 0x00048DA0 | Player bits — money/spend | DivineBarrierAddresses.txt |
| ASCII runs (0x20, 0x73…) | Dialog/text buffers | Muramasa investigation |

---

## Domain → expected region

| Domain | Typical region | Diff character |
|--------|---------------|----------------|
| Quest step | 0x4B3xx | Single or multi bit flip |
| Legendary weapon | 0x4B38E | Sequential power-of-two bit |
| DRI agent | 0x4B38C, 0x4B3B7, 0x4A7xx | Per-agent bit on shared bytes |
| Map change | 0x4B3F8 (MapId) | Byte value change |
| Map / seabed routing | 0x4B3F8, 0x4B400, 0x48D78, 0x48D7A | MapId + SeabedRoute / IsSubmerged on dive; see seabed-routing-investigation.md |
| Digimon stats | ~0x494xxx | Multi-byte numeric deltas |
| Common item possession | ~0x48ECx | Often 0x00 ↔ 0x01 |
| Auction | 0x4B370, 0x4B38A | Bit flags, story window |
