# DRI Agent Hunt

Reference for `memory-compare` when investigating a new DRI agent quest.
Confirmed agent tables live in [known-patterns.md](known-patterns.md).

---

## Snapshot convention

Place under `Tools/MemoryScanner/Snapshots/` (six files per agent):

| Pair | Files | Event |
|------|-------|-------|
| Step 1 | `{rookie}_before_dri_agent.bin` → `{rookie}_after_dri_agent.bin` | Talk to DRI NPC |
| Step 2 | `{rookie}_before_{boss}.bin` → `{rookie}_after_{boss}.bin` | Defeat DNA boss |
| Step 3 | `{rookie}_before_gives_dna_to_agent.bin` → `{rookie}_after_gives_dna_to_agent.bin` | Deliver DNA |

Examples: `monmon_*`, `kumamon_*`. Boss slug lowercase (e.g. `armormon`, `grapleomon`).

Compare each pair with region `quest` (`0x48000`–`0x4D000`).

---

## Hunt order (per pair)

1. **Talk** — new power-of-two bit on shared byte **`0x4B38C`**
2. **Boss** — new bit on **`0x4B3B7`** and/or adjacent **`0x4B3B8`**
3. **DNA** — possession byte `0x00 → 0x01` in **`~0x48DBx`** or **`~0x48Fxx`** (both regions used)
4. **Deliver** — new value on a **per-agent** byte in **`~0x4Axxx`** or **`~0x49xxx`**

Always discard encounter cache **`0x4B824`–`0x4BB00`** (and typical spawn/facing churn near `0x48D6x`–`0x48D84`).

---

## Step 1 — `0x4B38C` bit map

| BitMask | Agent | Status |
|---------|-------|--------|
| `0x01` | Agumon | confirmed |
| `0x02` | Guilmon | confirmed |
| `0x04` | Patamon | confirmed |
| `0x08` | Renamon | confirmed |
| `0x10` | Kotemon | confirmed |
| `0x20` | Kumamon | confirmed |
| `0x40` | Monmon | confirmed |
| `0x80` | Veemon | confirmed |

Do **not** assign free bits without a compare — only confirm from diffs.

---

## Anchor dump (every snapshot)

When comparing, dump these bytes across the timeline:

- `0x4B38C`, `0x4B3B7`, `0x4B3B8`
- Known DNA: `0x48DB6`, `0x48DC3`, `0x48DD2`, `0x48DD3`, `0x48DD6`, `0x48DD7`, `0x48F18`, `0x48F3B`
- Known step3: `0x4A028`, `0x4A7E0`, `0x4A404`, `0x49494`, `0x49870`, `0x49C4C`, `0x4ABBC`, `0x4AF98`

Unchanged known slots are expected when that agent is already complete on the save.

---

## Suggested Definitions shape

```json
{
    "Id": "driAgent{Rookie}",
    "Steps": [
        { "Number": 1, "Address": "0x0004B38C", "BitMasks": ["0x__"] },
        { "Number": 2, "Address": "0x0004B3B7", "BitMasks": ["0x__"] },
        {
            "Number": 3,
            "Address": "0x0004____",
            "BitMasks": ["0x__"],
            "Requisites": [
                { "Id": "{rookie}DDNA", "Address": "0x00048___" }
            ]
        }
    ]
}
```

Path: `Backend/Memory/Definitions/Quests/DriAgents/{Name}Addresses.json`.

After confirmation, integrate via skill `dri-agent-integrate` (or `quest-pattern-backend` then `quest-pattern-frontend`).
