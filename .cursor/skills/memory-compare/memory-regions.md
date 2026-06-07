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

| Agent | Status | Notes |
|-------|--------|-------|
| Agumon | Not mapped | 3 steps — `AgumonAddresses.json` |
| Guilmon | Not mapped | 3 steps — `GuilmonAddresses.json` |

Snapshots: `Tools/MemoryScanner/Snapshots/investigation_agumon/`,
`investigation_guilmon/`.

## Inventory / items

| Address | Purpose | Source |
|---------|---------|--------|
| 0x00048EC9 | Divine Barrier — current possession | DivineBarrierAddresses.txt |

Common items: possession may **clear on sell** — not the same as permanent
progress flags.

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
| DRI agent | TBD | TBD |
| Map change | 0x4B3F8 (MapId) | Byte value change |
| Digimon stats | ~0x494xxx | Multi-byte numeric deltas |
| Common item possession | ~0x48ECx | Often 0x00 ↔ 0x01 |
| Auction | 0x4B370, 0x4B38A | Bit flags, story window |
