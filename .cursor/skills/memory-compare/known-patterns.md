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
`rookieId` matches the numeric key in `DigimonsAddresses.json` (0–7).

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

## Combat live HP/MP / attrs / enemy stats (confirmed)

Snapshots:
- Tapirmon scout: `in-combat.bin` / `out-combat.bin`
- Mammothmon turn chain: `out-combat-west-1` → `in-combat-west-1..4` → `out-combat-west-2`

**Persistent party HP does not update mid-combat** — Kotemon `0x494BC`
(Current) stayed 1850 through all `in-west-*`, then became 1400 only on
`out-combat-west-2` (post-battle sync).

### Battle HP/MP slot table @ `0xA4470` (stride `0x20`) — HP confirmed

Layout per slot (Int16 LE) — **note: Max then Current** (inverted vs
`DigimonStatusAddresses` persistent `Current`/`Max`):

| Offset | Field | Evidence |
|--------|-------|----------|
| +0x00 | unit id / digievo token | Kotemon 386; enemy Tapirmon 206 / Mammothmon 212 |
| +0x06 | **HP Max** | Kotemon stayed 1850 while damaged; Mammothmon stayed 672 |
| +0x08 | **HP Current** | Kotemon `1850→1400`; Mammothmon `672→432→192→0` (`chain-match`) |
| +0x0A / +0x0C | **MP Max / MP Current** | Confirmed Max-then-Current (same as HP). `dinohumon-buffed`: MP `1140→1098` after skill |

| Slot | Base | Role |
|------|------|------|
| 0 | `0xA4470` | Ally party slot 0 — live HP Current @ `0xA4478` |
| 1 | `0xA4490` | Ally party slot 1 — live HP Current @ `0xA4498` |
| 2 | `0xA44B0` | Ally party slot 2 — live HP Current @ `0xA44B8` |
| 3 | `0xA44D0` | Enemy — live HP Current @ `0xA44D8` |

**Slots are fixed by party position, not by who is front/active.**
Switch snaps (`in-combat-kotemon` / `patamon` / `renamon`, same Kunemon fight):
Kotemon HP stayed at slot0 (`0xA4478`), Patamon at slot1, Renamon at slot2
across all three — only the active marker moved.

| Address | Role | Evidence |
|---------|------|----------|
| `0xA4468` | Active ally slot index (0/1/2) | `0→1→2` when switching Kotemon→Patamon→Renamon |
| `0xA4558` | Active unit id | `386→234→375` (Dinohumon→Angewomon→Taomon) |
| slot `+0x10` | **STR buff delta** | Ally: `dinohumon-buffed` `0xA4480` `0→252` (= combat STR gain). Wired as `InCombat.Strength` in `InCombatAddresses.json` |
| slot `+0x12` | **DEF buff delta** | Ally: `growlmon-normal`→`def-up` `0xA4482` `0→185` (= combat DEF `494→679`). Wired as `InCombat.Defense` |
| slot `+0x14` | **SPD buff delta** | Enemy: `hagurumon-1`→`2` `0xA44E4` `0→84` (= combat SPD `336→420`). Ally offset same; party wired as `InCombat.Speed` |

Pre-battle (`out-combat-west-1`): table zeroed. Post-battle (`out-west-2`) may
still hold last values briefly (enemy current 0, ally current 1400).

HUD mirrors (discard for logic): `0xE1408`/`0xE140C`/`0xE1410` track ally
current HP; `0xE141C`/`0xE1420`/`0xE1424` track enemy current HP.

### Strip near Blast @ `0x42B28` — NOT live current HP

| Address | Notes |
|---------|-------|
| `0x42B28` | battle-related counter/flag (1 or 3 in fights seen) |
| `0x42B34` | same enemy token as `0xA44D0` |
| `0x42B38` | enemy level (Mammothmon 23) |
| `0x42B3A` | enemy **max/initial** HP only — stayed 672 while `0xA44D8` dropped |
| `0x42B3C` | enemy MP |

### Combatant attributes / resistances @ `0xA4580` / `0xA45C0` (confirmed layout)

Two combatant blocks, **stride `0x40`**, Int16 LE. Hold **live battle**
Level + 5 attrs + 7 elemental resists (+ status-resist tail). Used by **both**
the engaged ally Digimon and the enemy — **contents swap** between the two
bases across turns/actions (not a fixed ally-only / enemy-only address).

