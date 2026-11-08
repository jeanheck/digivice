# Known Patterns — Digimon Battle: fixed battle slots

Fixed per-position battle slots (ally and enemy HP/MP, buff deltas, status byte),
active slot indexes, the strip near Blast (`groupId`, enemy level/HP, drop) and
the enemy drop design. Index of all battle addresses: [patterns-battle.md](patterns-battle.md).

Maintained by the memory-compare skill (retrofeed). Mark `(confirmed)` or `(suspected)`. Append; do not delete without evidence.

---

## Battle HP/MP slot table @ `0xA4470` (stride `0x20`) — confirmed

Snapshots:
- Tapirmon scout: `in-combat.bin` / `out-combat.bin`
- Mammothmon turn chain: `out-combat-west-1` → `in-combat-west-1..4` → `out-combat-west-2`

**Persistent party HP does not update mid-combat** — Kotemon `0x494BC`
(Current) stayed 1850 through all `in-west-*`, then became 1400 only on
`out-combat-west-2` (post-battle sync).

Layout per slot (Int16 LE) — **note: Max then Current** (inverted vs
`DigimonStatusAddresses` persistent `Current`/`Max`):

| Offset | Field | Evidence |
|--------|-------|----------|
| +0x00 | unit id / digievo token | Kotemon 386; enemy Tapirmon 206 / Mammothmon 212 |
| +0x06 | **HP Max** | Kotemon stayed 1850 while damaged; Mammothmon stayed 672 |
| +0x08 | **HP Current** | Kotemon `1850→1400`; Mammothmon `672→432→192→0` (`chain-match`) |
| +0x0A / +0x0C | **MP Max / MP Current** | Confirmed Max-then-Current (same as HP). `dinohumon-buffed`: MP `1140→1098` after skill |
| +0x10 / +0x12 / +0x14 | STR / DEF / SPD buff deltas | See *Attr buff deltas* below |
| +0x1C | Status / debuff byte | See *Status / debuff byte* below |

| Slot | Base | Role |
|------|------|------|
| 0 | `0xA4470` | Ally party slot 0 — live HP Current @ `0xA4478` |
| 1 | `0xA4490` | Ally party slot 1 — live HP Current @ `0xA4498` |
| 2 | `0xA44B0` | Ally party slot 2 — live HP Current @ `0xA44B8` |
| 3 | `0xA44D0` | Enemy — live HP Current @ `0xA44D8` |

Wired: ally slots via `parties/in-battle-addresses.json` (`digimon.inBattle`);
enemy slots via `battles/enemy-addresses.json` (`digimonBattle.enemy`, no MP).

### Enemy MP (confirmed, 2026-11-08)

Most enemies start with **MP Max = Current = 9999** (virtually infinite), but
they **do spend** it: the pool just never runs out in practice. Some enemies have
a "normal" MP pool that can matter in the fight.

| Snapshot | Enemy | MP Current / Max |
|----------|-------|------------------|
| `flymon-pink.bin` | Flymon(Pink) lv33 (memoryId 400) | 9999 / 9999 |
| `ebidramon.bin` | Ebidramon lv34 (165) | 9999 / 9999 |
| `blossomon.bin` | Blossomon lv34 (200) | 9999 / 9999 |
| `kimeramon.bin` → `kimeramon-6.bin` | Kimeramon lv44 (237), with Deltamon (138) and Triceramon(Red) (402) | `9999→9879→9759→9399` / 9999 (spends in steps of 120) |
| `bulbmon-1..3.bin` | Bulbmon lv43 boss (436) | 144 / **960** |
| Kunemon fight (`patterns-battle-combatants.md`) | Kunemon | `336→288` (−48 skill cost) |

Product decision: no enemy MP bar in the UI for now (pointless for 9999 enemies,
inconsistent if shown only for the others).

**Slots are fixed by party position, not by who is front/active.**
Switch snaps (`in-combat-kotemon` / `patamon` / `renamon`, same Kunemon fight):
Kotemon HP stayed at slot0 (`0xA4478`), Patamon at slot1, Renamon at slot2
across all three — only the active marker moved.

### Active indexes and focus

| Address | Role | Evidence |
|---------|------|----------|
| `0xA4468` | Active ally slot index (0/1/2) | `0→1→2` when switching Kotemon→Patamon→Renamon |
| `0xA446C` | Active **enemy** slot index (0/1/2) | **Confirmed** Gordon tamer snaps: `gordon-crabmon` `0`, `gordon-gizamon` `1`, `gordon-suposed-to-be-gekomon` `2` — mirrors ally `@0xA4468`; required when multiple enemy slots alive and `0xA4558` holds ally id |
| `0xA4558` | Active unit id → **focused / target unit** | `386→234→375` (Dinohumon→Angewomon→Taomon); Gordon: `386→132→386` — can be **ally** memoryId while enemy front changes. Phase snaps (2026-09-27, Kunemon fight): stays `386` through Dinohumon self-buff and Kunemon attack on Dinohumon; flips to `32` only at `dinohumon-animacao-ataque-10` (animation start, **not** at declaration `-9`); stays `32` after KO/victory. = unit on camera/receiving the action, **not** the actor, stale between actions |

