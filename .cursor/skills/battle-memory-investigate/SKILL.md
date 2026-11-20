---
name: battle-memory-investigate
description: >-
  Investigates Digimon World 2003 combat RAM — ally/enemy HP/MP slots, active
  slot indexes, buffs/debuffs, combatant attributes and resistances, battle
  field, enemy group/drops, blast gauge, status/condition. Use when the user
  brings battle snapshots or diffs, asks about 0xA4470 / 0xA44D0 / 0xA4580 /
  0xA45C0 / 0xA4530 / 0x42B2C / 0x42B6C, or wants a new in-combat field for
  DigimonBattle, Enemy or InBattle.
---

# Battle Memory Investigate

Specialization of `memory-compare` for **digimon combat**. Card battles and
NPC win flags live in `memory-compare/patterns/patterns-card-battle.md`.

## Required reading

1. [`../memory-compare/patterns/patterns-battle.md`](../memory-compare/patterns/patterns-battle.md) — index (address map + Blast); then **only** the file of the subject:
   - [`patterns-battle-slots.md`](../memory-compare/patterns/patterns-battle-slots.md) — fixed slots `0xA4470` / `0xA44D0`, HP/MP, buff deltas, status byte, active indexes, `groupId`, drops
   - [`patterns-battle-combatants.md`](../memory-compare/patterns/patterns-battle-combatants.md) — combatant pair `0xA4580` / `0xA45C0`, species, actor/target
   - [`patterns-battle-field.md`](../memory-compare/patterns/patterns-battle-field.md) — field id/potency, effect cluster, Cardmon curse
2. [`../memory-compare/memory-regions.md`](../memory-compare/memory-regions.md) — section *Digimon runtime stats*
3. [`../memory-compare/SKILL.md`](../memory-compare/SKILL.md) — generic compare workflow and output template

## Anchors already integrated

| Address | Meaning | Definitions → Model |
|---------|---------|---------------------|
| `0xA4470` + `n × 0x20` | Ally slot: HP/MP max+current `+0x06..+0x0C`, STR/DEF/SPD delta `+0x10..+0x14`, condition `+0x1C` | `parties/in-battle-addresses.json` → `digimon.inBattle` |
| `0xA44D0` + `n × 0x20` (3 slots) | Enemy slot (same offsets except MP; `Id @ +0x00`) | `battles/enemy-addresses.json` → `digimonBattle.enemy` |
| `0xA446C` | Active enemy slot index (primary active-enemy source) | `enemy-addresses.json` `activeEnemySlotIndex` |
| `0x42B2C` | Encounter group id (Int16) | `enemy-addresses.json` `groupId` |
| `0xA4530` | Battle field id (`0` neutral, `2` Fire … `8` Dark) | `battles/digimon-battle-addresses.json` → `digimonBattle.field` |
| `0x42B74` + `2 × rookieId` | Blast gauge (Int16, 0–1000, per Digimon) | `parties/digimons-addresses.json` `blastAddress` → `digimon.blast` |

## Known, not integrated

- `0xA4580` / `0xA45C0` — combatant attr/resist blocks (stride `0x40`); ally↔enemy base **swaps** — identify by matching slot id, not by base.
- `0xA4468` — active ally slot index.
- `0xA4558` — camera focus unit id (target / ally / stale); removed from `enemy-addresses.json` — do not use to pick the active enemy.
- `0x42B6C` — live drop item Val (drop model still open — see `patterns-battle-slots.md`).
- `0xA4532` — field potency (`1 + value / 128`; item `64`, skill `127`). Field-type skills are bugged: always `A4530 = 6` (Thunder). Field weaken has no observed effect.
- `0xA4414…0xA442A` — field companion cluster; not SSOT.
- `0xE1408` / `0xE141C` — HUD HP mirrors; discard.

## Workflow

1. Confirm both snaps are **in battle** (MapId `0x0600`) and note which ally/enemy is in front.
2. Dump the anchor table above in every snapshot before diffing — most answers are offsets inside known slots.
3. For stat changes, prefer typed commands (`compare-changed`, `chain-match --size 2`) over byte compare.
4. Distinguish **persistent** stats (`digimon-status-addresses.json`, sync after combat) from **live combat** values (slots above).
5. Discard `0x4B610`–`0x4B660` (battle session churn) and encounter cache `0x4B824`–`0x4BB00`.
6. Respond with the `memory-compare` output template; suggest JSON for `in-battle-addresses.json`, `enemy-addresses.json` or `digimon-battle-addresses.json` (relative offsets as hex strings).
7. Retrofeed the matching `patterns-battle-*.md` file, the address map in `patterns-battle.md` (new address or status change) and `memory-regions.md`.

## Domain rules to respect when proposing fields

- Enemy with `Id <= 0` → whole `Enemy = null` (never partial fields).
- `Field = 0` is a valid neutral field, not "empty".
- Blast is per Digimon, never global.

Source: `.cursor/rules/digivice-business.mdc` (Batalhas).

## After confirmation

New scalar on `InBattle` / `Enemy` / `DigimonBattle` → `memory-field-integration`.
