# Known Patterns — Digimon Battle: combatant pair `0xA4580` / `0xA45C0`

Swappable combatant blocks with live Level, attributes, elemental and status
resists and species; actor/target behavior; enemy catalog copy. **Not
integrated** (research). Index of all battle addresses: [patterns-battle.md](patterns-battle.md).

Maintained by the memory-compare skill (retrofeed). Mark `(confirmed)` or `(suspected)`. Append; do not delete without evidence.

---

## Layout (confirmed)

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
| +0x24 | Species / family | Int16 `N×0x100` — table in *Species* below |
| +0x26… | unused / zero in snaps seen | |

**No Charisma** in this block (persistent `DigimonStatusAddresses` still has Cha
at +0x32; combat block jumps Speed → Fire).

Field skills / items do **not** rewrite these attrs or resists
([patterns-battle-field.md](patterns-battle-field.md)).

## Only computed-attrs source (confirmed, 2026-09-27)

Snaps `dinohumon-644-on-{battle,central-park,menu}` (Kotemon slot 1, base STR 352,
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

**vs persistent DigimonStatus (~`0x4949C` for Kotemon in west snaps):** combat
attrs ≠ persistent (e.g. STR 674 vs 352). Combat values look like
digievolution + gear baked in; persistent block does not mid-fight update
(same story as HP).

---

## Species @ `+0x24` (Int16, high-byte family id `N×0x100`)

Confirmed so far:

| `+0x24` | Decimal | `enemy.json` label | Examples (confirmed) | Notes |
|---------|---------|--------------------|----------------------|-------|
| `0x100` | 256 | *(TBD — JSON often `rare`)* | Dinohumon (ally), Cardmon tree (`515` / memoryId `457`), Baronmon, Numemon | Real code, not zero/undefined; human label still open |
| `0x200` | 512 | `dino` | Triceramon, Tyrannomon, Tuskmon | |
| `0x300` | 768 | `evil` | DemiDevimon | Confirmed 2026-08-09 batch |
| `0x400` | 1024 | `ghoul` | Bakemon, Raremon | Confirmed 2026-08-09 batch |
| `0x500` | 1280 | `machine` | Andromon, Hagurumon, Mamemon, Datamon, Bulbmon, Thundermon, Maildramon, HiAndromon | Confirmed 2026-08-09 batch |
| `0x600` | 1536 | `beast` | Tapirmon, Mammothmon, Betamon, Apemon | |
| `0x700` | 1792 | `bird` | Kiwimon (`kabuterimon-3`) | |
| `0x800` | 2048 | `insect` | Kunemon, Kuwagamon, Yanmamon; ally Kabuterimon | |
| `0x900` | 2304 | `plant` | Vegiemon (`kabuterimon-2`), Woodmon | |
| `0xA00` | 2560 | `fish` | Gekomon, Shellmon, Coelamon, Cardmon aquatic (`516` / memoryId `458`) | |
| `0xB00` | 2816 | `dragon` | Seadramon, Airdramon | Label follows `enemy.json`; not fish despite aquatic theming |

**Still unmapped:** definitive name for `0x100` (vs placeholder `rare`).

**Ally Digimon `+0x24` is species too — not a fixed “player” marker.**  
Same table applies to the engaged ally half of `0xA4580`/`0xA45C0` (e.g. Dinohumon
`0x100`, Kabuterimon `0x800`). Status-resist tails still differ per combatant;
do not confuse with species.

Earlier `N × 0x200` guess is incomplete — use the table above.

---

## Which base holds whom

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

**Party coverage:** only the **engaged** ally appears in this pair. Bench
party members keep HP in `0xA4470+n×0x20` but have **no** per-slot combat
attr block in these snaps — switching front Digimon replaces the ally half
of the pair (`kotemon`/`patamon`/`renamon`).

---

## Actor / target

**Not attacker/defender slots** — tested with named turn snaps
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

---

## Attr buffs on the pair

Buffs rewrite the live attribute in the pair **and** add a delta on the fixed
battle slot (`+0x10` / `+0x12` / `+0x14`, see
[patterns-battle-slots.md](patterns-battle-slots.md)). Blocks may swap bases
between snaps; identify by fingerprint, then compare.

**Strength — confirmed (ally):** `dinohumon-normal` / `dinohumon-buffed` (vs Tapirmon `206`):

| Field | Normal (ally half) | Buffed (ally half) |
|-------|--------------------|--------------------|
| Strength | **674** | **926** (+252) |
| DEF / SPI / WIS / SPD | unchanged | unchanged |
| Elemental + status resists | unchanged | unchanged |
| Enemy half (Tapirmon) | unchanged | unchanged |
| Persistent DigimonStatus (~`0x4949C`) | STR 352 etc. | **identical** (buff did not write through) |

Blocks **swapped** bases between the two snaps (ally was at `0xA4580` then at
`0xA45C0`). Ally battle slot `+0x10` (`0xA4480`) went `0→252` (same delta).
MP Current spent `1140→1098` (−42) on the skill.

**SPD — confirmed (enemy):** `hagurumon-1` → `hagurumon-2` (Velocidade
Acima). Hagurumon fingerprint L23 / STR 420 / Machine 270:

| Field | Before | After |
|-------|--------|-------|
| Combat SPD (matched across swap) | **336** | **420** (+84) |
| Other combat attrs/resists | unchanged | unchanged |
| Ally combat half | unchanged | unchanged |
| Enemy slot `+0x14` (`0xA44E4`) | **0** | **84** |
| Enemy slot `+0x10` (STR delta) | 0 | 0 |
| Enemy MP Current (`+0x0C`) | 336 | 288 (−48 skill cost) |

**DEF — confirmed (ally):** `growlmon-normal` → `growlmon-def-up`.
Growlmon fingerprint L99 / STR 659 / SPD 632:

| Field | Before | After |
|-------|--------|-------|
| Combat DEF (matched across swap) | **494** | **679** (+185) |
| Other combat attrs/resists | unchanged | unchanged |
| Enemy combat half | unchanged | unchanged |
| Ally0 slot `+0x12` (`0xA4482`) | **0** | **185** |
| Ally0 `+0x10` / `+0x14` (STR/SPD deltas) | 0 | 0 |
| Ally0 MP Current (`+0x0C`) | 5566 | 5524 (−42 skill cost) |

---

## Tamer enemies vs wild `enemy.json` entry (confirmed 2026-11-05)

Snapshots: North Sector tamers (`ice-witch-*`, `fire-knight-*`,
`dark-knight-*`, `robot-*`) vs wild `numemon-new`, `vikemon`, `skull-meramon`,
`whamon`, `metal-seadramon`, `tylomon`.

- Tamer enemies reuse the **same `memoryId`** as the wild species (Frigimon
  `23`, Meramon `9`, SkullMeramon `189`, …); resists and species are
  **identical** to the wild entry.
- Level is set by the group; the 5 attrs are `floor(wild × (16 + k) / 16)` with
  `k` per enemy: `0` (Robot Dragomon/Pukumon, same level), `1` (Ice Witch all,
  Fire Knight Meramon/SkullMeramon, Dark Knight Vilemon(Purple)), `2`
  (Devidramon), `3` (SkullSatamon). `k` correlates with the level gain but no
  exact formula found.
- Enemy HP Max = `24 × (level + 5)` for every enemy seen here except Whamon
  (`1800` at lv45, wild and json agree).
- The pair only holds actor/target: an enemy that never acted has **no** block
  (Fire Knight BlueMeramon `104`, Robot Giromon `41`) — only slot HP/MP.
- Wired as `enemy.json` `603`–`614` (groups 19/20/21/193). BlueMeramon `608`
  (attrs = wild × 17/16, `k` assumed from Fire Knight's other enemies) and
  Giromon `614` (= wild, same level as Robot's other enemies) are
  **provisional** — no combatant block captured.
- `0x42B6C` read `0x0182` in all 18 battle snaps (different enemies) — stale
  here; not usable as drop without a fresh-load control.

## Enemy catalog attr copy (variable address; id → attrs +0x0E)

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

### Enemy definition table (confirmed 2026-11-07)

The catalog records above are entries of a contiguous **enemy definition
table** loaded in battle. Record stride **`0x46`**, Int16 LE:

| Offset | Field |
|--------|-------|
| +0x00 | Enemy id = `memoryId` (same value as enemy slot `0xA44D0 + 0x00`) |
| +0x06 | Table index (sequential; the table is ordered by it, not by id) |
| +0x0E…+0x16 | Strength, Defense, Spirit, Wisdom, Speed |
| +0x18…+0x24 | Fire, Water, Ice, Wind, Thunder, Machine, Dark |
| +0x26…+0x2E | Status resists: poison, paralyze, confuse, sleep, KO |
| +0x30 | Species family (`N`, same table as `+0x24` of the pair ÷ `0x100`) |

- No level, HP, name or `groupId` in the record.
- Base address varies (`0xBB508` … `0xE51A4` in `cardmon-*.bin`,
  `0xCFAE4` in `battle-against-natsumi.bin`); find it by walking `+0x06`.
- Every battle snapshot so far holds the same chunk: indexes **58–250**
  (193 records, ids 4–467). Indexes 0–57 not seen yet.
- `memoryId` identification: match the 5 attrs + 7 elemental resists (+
  species to split Cardmon(G) 463 / (H) 464) against `enemy.json`. Used on
  2026-11-07 to fill all 56 `memoryId: 0` entries (incl. DRI agent bosses
  449–456, Cardmons 457–464); validated in battle for MetalGreymon and six
  Cardmons, the rest pending in-game confirmation.
- Status resists do **not** encode `enemy.json` `can*`: 53 of 61 `can*: false`
  fields have a non-zero value here. `enemy.json` zeroes the value when
  `can*` is false (source shows `--`), so only compare status where `can*` is true.
- **Hypothesis (open):** `--` (immune) and `Steal`/`STR⇓`/`DEF⇓`/`SPD⇓ = No` come
  from the **battle/group**, not from this record. All 20 `enemy.json` entries
  with any `can*: false` are bosses or special groups (Game Master F-010, groups
  189–192, 322); none of the 74 wild entries has one. Same `memoryId` keeps
  base resists here (Game Master Minotarumon(Blue) shares record 399 with the
  wild one: 50/50/0/50/50 vs source all `--`). Boss status values in
  `enemy.json` (e.g. HiAndromon, WaruMonzaemon `0` vs record `50`) follow the
  source, not this record. To confirm: pair block status tail (`+0x1A…+0x22`)
  and record tail (`+0x32…+0x44`) in a boss battle vs a wild control.
- Ids **438–448, 466, 467** have no `enemy.json` entry (unidentified enemies).
