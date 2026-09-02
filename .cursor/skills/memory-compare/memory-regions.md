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
| 0x0004B3F8 | MapId | PlayerAddresses.json — changes on map transition; **`0x0700`** = in card battle screen |
| 0x0004B400 | PreviousMapId | PlayerAddresses.json — map just left on each transition; during card battle holds pre-battle world MapId (e.g. `0x0200` Genji, `0x021D` Natsumi) |
| 0x0004B404 | Card battle opponent id (Int32) | **integrated** — [`CardBattleAddresses.json`](Backend/Memory/Definitions/CardBattleAddresses.json) `OpponentId`; Genji `1`, Nacky `3`, Wong `5`, Steve `7`, Gloria `9`, Natsumi `11`; `0` outside card battle |
| 0x0004B410 | MapId mirror | seabed-routing investigation — tracks current MapId |
| 0x0004B420 | Card battle context id (Int32) | card-battle-natsumi — `0x23` (35) in card battle vs `0x03` in digimon battle; Genji card battle `0` |
| 0x00048D78 | SeabedRoute | PlayerAddresses.json — seabed corridor / dock pair; **Mobius: constant `0x01`** (not cell id) |
| 0x00048D7A | MapVariant | PlayerAddresses.json — seabed: `0x01` underwater; **Mobius: cell-pair `0x01`–`0x08`** (with MapId `0258`/`0259`) |

See also **Map / location** for seabed routing fields (including investigation-only mirrors).

## Map / location

| Address | Field | Source |
|---------|-------|--------|
| 0x0004B3F8 | Current MapId | PlayerAddresses.json — **`0x0700`** = card battle screen |
| 0x0004B400 | PreviousMapId (rolling) | PlayerAddresses.json — map just left on each transition; during card battle = world map before `0x0700` |
| 0x0004B404 | Card battle opponent id (Int32) | **integrated** — [`CardBattleAddresses.json`](Backend/Memory/Definitions/CardBattleAddresses.json) `OpponentId`; Genji `1`, Nacky `3`, Wong `5`, Steve `7`, Gloria `9`, Natsumi `11`; `0` outside card battle |
| 0x0004B410 | MapId mirror | seabed-routing investigation — tracks current MapId |
| 0x0004B420 | Card battle context id (Int32) | card-battle snapshots — Natsumi `0x23` in card battle; role TBD |
| 0x00048D68 | PreviousMapId mirror (player block) | seabed-routing investigation — mirrors `0x4B400` |
| 0x00048D78 | SeabedRoute (corridor / dock pair) | PlayerAddresses.json — set on dive, persists underwater; same from either entry; Mobius always `0x01` |
| 0x00048D7A | MapVariant / Mobius cell-pair | PlayerAddresses.json — seabed submerged `0`/`1`; Mobius pair index `1`–`8` (see mobius-desert-investigation.md) |
| 0x0000E2E0 | Player facing / direction (0–3) | map-subzones — discarded as area index; all forward-facing snaps = 1 |
| 0x0004DE30 | Zone resource pointer (suspected) | map-subzones — PSX `0x801Fxxxx`; companions `0x4DE34`/`38` fixed |
| 0x00048D82 | Room / sub-area byte (suspected) | map-subzones — volatile; not stable named-area enum |
| 0x00048D6D | Player tile X (u16) | map-subzones — use with Makisha grids |
| 0x00048D71 | Player tile Y (u16) | map-subzones — use with Makisha grids |
| 0x00048D6C – 0x00048D84 | Spawn / transition block (i32 coords noisy) | seabed-routing investigation |

Details: [seabed-routing-investigation.md](seabed-routing-investigation.md),
[mobius-desert-investigation.md](mobius-desert-investigation.md),
[map-subzones-investigation.md](map-subzones-investigation.md).

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
| 0x0004B38E | Legendary weapons | `Quests/LegendaryWeapons/` — Eternally 0x01, Invincible 0x02, Muramasa 0x04, Super Nova 0x08, Punishment 0x10 |
| 0x0004B3B6 – 0x0004B3FF | Main quest steps | `Quests/MainQuestAddresses.json` |
| 0x0004B3DF bit `0x20` | Genji tutorial digimon battle done (suspected) | genji-before/after-first; MQ step 44 uses same byte `0x10` |
| 0x00048F3x – 0x00048F4x | Side quest flags | `Quests/SideQuests/*.json` |

## DRI agents

Definitions: `Backend/Memory/Definitions/Quests/DriAgents/`

