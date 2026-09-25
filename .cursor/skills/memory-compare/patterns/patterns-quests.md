# Known Patterns — Quests

Quest-like progress flags: main quest, side quests, legendary weapons, DRI agents, Duel Island.

Maintained by the memory-compare skill (retrofeed). Mark `(confirmed)` or `(suspected)`. Append; do not delete without evidence.

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

## Duel Island card gauntlet (confirmed 2026-08-30)

Snapshots: `before/after-duel-island-opponent-{1..5,final}.bin`.

Unlike overworld card tamers (no per-NPC sticky flag), the Duel Island
gauntlet **does** persist round progress. Booster qty bytes in the same
compares are **rewards**, not steps (same trap as `0x48F19` / Natsumi).

### Booster rewards on win (not quest progress)

RA addresses — qty `0→1` on win:

| Opponent | Booster | Address |
|----------|---------|---------|
| 1 | 05a | `0x48F1C` |
| 2 | 06a | `0x48F1D` |
| 3 | 07a | `0x48F1E` |
| 4 | 08a | `0x48F1F` |
| 5 | R-01 | `0x48F36` |
| Final | R-02 | `0x48F37` |

Opp2 snap also shows `0x48F1C` `1→0` while `0x48F1D` `0→1` — booster band,
not gate progress.

### Round state (NPC path blocking; resets after final)

| Opponent | Address | BitMask | Evidence |
|----------|---------|---------|----------|
| 1 | `0x4B3B2` | `0x80` | `0x4F→0xCF` after opp1; clears `0xCF→0x4F` after final |
| 2 | `0x4B3B3` | `0x01` | `0x00→0x01` |
| 3 | `0x4B3B3` | `0x02` | `0x01→0x03` |
| 4 | `0x4B3B3` | `0x04` | `0x03→0x07` |
| 5 | `0x4B3B3` | `0x08` | `0x07→0x0F` |
| Final | — | — | `0x4B3B3` `0x0F→0x00` + `0x4B3B2` loses `0x80` (new round) |

Survives map leave mid-run (user-tested). `0x4B3B2` shares byte with MQ step 26
(`0x04`) — different bits.

First-run trophy: `0x48DC2` (Asuka Trophy, important item — not a booster).
Sun Trophy: `0x48DC4` (important item — RetroAchievements; pending manual validation).

**Sun Trophy rematch (confirmed 2026-08-31):** snapshots
`sun-trophy-before/after-divermon-{1..3}.bin` — post-Asuka second round reuses
**identical** round-state bytes (`0x4B3B2`/`0x4B3B3`) and booster band
(`0x48F1C`–`0x48F1E` qty `0→1` per win). `0x48DC2` stays `1` throughout;
Sun Trophy address still needs `before/after-final` snap.

**Backend integrated (2026-08-30):** `Quests/DuelIsland/AsukaTrophyAddresses.json`
— `Id` `asukaTrophy`, 6 steps (round flags + trophy), normalize in
`DuelIslandAssembler`.

**Backend integrated (2026-08-31):** `Quests/DuelIsland/SunTrophyAddresses.json`
— `Id` `sunTrophy`, requisite `asukaTrophy` @ `0x48DC2`, same round flags,
trophy @ `0x48DC4`.

Discard: encounter cache `0x4B824+`, entity table `0x4B618+`, spawn block
`0x48D6C–0x48D84`.