| Offset | Field | Notes |
|--------|-------|-------|
| +0x00 | Level | Matches battle level (e.g. Mammothmon 23, Kunemon 1) |
| +0x02 | Strength | |
| +0x04 | Defense | |
| +0x06 | Spirit | |
| +0x08 | Wisdom | |
| +0x0A | Speed | |
| +0x0C | Fire | |
| +0x0E | Water | |
| +0x10 | Ice | |
| +0x12 | Wind | |
| +0x14 | Thunder | |
| +0x16 | Machine | |
| +0x18 | Dark | |
| +0x1A | status resist 0 | Matches `enemy.json` poison (Mammothmon 0, Gekomon 99) |
| +0x1C | status resist 1 | paralyze |
| +0x1E | status resist 2 | confuse |
| +0x20 | status resist 3 | sleep |
| +0x22 | status resist 4 | KO-related (enemy.json `canKO` companion value) |
| +0x24 | **species / family flags?** | Int16; see species note below |
| +0x26… | unused / zero in snaps seen | |

**No Charisma** in this block (persistent `DigimonStatusAddresses` still has Cha
at +0x32; combat block jumps Speed → Fire).

**Species candidate @ `+0x24` (flags):** same value across three `dino` enemies
(`triceramon.bin` / `tyrannomon.bin` / `tuskmon.bin` → all **512 / `0x200`**),
and consistent across more families:

| Species (`enemy.json`) | Examples | `+0x24` |
|------------------------|----------|---------|
| dino | Triceramon, Tyrannomon, Tuskmon | **512 (`0x200`)** |
| mammal | Tapirmon, Mammothmon | **1536 (`0x600`)** |
| insect | Kunemon, **Kuwagamon, Yanmamon** | **2048 (`0x800`)** |
| fish | Gekomon, **Shellmon** | **2560 (`0xA00`)** |
| dragon | **Seadramon** | **2816 (`0xB00`)** |
| bird | **Kiwimon** (`kabuterimon-3`) | **1792 (`0x700`)** |
| plant | **Vegiemon** (`kabuterimon-2`) | **2304 (`0x900`)** |
| (ally Digimon) | see note below | **not a single constant** |

**Ally Digimon `+0x24` is species too — not a fixed “player” marker.**  
Dinohumon (front) repeatedly showed **256 (`0x100`)**. Kabuterimon front
(`kabuterimon-1..3.bin`, digievo token `19`, Renamon line) showed **2048
(`0x800`)** in all three snaps — same code as wild **insect** enemies — while
the enemy half was Tapirmon mammal / Vegiemon plant / Kiwimon bird. So ally
flags track the Digimon’s family (at least insect), and `0x100` is specific
to whatever Dinohumon/that form maps to, not “any ally”.

Snaps `shellmon` / `seadramon` / `kuwagamon` / `yanmamon.bin` (2026-08-05): insects
match; Shellmon matches fish. Seadramon is **`dragon` in enemy.json** (not fish)
and introduced **`0xB00`**. Earlier `N × 0x200` guess is incomplete. Need
evil/machine/ghoul/rare/etc. before treating as a full enum. Status-resist tails
still differ per enemy; do not confuse with species.

| Base | Role |
|------|------|
| `0xA4580` | Combatant A (ally **or** enemy depending on snap) |
| `0xA45C0` | Combatant B (the other side) |

**Evidence (enemy.json exact match):**

| Snap | `0xA4580` | `0xA45C0` |
|------|-----------|----------|
| `in-combat-west-1` | Ally Dinohumon (lv22, STR 674…) | Mammothmon 350/280/260/280/190 |
| `in-combat-west-2` | Mammothmon | Ally Dinohumon |
| `in-combat-west-3/4` | Ally Dinohumon | Mammothmon |
| `in-combat.bin` | Ally | Tapirmon 42/50/40/50/48 |
| `in-combat-kotemon` | Ally | Kunemon 50/42/40/50/48 |
| `in-combat-patamon` | Kunemon | Ally Angewomon (STR 329…) |
| `in-combat-renamon` | Ally Taomon (same 329… as Patamon front) | Kunemon |
| `in-combat-guilmon-gekomon-normal` | Ally Guilmon line | Gekomon 130/130/138/162/104 |

**Identification (for future readers):** do **not** assume `0xA4580` = active
unit (`0xA4558`). Counterexamples: `west-3/4` have `activeId` = enemy while
ally still sits at `0xA4580`; `patamon` has ally active but Kunemon at
`0xA4580`. Reliable approach: match one block’s attrs/resists to
`enemy.json` (or the enemy catalog copy below) using enemy slot id
`0xA44D0`; the other block is the engaged ally.

**Not attacker/defender slots either** — tested with named turn snaps
(`dinohumon|goburimon-attacking-*`, `stingmon|tuskmon-attacking-*`).
Filename = whose action turn it was:

| Snap | Attacker (name) | `0xA4580` | `0xA45C0` |
|------|-----------------|-----------|-----------|
| `dinohumon-attacking-1` | Dinohumon | Dinohumon | Goburimon(Red) |
| `goburimon-attacking-1` | Goburimon | Goburimon(Red) | Dinohumon |
| `dinohumon-attacking-2` | Dinohumon | Goburimon(Red) | Dinohumon |
| `stingmon-attacking-1` | Stingmon | Tuskmon | Stingmon |
| `stingmon-attacking-2` | Stingmon | Stingmon | Tuskmon |
| `stingmon-attacking-3` | Stingmon | Tuskmon | Stingmon |

Same attacker label appears with **both** arrangements → neither base is
“always attacker” or “always defender”. Goburimon in those snaps matches
`enemy.json` **Goburimon(Red)** (`405,270,212,230,216`), not base Goburimon.

**Party coverage:** only the **engaged** ally appears in this pair. Bench
party members keep HP in `0xA4470+n×0x20` but have **no** per-slot combat
attr block in these snaps — switching front Digimon replaces the ally half
of the pair (`kotemon`/`patamon`/`renamon`).

**vs persistent DigimonStatus (~`0x4949C` for Kotemon in west snaps):** combat
attrs ≠ persistent (e.g. STR 674 vs 352). Combat values look like
digievolution + gear baked in; persistent block does not mid-fight update
(same story as HP).

**Attr buff/debuff:** **confirmed for Strength** — snaps `dinohumon-normal` /
`dinohumon-buffed` (vs Tapirmon `206`):

| Field | Normal (ally half) | Buffed (ally half) |
|-------|--------------------|--------------------|
| Strength | **674** | **926** (+252) |
| DEF / SPI / WIS / SPD | unchanged | unchanged |
| Elemental + status resists | unchanged | unchanged |
| Enemy half (Tapirmon) | unchanged | unchanged |
| Persistent DigimonStatus (~`0x4949C`) | STR 352 etc. | **identical** (buff did not write through) |

Blocks still **swapped** bases between the two snaps (ally was at `0xA4580`
then at `0xA45C0`); identify by fingerprint, then compare. Ally battle slot
`+0x10` (`0xA4480`) went `0→252` (same delta) — STR buff accumulator on the
fixed HP/MP slot row. MP Current spent `1140→1098` (−42) on the skill.

**SPD buff — confirmed (enemy):** `hagurumon-1` → `hagurumon-2` (Velocidade
Acima). Hagurumon fingerprint L23 / STR 420 / Machine 270:

| Field | Before | After |
|-------|--------|-------|
| Combat SPD (matched across swap) | **336** | **420** (+84) |
| Other combat attrs/resists | unchanged | unchanged |
| Ally combat half | unchanged | unchanged |
| Enemy slot `+0x14` (`0xA44E4`) | **0** | **84** |
| Enemy slot `+0x10` (STR delta) | 0 | 0 |
| Enemy MP Current (`+0x0C`) | 336 | 288 (−48 skill cost) |

So live SPD is rewritten in the swappable combat block **and** mirrored as a
delta on the **fixed** enemy slot at `+0x14`.

**DEF buff — confirmed (ally):** `growlmon-normal` → `growlmon-def-up`.
Growlmon fingerprint L99 / STR 659 / SPD 632:

| Field | Before | After |
|-------|--------|-------|
| Combat DEF (matched across swap) | **494** | **679** (+185) |
| Other combat attrs/resists | unchanged | unchanged |
| Enemy combat half | unchanged | unchanged |
| Ally0 slot `+0x12` (`0xA4482`) | **0** | **185** |
| Ally0 `+0x10` / `+0x14` (STR/SPD deltas) | 0 | 0 |
| Ally0 MP Current (`+0x0C`) | 5566 | 5524 (−42 skill cost) |

**Fixed-slot attr buff deltas (confirmed trio), Int16 LE per battle slot** —
wired on ally party slots via `Parties/InCombatAddresses.json` (`Strength` /
`Defense` / `Speed` → Digimon `InCombat`):

| Offset | Field |
|--------|-------|
| `+0x10` | STR delta |
| `+0x12` | DEF delta |
| `+0x14` | SPD delta |

Same offsets on enemy `0xA44D0` — wired as top-level `State.Battle.Enemy` via
`BattleAddresses.json` (`BattleChanged` / InitialState).

### Field skill (element strengthen / weaken) — **not** in combat attr Int16s

Pairs:

| Pair | Skill (claimed effect) |
|------|------------------------|
| `taomon-without-field` / `taomon-with-field` | strengthen Thunder, weaken Machine |
| `sakuyamon-without-field` / `sakuyamon-with-field` | strengthen Ice, weaken Wind |
| `bkwargreymon-without-field` / `bkwargreymon-with-field` | strengthen Fire, weaken Ice |
| `MaloMyotismon-without-field` / `MaloMyotismon-with-field` | claimed strengthen Dark, weaken Thunder |

Match combatants by fingerprint across the `0xA4580`/`0xA45C0` swap
(pairs swap bases again).