| Address | Purpose | Source |
|---------|---------|--------|
| 0x0004B38C | Step 1 — talk to agent (per-agent bit) | Agumon `0x01`, Guilmon `0x02`, Patamon `0x04`, Renamon `0x08`, Kotemon `0x10`, Kumamon `0x20`, Monmon `0x40`, Veemon `0x80` |
| 0x0004B3B7 | Step 2 — defeat target | Agumon `0x04`, Guilmon `0x08`, Patamon `0x10`, Renamon `0x20`, Kotemon `0x40`, Monmon `0x80` |
| 0x0004B3B8 | Step 2 — defeat target | Kumamon `0x01`, Veemon `0x02` |
| 0x00048DD2 | Guilmon DNA — important item possession | Guilmon step 2 (persists after delivery) |
| 0x00048DB6 | Agumon DNA — important item possession | Agumon step 2 (persists after delivery) |
| 0x00048DC3 | Kotemon DNA — important item possession | Kotemon step 2 |
| 0x00048DD3 | Veemon DNA — important item possession | Veemon step 2 |
| 0x00048DD6 | Renamon DNA — important item possession | Renamon step 2 |
| 0x00048DD7 | Patamon DNA — important item possession | Patamon step 2 |
| 0x00048F3B | Kumamon DNA — important item possession | Kumamon step 2 |
| 0x00048F18 | Monmon DNA — important item possession | Monmon step 2 |
| 0x0004A7E0 | Guilmon step 3 — deliver DNA | Guilmon `0x08` |
| 0x0004A028 | Agumon step 3 — deliver DNA | Agumon `0x06` |
| 0x0004A404 | Veemon step 3 — deliver DNA | Veemon `0x07` |
| 0x00049494 | Kotemon step 3 — deliver DNA | Kotemon `0x03` |
| 0x00049870 | Kumamon step 3 — deliver DNA | Kumamon `0x04` |
| 0x00049C4C | Monmon step 3 — deliver DNA | Monmon `0x05` |
| 0x0004ABBC | Renamon step 3 — deliver DNA | Renamon `0x09` |
| 0x0004AF98 | Patamon step 3 — deliver DNA | Patamon `0x0A` |

| Agent | Status | Notes |
|-------|--------|-------|
| Agumon | Mapped | `DriAgentAgumonAddresses.json` |
| Guilmon | Mapped | `DriAgentGuilmonAddresses.json` |
| Veemon | Mapped | `DriAgentVeemonAddresses.json` |
| Kumamon | Mapped | `DriAgentKumamonAddresses.json` |
| Monmon | Mapped | `DriAgentMonmonAddresses.json` |
| Kotemon | Mapped | `DriAgentKotemonAddresses.json` |
| Renamon | Mapped | `DriAgentRenamonAddresses.json` |
| Patamon | Mapped | `DriAgentPatamonAddresses.json` |

Snapshots: `Tools/MemoryScanner/Snapshots/investigation_agumon/`,
`investigation_guilmon/`, `kumamon_*.bin`, `monmon_*.bin`, `kotemon_*.bin`,
`renamon_*.bin`, `patamon_*.bin`.

## Inventory / items

| Address | Purpose | Source |
|---------|---------|--------|
| 0x0004858F – 0x000486FF | Consumable/equipment **quantity** table (1 byte per item ID) | GameFAQs item guide; TNT Ball compare investigation |
| 0x000485BE | TNT Ball quantity (item Val. `0x005A`) | GameFAQs `dmw3.i05`; snapshots `99-tnt-ball` / `98-tnt-ball` |
| 0x00048EC9 | Divine Barrier — current possession | DivineBarrierAddresses.txt |
| 0x00048DD2 | Guilmon DNA — important item (permanent after obtain) | DriAgents/Guilmon investigation |
| 0x00048DB6 | Agumon DNA — important item (permanent after obtain) | DriAgents/Agumon investigation |
| 0x00048DC3 | Kotemon DNA — important item (permanent after obtain) | DriAgents/Kotemon investigation |
| 0x00048DD3 | Veemon DNA — important item (permanent after obtain) | DriAgents/Veemon |
| 0x00048DD6 | Renamon DNA — important item (permanent after obtain) | DriAgents/Renamon investigation |
| 0x00048F3B | Kumamon DNA — important item (permanent after obtain) | DriAgents/Kumamon investigation |
| 0x00048F18 | Monmon DNA — important item (permanent after obtain) | DriAgents/Monmon investigation |

Common items: possession may **clear on sell** — not the same as permanent
progress flags. Important items (DRI DNA) may persist after quest hand-in.

## Digimon runtime stats

