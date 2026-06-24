# RAM Regions

PS1 save RAM: **2 MiB** snapshot. MemoryScanner `compare` focuses on
**0x00048000 – 0x0004D000** by default.

This file is **incrementally maintained** by the memory-compare skill after
each investigation. Append new entries; do not remove without strong evidence.

---

## Player

| Address | Field | Source |
|---------|-------|--------|
| 0x00048D88 | Player name buffer | PlayerAddresses.json |
| 0x00048DA0 | Player bits (money etc.) | PlayerAddresses.json — **volatile** |
| 0x0004B3F8 | MapId | PlayerAddresses.json — changes on map transition |

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
| 0x00048EC9 | Divine Barrier — current possession | DivineBarrierAddresses.txt |
| 0x00048DD2 | Guilmon DNA — important item (permanent after obtain) | DriAgents/Guilmon investigation |
| 0x00048DB6 | Agumon DNA — important item (permanent after obtain) | DriAgents/Agumon investigation |

Common items: possession may **clear on sell** — not the same as permanent
progress flags. Important items (DRI DNA) may persist after quest hand-in.

## Digimon runtime stats

| Range | Purpose | Notes |
|-------|---------|-------|
| ~0x000494xxx | Digimon stat blocks | Battle noise — level, HP, EXP changes |
| Offsets | See DigimonStatusAddresses.json | Relative to each digimon base |

Diffs here are expected after battles; usually not quest flags.

---

## Noise — discard unless specifically investigating

| Range | Purpose | Source |
|-------|---------|--------|
| 0x0004B824 – 0x0004BB00 | Encounter cache (session pointers) | Program.cs analyze-pair |
| 0x00044xxx | Coordinates / animations | MemoryScanner compare filter |
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
| Digimon stats | ~0x494xxx | Multi-byte numeric deltas |
| Common item possession | ~0x48ECx | Often 0x00 ↔ 0x01 |
| Auction | 0x4B370, 0x4B38A | Bit flags, story window |
