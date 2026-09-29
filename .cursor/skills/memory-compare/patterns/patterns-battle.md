# Known Patterns — Digimon Battle

Blast gauge, combat HP/MP slots, combatant attributes/resistances, field, drops, status.

Maintained by the memory-compare skill (retrofeed). Mark `(confirmed)` or `(suspected)`. Append; do not delete without evidence.

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
| `0xA446C` | Active **enemy** slot index (0/1/2) | **Confirmed** Gordon tamer snaps: `gordon-crabmon` `0`, `gordon-gizamon` `1`, `gordon-suposed-to-be-gekomon` `2` — mirrors ally `@0xA4468`; required when multiple enemy slots alive and `0xA4558` holds ally id |
| `0xA4558` | Active unit id → **focused / target unit** | `386→234→375` (Dinohumon→Angewomon→Taomon); Gordon: `386→132→386` — can be **ally** memoryId while enemy front changes. Phase snaps (2026-09-27, Kunemon fight): stays `386` through Dinohumon self-buff and Kunemon attack on Dinohumon; flips to `32` only at `dinohumon-animacao-ataque-10` (animation start, **not** at declaration `-9`); stays `32` after KO/victory. = unit on camera/receiving the action, **not** the actor, stale between actions |
| slot `+0x10` | **STR buff delta** | Ally: `dinohumon-buffed` `0xA4480` `0→252` (= combat STR gain). Wired as `InBattle.Strength` in `InBattleAddresses.json` |
| slot `+0x12` | **DEF buff delta** | Ally: `growlmon-normal`→`def-up` `0xA4482` `0→185` (= combat DEF `494→679`). Wired as `InBattle.Defense` |
| slot `+0x14` | **SPD buff delta** | Enemy: `hagurumon-1`→`2` `0xA44E4` `0→84` (= combat SPD `336→420`). Ally offset same; party wired as `InBattle.Speed` |

