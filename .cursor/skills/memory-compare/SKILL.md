---
name: memory-compare
description: >-
  Analyzes Digimon World 2003 RAM compare output for any tracker type — quests,
  legendary weapons, DRI agents, player, party, digimon stats, map location,
  inventory, auctions. Use when the user pastes memory diffs, address lists from
  compare, snapshots, bitmask investigation, or Tools/MemoryScanner.
---

# Memory Compare Investigation

Find **Address + BitMask** (or raw-byte) candidates from RAM diffs. Applies to
quests, items, DRI agents, player/party, digimon data, map, and any future
tracker — not quest-only.

## Input policy (token economy)

**Default:** the user pastes compare output. Do **not** run MemoryScanner or
load `.bin` files unless the user explicitly asks or critical data is missing.

Line format:
`0xADDRESS: 0xBB -> 0xAA (+ flag: 0xXX) (- flag: 0xYY)`

Optional: gameplay context; snapshots under `Tools/MemoryScanner/Snapshots/`
(same investigation folder only).

## Workflow

1. **Identify domain** — what changed? (quest step, item obtained, digimon
   level-up, map transition, battle, auction, DRI agent step, etc.)

2. **Parse diffs** — per address: `addedBits = after & ~before`,
   `removedBits = before & ~after`.

3. **Filter noise** — deprioritize addresses listed in
   [memory-regions.md](memory-regions.md) as discard/noise unless the
   investigation targets that domain.

4. **Cross-reference** — scan `Backend/Memory/Definitions/**/*.json` and
   `**/*.txt` notes. Map `address → known purpose`. Check offsets relative to
   known anchors (party slots, digimon blocks, player fields).

5. **Rank candidates** — strongest signals:
   - Single-bit add (`+ flag` is power of two) for progress/possession flags
   - Same byte or block as related mapped data
   - Address in the expected region for the domain (see memory-regions.md)
   - Coherent prior value (sequential bits, expected counters for stats)
   - For acquisitions: prefer `+ flag` without conflicting `- flag`

6. **Respond** using the output template below. **Always suggest JSON** when the
   target maps to a Definitions file (quest, legendary weapon, DRI agent, etc.).

7. **Retrofeed** — after analysis, update project reference files when warranted
   (see Retrofeed section). Do this in the same turn; do not wait for the user to
   ask.

8. **Integration** — do not edit Definitions JSON or backend code unless the
   user explicitly asks beyond investigation.

## Bitmask rules

- Flag set: `(byte & bitMask) != 0`
- One byte, up to 8 independent flags
- Quest JSON: `BitMasks` (array); some files use singular `BitMask`
- Empty `BitMasks` → step read as `byte != 0`
- Multiple `BitMasks` on one step → **all** must be set

## Definitions layout (quests)

All quest-like trackers live under `Backend/Memory/Definitions/Quests/`:

- `MainQuestAddresses.json`
- `SideQuests/`
- `LegendaryWeapons/`
- `DriAgents/`

## Domain hints

| Domain | Definitions / anchors | Typical diff shape |
|--------|----------------------|-------------------|
| Main quest | `Quests/MainQuestAddresses.json` | Bit flip in `0x4B3xx` |
| Side quests | `Quests/SideQuests/` | Bit flip or raw byte |
| Legendary weapons | `Quests/LegendaryWeapons/` | Sequential bit on `0x4B38E` |
| DRI agents | `Quests/DriAgents/` + [dri-agent-hunt.md](dri-agent-hunt.md) | 3 pairs: talk / boss / deliver; see hunt order below |
| Player | `PlayerAddresses.json` (`Bits`, `MapId`, `Name`, `SeabedRoute`, `IsSubmerged`) | MapId byte change on transition; seabed fields on dive/emerge |
| Map / seabed | `PlayerAddresses.json` (`SeabedRoute`, `IsSubmerged`) + [seabed-routing-investigation.md](seabed-routing-investigation.md) | MapId change; `0x48D78` / `0x48D7A` on dive |
| Map subzones / encounters | [map-subzones-investigation.md](map-subzones-investigation.md) | Same MapId, different encounter regions; dump `0x0E2E0` / `0x4DE30` / `0x48D82` |
| Party | `PartyAddresses.json` (slots `0x48DA4+`) | Slot ID bytes |
| Digimon stats | `Parties/DigimonStatusAddresses.json` (offsets) | Multi-byte counters at `~0x494xxx` |
| Common items | `Auctions/*.txt` | Often `0x48ECx`, may clear on sell |
| Auctions | `Auctions/*.txt` | `0x4B370`, `0x4B38A` region |

