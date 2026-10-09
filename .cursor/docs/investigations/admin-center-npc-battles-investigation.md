# Admin Center NPC battles — memory investigation

Domain: other (NPC / enemy catalog — `enemy.json`, `npc/npc.json`)
Item/Event: Datamon, patrols, general, soldier, HQ guards, Game Master, Vemmon
Location: Administrative Center and surroundings (`0214`, `0215`, `0217`, `0219`, `021A`, `021C`)

## Gameplay context

- Main quest stretch covering the Administrative Center (main quest window ~43+).
- 31 in-battle snapshots under `Tools/MemoryScanner/Snapshots/` (one per front enemy per fight).
- Source map = `PreviousMapId` @ `0x4B400` (in battle `MapId` is always `0600`).
- Reads: enemy slots `0xA44D0 + n × 0x20` (id `+0x00`, HP max `+0x06`), `GroupId` @ `0x42B2C`,
  combat blocks `0xA4580` / `0xA45C0` (level + attrs + resists + species).

## Results

| Battle | Map | `groupId` | Party (`memoryId` — name, level, HP) |
|--------|-----|-----------|----------------------------------------|
| Datamon (boss) | `021C` | 9 | 141 — Datamon, 34, 1872 |
| Patrol ×3 (admin basement part 1) | `021A` | 192 | 139 — Raremon, 29, 816 · 134 — Cyclonemon, 29, 816 |
| Patrol ×2 (admin floor 1) | `0214` | 189 | 435 — Guardromon(Blue), 29, 816 · 76 — Tankmon, 30, 840 |
| Patrol ×2 (admin floor 2) | `0217` | 189 | same as above |
| Patrol ×3 (basement stairs) | `0215` | 189 | same as above |
| General (admin floor 2) | `0217` | 190 | 435 — Guardromon(Blue) · 76 — Tankmon · 76 — Tankmon |
| Soldier 1 (admin floor 2) | `0217` | 190 | same as General |
| Vemmon | `0217` | 322 | 465 — Vemmon, 28, 792 |
| HQ guards | `0219` | 191 | 334 — Maildramon, 32, 888 |
| Game Master | `0219` | 10 | 399 — Minotarumon(Blue), 33, 912 · 302 — Gargoylemon, 34, 1404 · 437 — Persiamon, ?, 3096 |

All patrols of the same map/group are identical fights (same `groupId`, same party).
General and Soldier 1 share group 190.

## Applied corrections

- `enemy.json` `582` Minotarumon(Blue): `memoryId` 10 → 399, `groupId` → 10.
- `enemy.json` `583` Gargoylemon: `memoryId` 10 → 302, `groupId` → 10.
- `enemy.json` `584` Persiamon: `memoryId` 10 → 437, `groupId` → 10.
- `enemy.json` `585` Vemmon: `memoryId` 10 → 465, `groupId` → 322.
- `enemy.json` `113` Guardromon(Blue): `memoryId` 0 → 435 (`groupId` left `null` — see pending).
- `enemy.json` `126` Tankmon: `memoryId` 0 → 76 (`groupId` left `null` — see pending).
- `npc/npc.json` `gameMaster`: `party.enemyId` (memoryId) → 399 / 302 / 437, `locationId` → `0219`.
- Datamon (`16`): already correct (141 / 9).

## Created entries (second pass)

`npc/npc.json` (party `enemyId` = memoryId / `groupId`):

| NPC | Map | Party |
|-----|-----|-------|
| `trooper1AdminCenterB1F` / `trooper2AdminCenterB1F` / `trooper3AdminCenterB1F` | `021A` | 139/192, 134/192 |
| `adminFloor1Patrol` | `0214` | 435/189, 76/189 |
| `adminFloor2Patrol` | `0217` | 435/189, 76/189 |
| `basementStairsPatrol` | `0215` | 435/189, 76/189 |
| `adminGeneral` | `0217` | 435/190, 76/190, 76/190 |
| `adminSoldier` | `0217` | 435/190, 76/190, 76/190 |
| `headquartersGuards` | `0219` | 334/191 |

`enemy.json` NPC-only entries (`can*: false` → `0`; owner derived from the party via
memoryId + groupId, first owner wins):
`586` Raremon lv29, `587` Cyclonemon lv29, `588` Maildramon lv32,
`589`/`590` Guardromon(Blue)/Tankmon group 189, `591`/`592` Guardromon(Blue)/Tankmon group 190.
`113` / `126` remain the generic entries (`groupId: null`).

Also: names in `i18n/locales/{pt-BR,en-US}/npcs.json` (including `gameMaster`) and
`map.json` `npcs` on `0214`, `0215`, `0217`, `0219`, `021A`.

## Pending (future iteration)

- NPCs `exp` / `dvexp` / `bit` set to `0` (not captured).
- No `mainQuestAvailabilityWindow` (in `npc/npc.json`) on the new NPCs nor on `gameMaster` (the former
  `starts: null` broke `MainQuestAvailabilityWindowRaw`).
- `map.json` coordinates are placeholders (`y: 50`, `x` 30–70).
- Variants `586`–`592` copy `rate`, `dvexp`/`exp`/`bits`, attacks and `drops` from the
  base entry — to review.
### Combat-block stats used for the new variants

Same `memoryId` as the wild version, different level. Resists listed as RAM values.

| Name | `memoryId` | `groupId` | Lv | HP | STR/DEF/SPI/WIS/SPD | Fire/Water/Ice/Wind/Thunder/Machine/Dark | Status resists (poison/paralyze/confuse/sleep/KO) | Species |
|------|-----------:|----------:|---:|---:|---------------------|-------------------------------------------|---------------------------------------------------|---------|
| Raremon | 139 | 192 | 29 | 816 | 340/510/318/340/272 | 103/103/103/60/290/280/290 | 99/0/50/50/99 | ghoul (`0x400`) |
| Cyclonemon | 134 | 192 | 29 | 816 | 289/340/398/425/272 | 103 ×7 | 50/50/50/50/99 | `0x100` (rare) |
| Maildramon | 334 | 191 | 32 | 888 | 371/464/515/371/253 | 265/60/265/100/60/265/100 | 99/0/50/50/50 | machine (`0x500`) |

### Open questions

- Persiamon (`584`) level not confirmed — its combat block was never captured (only the
  catalog copy @ `0xBEE8A`, which has no level). `enemy.json` keeps 38.
- Vemmon (`585`) has no `locations`; fought on `0217`.
- RAM status resists for NPC-only entries (`582`–`585`) are non-zero (e.g. Minotarumon(Blue)
  50/50/0/50/50, Vemmon 99 ×5) but JSON stores `0` with `can*: false`, same as Datamon. Kept.

## Status

- [x] Confirmed manually (snapshots)
- [x] Applied to `enemy.json` / `npc.json` (existing entries only)
- [x] New NPCs and enemy variants added
- [ ] Provisional fields reviewed (exp/dvexp/bit, drops, map coordinates)
