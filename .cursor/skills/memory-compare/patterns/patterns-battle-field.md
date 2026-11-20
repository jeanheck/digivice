# Known Patterns — Digimon Battle: field and battle effects

Elemental field (id, potency, skill bug, weaken), the `0xA4414…A442A` effect
cluster and the Cardmon “curse”. Index of all battle addresses: [patterns-battle.md](patterns-battle.md).

Maintained by the memory-compare skill (retrofeed). Mark `(confirmed)` or `(suspected)`. Append; do not delete without evidence.

---

## Field skill / item (element strengthen / weaken) — **confirmed** id @ `0xA4530`

**Integrated:** `battles/digimon-battle-addresses.json` `field` → `digimonBattle.field`. `0` = neutral field (valid value).

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
`skill-water-field`). Prefer **`0xA4530`** as SSOT. Also lights up on the Cardmon
“curse” (see below).

### Field potency — `0xA4532` (**confirmed** 2026-09-28, not integrated)

Strengthen multiplier = `1 + A4532 / 128`. `0` when no field.

| Source | `A4532` | Multiplier | Damage evidence |
|--------|--------:|-----------:|-----------------|
| Item (any element) | `64` (`0x40`) | ×1.5 (+50%) | Sakuyamon thunder tech vs Kunemon 850 → **1275**; Lightning Bolt vs Hagurumon/Clockmon 2200 → **3300**; Giga Freeze 780 → **1170**; Snow Storm 650 → **975** (Ice item) |
| Skill (any Field-type skill) | `127` (`0x7F`) | ×1.992 (~+99%) | Sakuyamon thunder tech vs Kunemon 850 → **1693** (predicted 1693.4); repeated to rule out crits |

### Field skills are bugged — always Thunder (**confirmed** 2026-09-28)

Every Digimon Field-type skill writes **`A4530 = 6` (Thunder)** + **`A4532 = 127`**,
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

### Field weaken — **no effect observed** (2026-09-28, item fields only)

| Attacker | Move | Field (item) | Should weaken | Result |
|----------|------|--------------|---------------|--------|
| Clockmon | Magic Missile (magic, Machine) | Thunder | Machine | 426 → 426 (also 426 under skill field) |
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

### Field — open questions

- Does strengthen also apply to **enemy** attacks? All strengthen evidence so far
  comes from player attacks.
- **Duration:** item field ends after **5 player actions** (skill field untested).
  Countdown address not found (`A441C=2` / `A4418=220` right after apply do not
  look like a 5-count) — needs one snap per turn until the field expires.
- Where (if anywhere) the claimed element / skill id of a Field-type cast is stored.

---

## Cardmon “curse” (suspected / incomplete — 2026-08-26)

Snaps: `cardmon-cursed.bin` (in-game "cursed" message) vs `cardmon-normal.bin`
(later fight, start of Cardmon battle before hit). **Not** a same-fight pair —
expect HP/turn noise.

| Check | Result |
|-------|--------|
| Ally/enemy Condition `@ +0x1C` | **`0` in both** — curse is **not** the poison/confuse status byte (matches “no condition icon”) |
| `0xA4530` field id | **`0` both** — not elemental Field |
| `0xA4414…A442A` cluster | **On only in cursed** (`A4414=0x10`, `A441C=0x7A`, `A4424=1`, `A442A=1`) — same neighborhood as Field-type skills, different `A441C` |
| Digimon status region `stats` | **0 diffs** |
| Enemy `+0x04` | `0→1` on cursed (known turn/state noise) |

**Still open:** same-battle before/after curse hit; whether `A441C=0x7A` is a
stable curse mode id or one-off timer/FX; any HUD/timer byte outside battle
table.
