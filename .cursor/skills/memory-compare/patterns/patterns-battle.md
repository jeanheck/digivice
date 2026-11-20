# Known Patterns — Digimon Battle (index)

Entry point for Digimon battle RAM. Read this index, then **only** the file of
the subject under investigation.

Maintained by the memory-compare skill (retrofeed). Mark `(confirmed)` or `(suspected)`. Append; do not delete without evidence.

| File | Subject |
|------|---------|
| [patterns-battle-slots.md](patterns-battle-slots.md) | Fixed battle slots `0xA4470` (ally/enemy HP/MP, buff deltas, status byte), active slot indexes, focus `0xA4558`, active enemy resolution, strip `0x42B28` (`groupId`, enemy level/HP, drop), enemy drops design |
| [patterns-battle-combatants.md](patterns-battle-combatants.md) | Swappable combatant pair `0xA4580` / `0xA45C0` (attrs, resists, species, actor/target, buffs on the pair), enemy catalog copy |
| [patterns-battle-field.md](patterns-battle-field.md) | Elemental field (id, potency, skill bug, weaken), effect cluster `0xA4414…A442A`, Cardmon “curse” |

Blast gauge (below) stays in this file.

## Address map

| Address | Meaning | Status | Integrated as | Details |
|---------|---------|--------|---------------|---------|
| `0x42B74 + 2 × rookieId` | Blast gauge (Int16, 0–1000, per Digimon) | confirmed | `digimons-addresses.json` `blastAddress` → `digimon.blast` | this file |
| `0x42B28` | Battle counter/flag | suspected | — | slots |
| `0x42B2C` | Encounter `groupId` (Int16) | confirmed | `enemy-addresses.json` `groupId` → `enemy.groupId` | slots |
| `0x42B38` / `0x42B3A` / `0x42B3C` | Enemy level / max HP / MP | confirmed | — | slots |
| `0x42B6C` | Live drop item Val (Int16) | confirmed | — (open design) | slots |
| `0xA4468` | Active ally slot index | confirmed | — | slots |
| `0xA446C` | Active enemy slot index | confirmed | `enemy-addresses.json` `activeEnemySlotIndex` | slots |
| `0xA4470 + n × 0x20` | Ally battle slots (HP/MP, buff deltas, condition) | confirmed | `in-battle-addresses.json` → `digimon.inBattle` | slots |
| `0xA44D0 + n × 0x20` | Enemy battle slots (3, NPC multi-enemy) | confirmed | `enemy-addresses.json` → `digimonBattle.enemy` | slots |
| `0xA4558` | Camera focus unit id (not the actor) | confirmed | — (removed from the reader) | slots |
| `0xA4530` | Active field id | confirmed | `digimon-battle-addresses.json` `field` → `digimonBattle.field` | field |
| `0xA4532` | Field potency | confirmed | — | field |
| `0xA4414…0xA442A` | Field / curse effect cluster | suspected | — | field |
| `0xA38A0…0xA38A3` | Last-action record (not the skill id) | suspected | — | field |
| `0xA4580` / `0xA45C0` | Combatant pair (attrs, resists, species) | confirmed layout | — | combatants |
| variable (heap) | Enemy catalog attr copy | confirmed | — (audit only) | combatants |
| `0xE1408…0xE1424` | HUD HP mirrors | confirmed | — (discard) | slots |

---

## Blast gauge Int16 (confirmed)

**Domain (in-game, validated):** each Digimon has its **own** blast gauge (0–1000).
Not a single global bar. With 3 Digimons in party, each keeps its own value even
out of battle (no HUD display, but values persist per slot).

**Formula:** `address = 0x00042B74 + (2 × rookieId)` — Int16 LE, range 0–1000
(2-byte stride between Digimons; read with `readInt16` in `DigimonReader`).
`rookieId` matches the numeric key in `digimons-addresses.json` (0–7).

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

Use `chain-match --region full` for counter hunts; byte-only `compare`
will not find these (addresses are below quest region `0x48000`). The original
hunts used `--size 4`, which only matched because the next Digimon's gauge was
`0`; prefer `--size 2`.