| Region | Result |
|--------|--------|
| Combatant attrs/resists (`0xA4580` / `0xA45C0`) | **No change** to claimed elements (or any mapped attr/resist) for the same combatant |
| Fixed slot STR delta `+0x10` | Still **0** (unlike Strength buff) |
| Ally slot MP | Skill cost (−100) |
| Enemy slot `+0x04` | often `0→1` (turn/state noise) |

Field effects do **not** rewrite live elemental resist Int16s. Prefer a
**global field state** near the battle table.

**Candidates** (zero → nonzero while field-active cluster present; near `0xA4470`):

| Address | Taomon (Thunder) | Sakuya (Ice) | BKW (Fire) | Malo (Dark?) | Reading |
|---------|------------------|--------------|------------|--------------|---------|
| `0xA4414` | 16 | 16 | 16 | 16 | Shared constant — **not** an element bitmask |
| `0xA4418` | 221 | 220 | 221 | 220 | Near-identical — timer / instance noise? |
| `0xA441C` | **5** | **4** | **4** | **4** | Only **two** observed ids. Thunder alone is 5; Fire/Ice/Dark all write **4** |
| `0xA442A` | 1 | 1 | 1 | 1 | Active flag |
| `0xA4424` | 1 | 0 | 0 | 0 | Optional / timing |
| `0xA4530` | 6 | 6 | 6 | 6 | Shared field-category / mode id? |
| `0xA4532` | **102** | **127** | **127** | **127** | Tracks with `A441C` (5→102, 4→127) — duration / potency class? |

Healthy “field on” snaps set **both** the `0xA4414…A442A` cluster **and**
`0xA4530`/`0xA4532` together.

**`0xA441C` is not a per-element field enum.** Fire↑Ice↓ (BKW) and Ice↑Wind↓
(Sakuya) produce **byte-identical** field clusters (`4` / `127`). Either:

1. persisted RAM only stores a coarse mode (4 vs 5), and the actual
   strengthen/weaken pair is looked up from the technique id at cast/damage
   time (not kept in this cluster), or
2. several Campo skills incorrectly share one effect row (see Dark below).

No other Int16 near `0xA4400–0xA4560` discriminates Fire vs Ice. One-off
`0xA3868` `0→2` on BKW only (Ice index?) — **not** written by Sakuya/Taomon;
treat as fight-specific noise until reproduced.

**MaloMyotismon / Campo Sombrio:** structurally clean (full cluster + MP −100) but
matches Ice/Fire (`A441C=4`). Player also sees **“campo desapareceu”** after
the skill (unlike Taomon/Sakuya/BKW). Likely bug — do **not** map `4` to Dark.

**Still open:** Water / Wind / Machine fields; second discriminator outside the
battle neighborhood; damage-time technique id.

### Enemy catalog attr copy (variable address; id → attrs +0x0E)

Besides the swappable pair, enemy base attrs+resists also appear in a
**heap-like catalog** record: `enemyId` (Int16) then attrs start **`+0x0E`**
later (no Level prefix — starts at Strength). Address is **not fixed**:

| Fight | enemyId @ | attrs @ | Match |
|-------|-----------|---------|-------|
| Mammothmon west | `0xB97A2` | `0xB97B0` | stable across `in-west-1..4` |
| Kunemon | `0x15A5EC` | `0x15A5FA` | `in-combat-kotemon` |
| Tapirmon | `0xFBDAE` | `0xFBDBC` | `in-combat.bin` |
| Gekomon | `0x10CC6E` | `0x10CC7C` | `guilmon-gekomon-normal` |
| enemy 166 | `0xC3800` | `0xC380E` | `guilmon-normal` |

`0xB97B0` alone is **not** a universal enemy-attrs absolute — only correct
when that catalog slot is bound (garbage in Kunemon/Tapirmon snaps). Prefer
the swappable blocks for combat UI; catalog is a good **audit** / identity
helper.

### Status / debuff byte (suspected → stronger)

Ally battle slot offset **`+0x1C`** (byte). Guilmon party slot 0 → `0xA448C`.

| Snap pair | Normal | Debuff | Value @ `+0x1C` |
|-----------|--------|--------|-----------------|
| `guilmon-normal` / `guilmon-poison` | 0 | Poison | **`0x01`** |
| `guilmon-gekomon-normal` / `guilmon-gekomon-confuse` | 0 | Confuse | **`0x04`** |

Only the active Guilmon slot changed; other ally slots stayed 0.
Looks like a **bitfield** (poison bit0, confuse bit2) or enum powers-of-two.
Secondary noise on confuse snap: `+0x1F` `0x00→0x50` (unknown — timer?).
Wired as nested Digimon `InCombat.Condition` (`byte`) via `InCombatAddresses` / `DigimonLoader`.

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