**Ally slot `+0x00` id (2026-09-27, `on-wild-digimon-battle-1..7`, Kotemon/Dinohumon
vs Kunemon, no switch):** slot0 `0xA4470` = `386` = Kotemon `ActiveDigievolution`
(persistent `0x4949C-4`); Agumon without digievo showed `3` (rookie index). RA note
"Current Digivolution @ 0xA4470" = only slot0's id. Slots 1/2 held `31` / `373`
while Patamon/Renamon actives were `56` / `19` — ids not in digievolution.json nor
enemy.json. **Confirmed stale** (`dinohumon` / `digitamamon` / `kabuterimon` switch
snaps): slot id is written only when the member **enters the field** — slot1
`31→56` on switch to Patamon, slot2 `373→19` on switch to Renamon; ids stay after
switching away. `0xA4468` `0→1→2` and `0xA4558` `386→56→19` tracked the switch.
Do **not** use slot `+0x00` as active digievo for members that have not fought yet;
use persistent `ActiveDigievolution`. `0xA4558` went `386` (#1–4) → `32` Kunemon
(#5–7, enemy turn).

Pre-battle (`out-combat-west-1`): table zeroed. Post-battle (`out-west-2`) may
still hold last values briefly (enemy current 0, ally current 1400).

HUD mirrors (discard for logic): `0xE1408`/`0xE140C`/`0xE1410` track ally
current HP; `0xE141C`/`0xE1420`/`0xE1424` track enemy current HP.

### Strip near Blast @ `0x42B28` — NOT live current HP

| Address | Notes |
|---------|-------|
| `0x42B28` | battle-related counter/flag (1 Natsumi, 20 Genji, varies wild) |
| `0x42B2C` | **`GroupId`** (Int16 LE) — encounter group template id (site: "Group F-201"). Natsumi **201**, Genji **272**, wild 39–43. Stable across multi-enemy NPC fights. Wired in `EnemyAddresses.json` → `Enemy.GroupId` / `DigimonBattleChanged` |
| `0x42B34` | same enemy token as `0xA44D0` |
| `0x42B38` | enemy level (Mammothmon 23) |
| `0x42B3A` | enemy **max/initial** HP only — stayed 672 while `0xA44D8` dropped |
| `0x42B3C` | enemy MP |
| `0x42B6C` | **drop item Val** (Int16, live battle) — see **Enemy drops (variable)** below |

### Enemy drops (variable) — open design (2026-08-09)

Combat RAM exposes the **current** drop as item Val @ `0x42B6C` (GameFAQs
item Val; e.g. Booster 1b=`0x0177`, 2b=`0x0178`). Confirmed with Cardmon fish
(`memoryId` `458`): east vs south snaps differed **only** at this Int16 for the
booster pair.

**Two cases the static `dropId: string` model does not cover:**

1. **Same enemy, different locations → different drops**  
   Example: Cardmon fishing — Central Park / east → Booster 1b; Bulk Bridge /
   south → Booster 2b. Same `memoryId` and combat stats; drop depends on where
   the encounter was rolled.

2. **Same enemy, same location → more than one possible drop**  
   Observed: one enemy on one map can still yield **up to two** distinct drops
   across encounters (not location-keyed). Exact pool / rates TBD.

**Interim frontend data (pending product decision):** for enemies with multiple
possible drops, `enemy.json` may store a **list** on the drop field (no
location binding yet). That will likely break `EnemyRaw` / UI until those
layers accept `string | string[]` (or a richer shape). Do not treat the list as
final SSOT for location-aware drops — revisit when wiring bestiary UI and/or
live `0x42B6C` reads.

**Sources:** GameFAQs Patch Code Generation Guide (item Val); Card Battle FAQ
(Cardmon booster-by-sector notes).

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
| +0x24 | Species / family | Int16 `N×0x100` — table in species section below |
| +0x26… | unused / zero in snaps seen | |

**Only computed-attrs source (confirmed, 2026-09-27).** Snaps
`dinohumon-644-on-{battle,central-park,menu}` (Kotemon slot 1, base STR 352,
menu STR 644 = base + equips, 674 = + Dinohumon bonus): `0xA4580` holds
STR **674** in battle (digievo bonus **is** applied because the engaged unit is
the digievo) and is all-zero out of battle. Out of battle there is **no**
RAM copy of 644/674 near `0x4949C` nor anywhere stable — every Int16 644 hit
is a static curve table (`0x56156`, `0x56BC4`, `0x5888C`, `0x59BC6`,
`0x4C03E`, `0x18CBA`) or a transient sequential/graphics buffer. Menu value
is computed on the fly. Equip bonus alone (292/322) not stored near the block
either. Conclusion: out-of-battle final attributes must be computed
(base + equipment [+ digievo]).

Cross-check with `agumon-on-{battle,asuka-city,menu}` (Agumon slot 1, no
digievo, base STR 48, equipped 340): battle block `0xA45C0` = lv1, STR **340**,
DEF 50, SPI 52, WIS 30, SPD **11** (base 19 — equipment also subtracts),
resists identical to base. No address holds 340 (Agumon) and 644/674
(Kotemon) out of battle in any alignment/size — negative result confirmed.
**Stale data:** out of battle the pair is **not** cleared; it keeps the last
fight (Agumon out-of-battle snaps still show Dinohumon 674 + old enemy).
Zero only before the first fight after load. Always gate by `isInBattle`.
RetroAchievements notes `0x48DA4` (party slot 1) and `0x4A058` (Agumon STR)
are the already-integrated party slot and **base** STR (`0x4A030 + 0x28`).

**No Charisma** in this block (persistent `DigimonStatusAddresses` still has Cha
at +0x32; combat block jumps Speed → Fire).

**Species @ `+0x24` (Int16, high-byte family id `N×0x100`) — confirmed so far:**

| `+0x24` | Decimal | `enemy.json` label | Examples (confirmed) | Notes |
|---------|---------|--------------------|----------------------|-------|
| `0x100` | 256 | *(TBD — JSON often `rare`)* | Dinohumon (ally), Cardmon tree (`515` / memoryId `457`), Baronmon, Numemon | Real code, not zero/undefined; human label still open |
| `0x200` | 512 | `dino` | Triceramon, Tyrannomon, Tuskmon | |
| `0x300` | 768 | `evil` | DemiDevimon | Confirmed 2026-08-09 batch |
| `0x400` | 1024 | `ghoul` | Bakemon, Raremon | Confirmed 2026-08-09 batch |
| `0x500` | 1280 | `machine` | Andromon, Hagurumon, Mamemon, Datamon, Bulbmon, Thundermon, Maildramon, HiAndromon | Confirmed 2026-08-09 batch |
| `0x600` | 1536 | `mammal` | Tapirmon, Mammothmon, Betamon, Apemon | |
| `0x700` | 1792 | `bird` | Kiwimon (`kabuterimon-3`) | |
| `0x800` | 2048 | `insect` | Kunemon, Kuwagamon, Yanmamon; ally Kabuterimon | |
| `0x900` | 2304 | `plant` | Vegiemon (`kabuterimon-2`), Woodmon | |
| `0xA00` | 2560 | `fish` | Gekomon, Shellmon, Coelamon, Cardmon aquatic (`516` / memoryId `458`) | |
| `0xB00` | 2816 | `dragon` | Seadramon, Airdramon | Label follows `enemy.json`; not fish despite aquatic theming |

**Still unmapped:** definitive name for `0x100` (vs placeholder `rare`).

**NPC multi-enemy battle slots (confirmed 2026-08-09):** enemies for one NPC fight
occupy `0xA44D0`, `0xA44F0`, `0xA4510` (stride `0x20`) — each with its own
`memoryId` + HP. Do not treat `0xA44D0` alone as a shared party id.

**Active enemy slot resolution (integrated 2026-08-27):** `EnemyReader` picks
which of the three enemy slots to expose as `State.Battle.Enemy`:

1. `ActiveEnemySlotIndex` @ `0xA446C`, if index valid and slot `id != 0` —
   **no HP gate** (stays on KO'd front enemy until the next one enters).
2. Else first slot with `id != 0` and `HP.Current > 0`.
3. Else highest-index slot with `id != 0` (all KO — stay on last defeated).
4. Else slot 0 (empty battle fallback).

Wired in `EnemyAddresses.json` as `SlotStride`, `SlotCount`, `ActiveEnemySlotIndex`.

**Changed 2026-09-27:** removed the former step 1 (match `0xA4558` to a slot id)
and `ActiveUnitId` from `EnemyAddresses.json`. `0xA4558` is camera focus; it never
changed the result in wild/tamer phase snaps and risked stale picks mid-switch and
wrong slot with duplicate enemy ids.

**Tamer phase snaps (2026-09-27, 41 snaps, Dinohumon vs Crabmon/Gizamon/Gekomon,
voluntary tamer switch + KO switches):** step 1 result always equalled
`ActiveEnemySlotIndex` alone. `0xA446C` flips when the new enemy is **on field**
(`troca-realizada-*`; already at switch animation after a KO). `0xA4558` behaves
as **camera focus**: target during attack animation, but also resets to the ally
(`386`) at action choice / after switches, and shows a stale enemy (`132` during
switch animation to Gekomon, `110` during switch back to Gizamon). Not reliable
alone; harmless as step 1 here.

**Bug (confirmed 2026-09-02, fixed):** step 2 (first live slot) failed when tamer
switches front while prior Digimons stay alive. Fix: `ActiveEnemySlotIndex` @
`0xA446C` as step 2 when step 1 misses.

**Ally Digimon `+0x24` is species too — not a fixed “player” marker.**  
Same table applies to the engaged ally half of `0xA4580`/`0xA45C0` (e.g. Dinohumon
`0x100`, Kabuterimon `0x800`). Status-resist tails still differ per combatant;
do not confuse with species.

Earlier `N × 0x200` guess is incomplete — use the table above.

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

**Revised (suspected actor/target, 2026-09-27):** phase-labelled Kunemon fight
(`dinohumon-escolhendo-acao-1` … `batalha-ganha-13`) fits **`0xA4580` = actor**
(written at action **declaration**: Kunemon at `kunemon-declarou-ataque-5`,
Dinohumon at `dinohumon-declarou-ataque-9`) and **`0xA45C0` = target** (Dinohumon
while Kunemon attacks; Kunemon while Dinohumon attacks; Dinohumon in **both**
blocks at `terminou-animacao-poder-duplo-4` — self-buff). Blocks stay stale until
the next action. The older `*-attacking-*` snaps above were likely captured at
ambiguous phases; re-test before relying on it. **Reinforced** by 41 tamer snaps
(Crabmon/Gizamon/Gekomon): actor written at every `*-escolheu-ataque-*`; tamer
switch does not rewrite the pair until the new enemy acts or is targeted
(exception: Gekomon appeared as actor right after entering, before its attack).

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
wired on ally party slots via `Parties/InBattleAddresses.json` (`Strength` /
`Defense` / `Speed` → Digimon `InBattle`):

| Offset | Field |
|--------|-------|
| `+0x10` | STR delta |
| `+0x12` | DEF delta |
| `+0x14` | SPD delta |

Same offsets on enemy `0xA44D0` — wired as `State.DigimonBattle.Enemy` via
`Battles/EnemyAddresses.json` (`DigimonBattleChanged` / InitialState).

### Field skill / item (element strengthen / weaken) — **confirmed** id @ `0xA4530`

**Integrated:** `Battles/DigimonBattleAddresses.json` `Field` → `DigimonBattle.Field` (`DigimonBattleChanged`). `0` = neutral field (valid value).

Live combat attrs/resists (`0xA4580` / `0xA45C0`) do **not** change when a field
is applied. Field is a global battle state that **only strengthens** one element.
Bonus is **not** fixed: it comes from potency `0xA4532` (see *Field potency*).
The in-game "weakens X" text has **no observable effect** (see *Field weaken*).

**Active field id — `0xA4530` (byte / Int16 LE, low byte):**

| Value | Field |
|------:|-------|
| `0` | none |
| `2` | Fire |
| `3` | Water |
| `4` | Ice |
| `5` | Wind |
| `6` | Thunder |
| `7` | Machine |
| `8` | Dark |

(`1` unused / unseen.) Same order as combat resist list (Fire→…→Dark), with
`0` = none and id starting at `2` for Fire.

**Confirmed 2026-08-26** — Plug Cape / Shellmon, same battle, item fields,
snaps `before/after-*-field.bin`. `chain-match` on after-sequence:

`0xA4530: 0 → 2 → 3 → 4 → 5 → 6 → 7 → 8`

(`before-fire` alone is `0`; later `before-*` keep the previous field id until
the matching `after-*`.)

**Cluster `0xA4414…A442A`:** still lights up when a field becomes active (e.g.
`A4414=16`, `A4418=220`, `A441C=2`, `A442A=1`) but does **not** identify which
field nor its source (item and skill Wind snaps were identical). Can be already
cleared while `0xA4530` / `0xA4532` still hold the field (`skill-machine-field` /
`skill-water-field`). Prefer **`0xA4530`** as SSOT.

#### Field potency — `0xA4532` (**confirmed** 2026-09-28)

Strengthen multiplier = `1 + A4532 / 128`. `0` when no field.

| Source | `A4532` | Multiplier | Damage evidence |
|--------|--------:|-----------:|-----------------|
| Item (any element) | `64` (`0x40`) | ×1.5 (+50%) | Sakuyamon thunder tech vs Kunemon 850 → **1275**; Raio Elétrico vs Hagurumon/Clockmon 2200 → **3300**; Giga Congelar 780 → **1170**; Chuva de Neve 650 → **975** (Ice item) |
| Skill (any Campo skill) | `127` (`0x7F`) | ×1.992 (~+99%) | Sakuyamon thunder tech vs Kunemon 850 → **1693** (predicted 1693.4); repeated to rule out crits |

#### Field skills are bugged — always Thunder (**confirmed** 2026-09-28)

Every Digimon Campo skill writes **`A4530 = 6` (Thunder)** + **`A4532 = 127`**,
regardless of the element it claims. The item path is the only one that writes
the matching element id (`2`–`8`, potency `64`).

| Technique | Digimon (id) | Claimed | Written | Evidence |
|-----------|--------------|---------|---------|----------|
| `thunderField` | Taomon (375) | Thunder | `6` | old snaps `taomon-*-field` |
| `iceField` | Sakuyamon (376) | Ice | `6` / `127` | old snaps + `possible-ice-field`; in-game: boosts **Thunder**, Ice unchanged |
| `fireField` | BK WarGreymon (267) | Fire | `6` | old snaps `bkwargreymon-*-field`; in-game: boosts **Thunder** |
| `darkField` | MaloMyotismon (378) | Dark | `6` / `127` | old snaps (first pair contaminated, retaken) |
| `windField` | Seraphimon (214) | Wind | `6` / `127` | `skill-wind-field` (vs `item-wind-field` = `5` / `64`) |
| `waterField` | Rosemon (144) | Water | `6` / `127` | `skill-water-field` |
| `metalField` | MetalGarurumon (196) | Machine | `6` / `127` | `skill-machine-field` |

Old snaps only logged `A4530`; `127` was explicitly seen on Malo and all 2026-09-28
snaps. Taomon matches only by coincidence (still `127` potency, not `64`).

`0xA38A0..A38A3` (zero with no field; item Wind `07 00 14 05`, skill Wind
`07 00 51 32`, skill Ice `07 00 c9 32`, skill Machine/Water both `07 00 61 31`)
looked like a last-action record, but **`A38A2` is not the skill id** (Machine and
Water identical). No address found yet that stores the claimed element / skill used.

#### Field weaken — **no effect observed** (2026-09-28, item fields only)

| Attacker | Move | Field (item) | Should weaken | Result |
|----------|------|--------------|---------------|--------|
| Clockmon | Míssil Mágico (magic, Machine) | Thunder | Machine | 426 → 426 (also 426 under skill field) |
| Airdramon (gold) | Mega Tornado (Wind) | Ice | Wind | 259 → 259 |
| Sakuyamon (player) | thunder technique | Dark | Thunder | unchanged |

Weaken appears absent for both player and enemy attacks. `field.json` `weakens`
and technique descriptions reflect game text only. Not yet decided how Digivice
should present it.

Side notes from the same session:

- Airdramon (gold, `groupId` 207) used **Mega Tornado**, but `enemy.json` lists
  `techniqueId: "airBlast"` — enemy data to review.
- Airdramon physical `noElement` attack on Kotemon 49 → 20 with a field: not a
  field effect (physical, no element) — discard as noise / other modifier.

#### Field — open questions

- Does strengthen also apply to **enemy** attacks? All strengthen evidence so far
  comes from player attacks.
- **Duration:** item field ends after **5 player actions** (skill field untested).
  Countdown address not found (`A441C=2` / `A4418=220` right after apply do not
  look like a 5-count) — needs one snap per turn until the field expires.
- Where (if anywhere) the claimed element / skill id of a Campo cast is stored.

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

### Status / debuff byte (confirmed)

Battle HP/MP slot offset **`+0x1C`** (byte) — same layout for ally and enemy.

| Side | Slot base | Condition abs | Source |
|------|-----------|---------------|--------|
| Ally | `0xA4470` + `n × 0x20` | e.g. Guilmon slot0 `0xA448C` | `InBattleAddresses.json` |
| Enemy | `0xA44D0` (+ `0x20` / `0x40` multi) | first enemy `0xA44EC` | `EnemyAddresses.json` |

| Snap pair | Side | Normal | Debuff | Value @ `+0x1C` |
|-----------|------|--------|--------|-----------------|
| `guilmon-normal` / `guilmon-poison` | ally | 0 | Poison | **`0x01`** |
| `new-condition` (ally slot0 `0xA448C`) | ally | 0 | Paralyze / in-game “Freeze” | **`0x02`** |
| `sleep-condition` (ally slot0 `0xA448C`, Dinohumon `386`) | ally | 0 | Sleep | **`0x08`** |
| `guilmon-gekomon-normal` / `guilmon-gekomon-confuse` | ally | 0 | Confuse | **`0x04`** |
| `redGoburimon-without-poison` / `redGoburimon-with-poison` | enemy | 0 | Poison | **`0x01`** @ `0xA44EC` |

Exclusive status-ailment byte (one value at a time — a new ailment **overwrites**
the byte; values are not combined): poison `0x01`, paralyze `0x02`, confuse `0x04`,
sleep `0x08`. Ally poison/confuse/paralyze/sleep: only active slot changed. Enemy
poison: ally `+0x1C` stayed 0; enemy id `0x018E` stable. Secondary on enemy poison
pair: `+0x04` `0x00→0x01` (unknown — not Condition); HP current also dropped
(combat damage). Confuse snap noise: `+0x1F` `0x00→0x50` (timer?); paralyze snap
`+0x1D` `0x60` on `new-condition` (timer?). Wired as `InBattle.Condition` /
`Enemy.Condition`. Digivice tooltip map: `DigimonDebuff` bitflags
(`digimon-debuff.constant.ts`) via `DigimonService.getActiveDebuffs`.

### Cardmon “curse” (suspected / incomplete — 2026-08-26)

Snaps: `cardmon-cursed.bin` (message “amaldiçoado”) vs `cardmon-normal.bin`
(later fight, start of Cardmon battle before hit). **Not** a same-fight pair —
expect HP/turn noise.

| Check | Result |
|-------|--------|
| Ally/enemy Condition `@ +0x1C` | **`0` in both** — curse is **not** the poison/confuse status byte (matches “no condition icon”) |
| `0xA4530` field id | **`0` both** — not elemental Field |
| `0xA4414…A442A` cluster | **On only in cursed** (`A4414=0x10`, `A441C=0x7A`, `A4424=1`, `A442A=1`) — same neighborhood as Campo skills, different `A441C` |
| Digimon status region `stats` | **0 diffs** |
| Enemy `+0x04` | `0→1` on cursed (known turn/state noise) |

**Still open:** same-battle before/after curse hit; whether `A441C=0x7A` is a
stable curse mode id or one-off timer/FX; any HUD/timer byte outside battle
table.

---