| Range | Purpose | Notes |
|-------|---------|-------|
| ~0x000494xxx | Digimon stat blocks (persistent / post-battle) | HP Current syncs **after** combat only — see DigimonStatusAddresses.json |
| `0x00042B74` + `2 × rookieId` | Blast gauge (Int16, 0–1000, per Digimon) | Confirmed — see known-patterns.md; updates in combat |
| `0x00042B28` – `0x00042B3E` | Enemy battle strip (near Blast) | `0x42B28` counter; **`0x42B2C` GroupId** (Int16, wired in `EnemyAddresses.json` → `Enemy.GroupId`); `0x42B34+` token/level/max HP+MP — does **not** track current HP damage |
| `0x00042B6C` | Enemy **drop item Val** (Int16, live) | **Confirmed** — see known-patterns.md “Enemy drops (variable)”. Same Digimon can show different Vals by map or roll; static single `dropId` is incomplete. |
| `0x000A4470` + `n × 0x20` | Battle HP/MP slot table | **Confirmed** live HP/MP + Condition @ +0x1C; attr buff deltas STR/DEF/SPD @ +0x10/+0x12/+0x14 — allies in `Parties/InBattleAddresses.json`; enemy slots start `0xA44D0` (also `0xA44F0`, `0xA4510` for NPC multi-enemy parties — each has own memoryId+HP) in `EnemyAddresses.json` |
| `0x000A4468` | Active ally slot index | 0/1/2 — switches with front Digimon (`in-combat-kotemon/patamon/renamon`) |
| `0x000A4558` | Active unit id | Tracks front digievo/token while HP stays in fixed slots |
| `0x000A4580` / `0x000A45C0` | Combatant attr/resist blocks (stride `0x40`) | **Confirmed** layout: Level, STR/DEF/SPI/WIS/SPD, 7 elemental resists, status-resist tail, **species @ +0x24**. Ally↔enemy **swap** which base holds whom — identify by matching enemy.json / slot id `0xA44D0`. No Charisma. Not per party slot (engaged pair only). Field skills do **not** rewrite these resists. Species code table: known-patterns.md |
| `0x000A4530` | Active battle **field** id (byte) | **Confirmed** + **integrated** in `EnemyAddresses.json` as `Field` on `Battle` — `0` none; `2` Fire … `8` Dark |
| `0x000A4532` | Field companion (potency/timer class?) | `0` none; `0x40` while item field active (same for all elements in that series) |
| `0x000A4414`…`0xA442A` | Field support cluster | Lights when field active; **not** per-element SSOT — prefer `0xA4530` |
| Enemy catalog (variable) | Enemy base attrs copy | `enemyId` then attrs at **+0x0E** (e.g. Mammothmon `0xB97A2`→`0xB97B0`); address moves per fight — audit helper, not a fixed absolute |
| `0x000E1408` / `0x000E141C` | HUD HP mirrors | Track current HP; discard for authoritative state |
| Offsets | See DigimonStatusAddresses.json | Relative to each digimon base (persistent; mid-combat attrs differ from `0xA4580` pair) |

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
| Map / seabed routing | 0x4B3F8, 0x4B400, 0x48D78, 0x48D7A | MapId + PreviousMapId + SeabedRoute / MapVariant on dive; see seabed-routing-investigation.md |
| Map / Mobius Desert | 0x4B3F8, 0x48D7A (0x48D78=0x01) | MapId 0258/0259 + cell-pair 1–8 at D7A; see mobius-desert-investigation.md |
| Map subzones / encounters | 0x0E2E0, 0x4DE30, 0x48D82 + Makisha grids | Same MapId, different encounter pools; see map-subzones-investigation.md |
| Digimon stats | ~0x494xxx | Multi-byte numeric deltas |
| Common item possession | ~0x48ECx | Often 0x00 ↔ 0x01 |
| Auction | 0x4B370, 0x4B38A | Bit flags, story window |
| NPC digimon battle | `0x4B3DF` (`#0`), `0x4B39A+` (`#1+`) | **integrated** — [`NpcAddresses.json`](Backend/Memory/Definitions/NpcAddresses.json); BattledTamer bitfield — see known-patterns.md |
| NPC card battle | `0x48E0B`, `0x48F19` (shared counters) | Not per-NPC flags — win counters across tamer groups; see known-patterns.md; discard transient `0x48ABC` |
| Duel Island gauntlet (round progress) | `0x4B3B2` bit `0x80`, `0x4B3B3` bits `0x01`–`0x08` | NPC gates for current run; reset after final — duel-island snaps 2026-08-30 |
| Duel Island booster rewards | `0x48F1C`–`0x48F1F`, `0x48F36`, `0x48F37` | Booster qty `0→1` on win (RA: 05a–08a, R-01, R-02) — **not** quest steps |
| A.o.A Attacker battle (suspected) | `0x4B3E5` bit `0x08` | before/after-aoa-attacker — only sticky single-bit add near MQ tail; `0x4B3CA` unchanged (MQ step 61, not battle) |
| Digimon battle session RAM | `0x4B610`–`0x4B660` | before/after-aoa-attacker — large churn during fight; discard |