### Ally slot `+0x00` id

**2026-09-27, `on-wild-digimon-battle-1..7`, Kotemon/Dinohumon vs Kunemon, no switch:**
slot0 `0xA4470` = `386` = Kotemon `ActiveDigievolution`
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

### Lifecycle and mirrors

Pre-battle (`out-combat-west-1`): table zeroed. Post-battle (`out-west-2`) may
still hold last values briefly (enemy current 0, ally current 1400).

HUD mirrors (discard for logic): `0xE1408`/`0xE140C`/`0xE1410` track ally
current HP; `0xE141C`/`0xE1420`/`0xE1424` track enemy current HP.

---

## Enemy slots and active enemy resolution

**NPC multi-enemy battle slots (confirmed 2026-08-09):** enemies for one NPC fight
occupy `0xA44D0`, `0xA44F0`, `0xA4510` (stride `0x20`) — each with its own
`memoryId` + HP. Do not treat `0xA44D0` alone as a shared party id.

**Active enemy slot resolution (integrated 2026-08-27):** `EnemyReader` picks
which of the three enemy slots to expose as `digimonBattle.enemy`:

1. `ActiveEnemySlotIndex` @ `0xA446C`, if index valid and slot `id != 0` —
   **no HP gate** (stays on KO'd front enemy until the next one enters).
2. Else first slot with `id != 0` and `HP.Current > 0`.
3. Else highest-index slot with `id != 0` (all KO — stay on last defeated).
4. Else slot 0 (empty battle fallback).

Wired in `enemy-addresses.json` as `slotStride`, `slotCount`, `activeEnemySlotIndex`.

**Changed 2026-09-27:** removed the former step 1 (match `0xA4558` to a slot id)
and `ActiveUnitId` from `enemy-addresses.json`. `0xA4558` is camera focus; it never
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

---

## Attr buff deltas @ `+0x10` / `+0x12` / `+0x14` (confirmed trio)

Int16 LE per battle slot, same offsets for ally and enemy slots. Buffs also
rewrite the live attribute in the swappable combatant pair — full before/after
tables in [patterns-battle-combatants.md](patterns-battle-combatants.md) (*Attr buffs on the pair*).

| Offset | Field | Evidence |
|--------|-------|----------|
| `+0x10` | **STR delta** | Ally: `dinohumon-buffed` `0xA4480` `0→252` (= combat STR gain `674→926`) |
| `+0x12` | **DEF delta** | Ally: `growlmon-normal`→`def-up` `0xA4482` `0→185` (= combat DEF `494→679`) |
| `+0x14` | **SPD delta** | Enemy: `hagurumon-1`→`2` `0xA44E4` `0→84` (= combat SPD `336→420`) |

Wired: ally slots via `parties/in-battle-addresses.json` (`strength` / `defense` /
`speed` → `digimon.inBattle`); enemy `0xA44D0` via `battles/enemy-addresses.json`
(`EnemyReader` → `digimonBattle.enemy`).

---

## Status / debuff byte (confirmed)

Battle HP/MP slot offset **`+0x1C`** (byte) — same layout for ally and enemy.

| Side | Slot base | Condition abs | Source |
|------|-----------|---------------|--------|
| Ally | `0xA4470` + `n × 0x20` | e.g. Guilmon slot0 `0xA448C` | `in-battle-addresses.json` |
| Enemy | `0xA44D0` (+ `0x20` / `0x40` multi) | first enemy `0xA44EC` | `enemy-addresses.json` |

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
`+0x1D` `0x60` on `new-condition` (timer?). Wired as `inBattle.condition` /
`enemy.condition`. Digivice tooltip map: `DigimonDebuff` bitflags
(`digimon-debuff.constant.ts`) via `DigimonService.getActiveDebuffs`.

---

## Strip near Blast @ `0x42B28` — NOT live current HP

| Address | Notes |
|---------|-------|
| `0x42B28` | battle-related counter/flag (1 Natsumi, 20 Genji, varies wild) |
| `0x42B2C` | **`groupId`** (Int16 LE) — encounter group template id (site: "Group F-201"). Natsumi **201**, Genji **272**, wild 39–43. Stable across multi-enemy NPC fights. Wired in `enemy-addresses.json` → `digimonBattle.enemy.groupId` |
| `0x42B34` | same enemy token as `0xA44D0` |
| `0x42B38` | enemy level (Mammothmon 23) |
| `0x42B3A` | enemy **max/initial** HP only — stayed 672 while `0xA44D8` dropped |
| `0x42B3C` | enemy MP |
| `0x42B6C` | **drop item Val** (Int16, live battle) — see *Enemy drops (variable)* below |

Tamer fight: the strip stays on the **first** enemy (Crabmon 197, lv17, HP 528)
after it is KO'd and replaced — use the enemy slots, not the strip, for the
current enemy.

---

## Enemy drops (variable) — open design (2026-08-09)

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
