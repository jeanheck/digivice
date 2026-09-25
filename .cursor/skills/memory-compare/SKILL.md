---
name: memory-compare
description: >-
  Analyzes Digimon World 2003 RAM compare output for any tracker type — quests,
  legendary weapons, DRI agents, Duel Island, player, party, digimon stats, map
  location, inventory, auctions, NPC and card battles. Use when the user pastes
  memory diffs, address lists from compare, snapshots, bitmask investigation,
  or Tools/MemoryScanner output.
---

# Memory Compare Investigation

Find **Address + BitMask** (or raw-byte / typed value) candidates from RAM
diffs. Domain-specific workflows:

| Domain | Specialist skill |
|--------|------------------|
| Digimon battle (HP/MP slots, attrs, field, enemy, drops) | `battle-memory-investigate` |
| Map subzones / encounter regions | `map-subzone-investigate` |

For heavy analyses, consider delegating to the `memory-investigator` subagent
(`.cursor/agents/memory-investigator.md`) to keep the main context small.

## Input policy (token economy)

**Default:** the user pastes compare output. Do **not** run MemoryScanner or
load `.bin` files unless the user explicitly asks or critical data is missing.

Line format: `0xADDRESS: 0xBB -> 0xAA (+ flag: 0xXX) (- flag: 0xYY)`

Optional: gameplay context; snapshots under `Tools/MemoryScanner/Snapshots/`.

## Reference files — read only what the domain needs

| File | Read when |
|------|-----------|
| [memory-regions.md](memory-regions.md) | Always (RAM map, noise, domain → region) |
| [patterns/patterns-general.md](patterns/patterns-general.md) | Always (noise signatures, possession vs progress, validation) |
| [patterns/patterns-quests.md](patterns/patterns-quests.md) | Main/side quests, legendary weapons, DRI agents, Duel Island |
| [patterns/patterns-battle.md](patterns/patterns-battle.md) | Digimon battle, blast, enemy, field, status |
| [patterns/patterns-card-battle.md](patterns/patterns-card-battle.md) | Tamer battles, card battles, NPC win flags |
| [patterns/patterns-map.md](patterns/patterns-map.md) | MapId, seabed, Mobius Desert, subzones |

Closed / long investigations (evidence logs, not instructions):
`.cursor/docs/investigations/` — seabed routing, Mobius Desert, map subzones,
Natsumi card rematch + map scripts, DRI agent hunt, and the
`investigation-template.md` for new write-ups.

## Workflow

1. **Identify domain** — quest step, item obtained, level-up, map transition, battle, auction, NPC/card battle, etc.
2. **Parse diffs** — per address: `addedBits = after & ~before`, `removedBits = before & ~after`.
3. **Filter noise** — deprioritize addresses listed as noise in `memory-regions.md` / `patterns-general.md` unless the investigation targets that domain.
4. **Cross-reference** — scan `Backend/Memory/Definitions/**/*.json`. Map `address → known purpose`; check offsets relative to known anchors (party slots, digimon blocks, player fields, battle slots).
5. **Rank candidates**:
   - Single-bit add (`+ flag` power of two) for progress/possession flags
   - Same byte or block as related mapped data
   - Address in the expected region for the domain
   - Coherent prior value (sequential bits, expected counters)
   - For acquisitions: `+ flag` without conflicting `- flag`
6. **Respond** with the output template. **Always suggest JSON** when the target maps to a Definitions file.
7. **Retrofeed** in the same turn (see below).
8. **Integration** — do not edit Definitions JSON or backend code unless the user explicitly asks beyond investigation.

## Bitmask rules

- Flag set: `(byte & bitMask) != 0`; one byte holds up to 8 independent flags
- Definitions use `BitMasks` (array) on steps and requisites; empty → `byte != 0`; multiple → **all** must be set
- Some non-quest files use singular `BitMask` objects (`AuctionAddresses.json`, `NpcAddresses.json`)

## Domain hints

