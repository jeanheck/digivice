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

1. [`../memory-compare/patterns/patterns-battle.md`](../memory-compare/patterns/patterns-battle.md) — all confirmed combat layouts and open questions
2. [`../memory-compare/memory-regions.md`](../memory-compare/memory-regions.md) — section *Digimon runtime stats*
3. [`../memory-compare/SKILL.md`](../memory-compare/SKILL.md) — generic compare workflow and output template

## Anchors already integrated

| Address | Meaning | Definitions → Domain |
|---------|---------|----------------------|
| `0xA4470` + `n × 0x20` | Ally slot: HP/MP max+current `+0x06..+0x0C`, STR/DEF/SPD delta `+0x10..+0x14`, condition `+0x1C` | `Parties/InBattleAddresses.json` → `Digimon.InBattle` |
| `0xA44D0` + `n × 0x20` (3 slots) | Enemy slot (same offsets; `Id @ +0x00`) | `Battles/EnemyAddresses.json` → `DigimonBattle.Enemy` |
| `0xA446C` | Active enemy slot index | `EnemyAddresses.json` `ActiveEnemySlotIndex` |
| `0xA4558` | Active unit id (front digievo/token) | `EnemyAddresses.json` `ActiveUnitId` |
| `0x42B2C` | Encounter group id (Int16) | `EnemyAddresses.json` `GroupId` |
| `0xA4530` | Battle field id (`0` neutral, `2` Fire … `8` Dark) | `Battles/DigimonBattleAddresses.json` → `DigimonBattle.Field` |
| `0x42B74` + `2 × rookieId` | Blast gauge (Int16, 0–1000, per Digimon) | `Parties/DigimonsAddresses.json` `BlastAddress` → `Digimon.Blast` |

## Known, not integrated

- `0xA4580` / `0xA45C0` — combatant attr/resist blocks (stride `0x40`); ally↔enemy base **swaps** — identify by matching slot id, not by base.
- `0xA4468` — active ally slot index.
- `0x42B6C` — live drop item Val (drop model still open — see patterns file).
- `0xA4532`, `0xA4414…0xA442A` — field companions; not SSOT.
- `0xE1408` / `0xE141C` — HUD HP mirrors; discard.

## Workflow

1. Confirm both snaps are **in battle** (MapId `0x0600`) and note which ally/enemy is in front.
2. Dump the anchor table above in every snapshot before diffing — most answers are offsets inside known slots.
3. For stat changes, prefer typed commands (`compare-changed`, `chain-match --size 2`) over byte compare.
4. Distinguish **persistent** stats (`DigimonStatusAddresses.json`, sync after combat) from **live combat** values (slots above).
5. Discard `0x4B610`–`0x4B660` (battle session churn) and encounter cache `0x4B824`–`0x4BB00`.
6. Respond with the `memory-compare` output template; suggest JSON for `InBattleAddresses.json`, `EnemyAddresses.json` or `DigimonBattleAddresses.json` (relative offsets as hex strings).
7. Retrofeed `patterns-battle.md` and `memory-regions.md`.

## Domain rules to respect when proposing fields

- Enemy with `Id <= 0` → whole `Enemy = null` (never partial fields).
- `Field = 0` is a valid neutral field, not "empty".
- Blast is per Digimon, never global.

Source: `.cursor/rules/digivice-business.mdc` (Batalhas).

## After confirmation

New scalar on `InBattle` / `Enemy` / `DigimonBattle` → `address-field-backend`
→ `address-field-frontend`.