## DRI agent investigation

When the domain is a DRI agent quest, follow [dri-agent-hunt.md](dri-agent-hunt.md).
Confirmed per-agent tables: [known-patterns.md](known-patterns.md) (DRI agents section).

**Snapshots (six files):** `{rookie}_before/after_dri_agent`,
`{rookie}_before/after_{boss}`, `{rookie}_before/after_gives_dna_to_agent`
under `Tools/MemoryScanner/Snapshots/`.

**Hunt order:**

1. Talk → new bit on shared **`0x4B38C`**
2. Boss → new bit on **`0x4B3B7`** or **`0x4B3B8`**
3. DNA → `0x00 → 0x01` in **`~0x48DBx`** or **`~0x48Fxx`**
4. Deliver → per-agent byte in **`~0x4Axxx`** or **`~0x49xxx`**

Filter encounter cache `0x4B824`–`0x4BB00`. Suggest JSON under
`Quests/DriAgents/{Name}Addresses.json`. For end-to-end wiring after
confirmation, point the user to skill `dri-agent-integrate`.

## Output template

```markdown
## Context
[Event, domain, assumptions]

## Primary candidate
- Address: 0x________
- BitMask: 0x__ (or raw byte / offset note)
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

After each investigation, update skill reference files **without being asked**:

**[memory-regions.md](memory-regions.md)**
- New **noise/discard** address or range (e.g. map-transition churn, battle junk)
- New **confirmed** region or anchor worth reusing
- One line per entry; include brief reason and source context

**[known-patterns.md](known-patterns.md)**
- New recurring pattern (confirmed, not guessed)
- Fill in DRI agent sections when a new agent’s addresses are confirmed
- Mark entries `(confirmed)` vs `(suspected)` when uncertain

Rules:
- Append; do not delete existing entries unless clearly wrong
- Keep entries short
- Do not retrofeed guesses — only patterns supported by this compare or prior Definitions

## MemoryScanner (suggest only — do not run)

Doc: `Tools/MemoryScanner/README.md`. Value sizes: 1=byte, 2=Int16, 4=Int32.

| Command | When to suggest |
|---------|-----------------|
| `compare f1 f2 [--region quest]` | Quest flags (byte, bit analysis) |
| `chain-match f1 f2 ... --values v1,v2,... --size 4` | Counter with known values per snapshot (blast gauge, EXP) |
| `compare-changed f1 f2 newVal --size N [--old-val prev]` | Value changed to X (optionally from A) |
| `compare-delta f1 f2 delta --size N` | Known increment between two snapshots |
| `intersect-changed f1 f2 f3 [--size 1]` | Reversibility (buy → sell, enter → leave) |
| `search-value file val --size N [--region NAME]` | Find all addresses holding a value |
| `analyze-pair before after` | Main quest may be involved |
| `dump file.bin 0xADDR 32` | Inspect bytes around candidate |

## Additional resources

- [memory-regions.md](memory-regions.md) — RAM map, noise, anchors
- [known-patterns.md](known-patterns.md) — confirmed patterns, DRI agent tables
- [dri-agent-hunt.md](dri-agent-hunt.md) — DRI snapshot convention and hunt order
- [investigation-template.md](investigation-template.md) — post-confirmation doc
- [seabed-routing-investigation.md](seabed-routing-investigation.md) — SeabedRoute, IsSubmerged, PreviousMapId
- [map-subzones-investigation.md](map-subzones-investigation.md) — same-MapId encounter regions, Makisha grids
- Sibling skill: `.cursor/skills/dri-agent-integrate/` — end-to-end DRI wiring after addresses confirmed
- Sibling skill: `.cursor/skills/map-subzone-investigate/` — continue subzone investigation