| Domain | Definitions / anchors | Typical diff shape |
|--------|----------------------|-------------------|
| Main quest | `Quests/MainQuestAddresses.json` | Bit flip in `0x4B3xx` |
| Side quests | `Quests/SideQuests/` | Bit flip or raw byte |
| Legendary weapons | `Quests/LegendaryWeapons/` | Sequential bit on `0x4B38E` |
| DRI agents (8/8 done) | `Quests/DriAgents/` + `.cursor/docs/investigations/dri-agent-hunt.md` | Talk `0x4B38C` / boss `0x4B3B7`–`B8` / deliver per-agent byte |
| Duel Island | `Quests/DuelIsland/` | Round bits `0x4B3B2` `0x80`, `0x4B3B3` `0x01`–`0x08`; trophies `0x48DC2` / `0x48DC4` |
| Important items | `ImportantItemsAddresses.json` | Possession byte `0 → 1` in `~0x48DBx`–`0x48DCx` |
| Player | `PlayerAddresses.json` (`Bits`, `MapId`, `PreviousMapId`, `SeabedRoute`, `MapVariant`) | MapId / PreviousMapId on transition |
| Map / seabed / Mobius | `PlayerAddresses.json` + `patterns-map.md` | `0x48D78` / `0x48D7A` / `0x4B400` |
| Map subzones | skill `map-subzone-investigate` | Same MapId, different encounter regions |
| Party | `PartyAddresses.json` (slots `0x48DA4+`) | Slot ID bytes |
| Digimon stats | `Parties/DigimonStatusAddresses.json` (offsets) | Multi-byte counters at `~0x494xxx` |
| Digimon battle | `Battles/*.json`, `Parties/InBattleAddresses.json` | skill `battle-memory-investigate` |
| NPC digimon battles | `NpcAddresses.json` | BattledTamer bits `0x4B3DF`, `0x4B39A+` |
| Card battle | `CardBattleAddresses.json` (`0x4B404`) | Config id on MapId `0x0700`; win counters `0x48E0B` / `0x48F19` |
| Auctions | `AuctionAddresses.json` | Bits on `0x4B38A`; story window `0x4B370` |
| Common items | — | Quantity table `0x4858F+`; possession `~0x48ECx` may clear on sell |

## Output template

```markdown
## Context
[Event, domain, assumptions]

## Primary candidate
- Address: 0x________
- BitMask: 0x__ (or raw byte / typed value)
- Evidence: 0x__ → 0x__ (+0x__)
- Why: [cross-ref, region, pattern]

## Discarded
| Address | Reason |
|---------|--------|

## Validation (manual)
- [ ] ...

## Suggested JSON
[matching Definitions path and schema]
```

## Retrofeed (mandatory when applicable)

After each investigation, update reference files **without being asked**:

- [memory-regions.md](memory-regions.md) — new noise/discard range, new confirmed anchor; one line per entry with reason and source
- `patterns/patterns-{domain}.md` — new recurring pattern in the matching domain file; mark `(confirmed)` / `(suspected)`
- Long evidence logs → new file in `.cursor/docs/investigations/` (from `investigation-template.md`), linked from the patterns file

Rules: append; do not delete unless clearly wrong; keep entries short; never
retrofeed guesses.

## MemoryScanner (suggest only — do not run)

Doc: `Tools/MemoryScanner/README.md`. Value sizes: 1=byte, 2=Int16, 4=Int32.

| Command | When to suggest |
|---------|-----------------|
| `compare f1 f2 [--region quest]` | Quest flags (byte, bit analysis) |
| `chain-match f1 f2 ... --values v1,v2,... --size 4` | Counter with known values per snapshot (blast, EXP) |
| `compare-changed f1 f2 newVal --size N [--old-val prev]` | Value changed to X (optionally from A) |
| `compare-delta f1 f2 delta --size N` | Known increment between two snapshots |
| `intersect-changed f1 f2 f3 [--size 1]` | Reversibility (buy → sell, enter → leave) |
| `search-value file val --size N [--region NAME]` | All addresses holding a value |
| `analyze-pair before after` | Main quest may be involved |
| `dump file.bin 0xADDR 32` | Inspect bytes around a candidate |

## Next steps after confirmation

| Target | Skill |
|--------|-------|
| `Quests/**` tracker | `quest-pattern-backend` → `quest-pattern-frontend` |
| Field on an existing entity | `address-field-backend` → `address-field-frontend` |
| New entity on `State` | `memory-entity-backend` → `memory-entity-frontend` |
