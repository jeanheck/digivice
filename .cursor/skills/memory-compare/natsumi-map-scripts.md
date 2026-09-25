# Natsumi map dump — WSTAG330.PRO

- stage_id: `0x021D` (Digivice locationId `021D`)
- talk_file id: `211` → `estalk02.toml`
- entities: 41, logics: 131
- source: `dmw3-tools/dump/dmw2003/maps.tar` + talk toml
- extracted: 2026-08-27
- helper script: `Tools/MemoryScanner/_extract_natsumi_dump.py`

## Event branch table (main clone — Entity#11 / #13 / #14)

Priority is **first matching Logic** in list order (same pattern on story variants).

| # | Conditions (simplified) | Action / dialogue |
|---|-------------------------|-------------------|
| 0 | `TamerFlag#0` UNSET | Intro “happy high school student” → **SET `TamerFlag#0`** |
| 1 | Met + `Charisma[0]` fail | “Not interested in younger boys” (no battle) |
| 2 | Met + `Charisma[0]` pass + `Charisma[2]` **fail** | **`CardBattle#5`** (normal card) |
| 3 | Met + `Charisma[0]`+`[2]` pass + `BattledTamer#1` **UNSET** | **SET `BattledTamer#1`** + **`ScriptedBattle#0`** (digimon) |
| 4 | Met + high band + digimon done + **lacks Item#18** | “I’ll lose if I duel… pass this time” (no battle) |
| 5 | Met + digimon done + **has Item#18** + `Charisma[4]` fail | “You’re strong… maybe next time” (no battle) |
| 6 | Met + digimon done + has Item#18 + `Charisma[4]` pass | **`StrongerCardBattle#5`** |

Notes from script (important for RAM hunt):

- **Digimon:** script explicitly **SET `BattledTamer#1`** when starting the fight.
- **Normal card (`CardBattle#5`):** script only **starts** the duel — **no** SET of `BattledTamer` / extra tamer bit in this list.
- **Stronger card:** only after digimon done + Item#18 (likely Asuka Trophy / related gate) + higher charisma.
- Transient card outcome UI uses `TamerFlag#16` / `#17` on Entity#12 (win/lose lines), then clears them — not permanent “card completed”.

If in-game rematch of normal card is blocked after a win while still in the `Charisma[2] fail` band, that mark is **not** written by this map script (engine-side / unknown flag) — worth hunting in snapshots.

## How to read

Each **Logic#** is one dialogue branch. Game picks a matching branch
(conditions all true). **scripts** are actions (start battle, set flag).

- `TamerFlag#N SET/UNSET` — intro / met bit
- `BattledTamer#N` — digimon-battle progress bit
- `CardBattle#M` / `StrongerCardBattle#M` — start card duel config M
- `ScriptedBattle#M` — start digimon battle
- `Charisma[i] pass/fail` — threshold from `charisma_reqs[i]`

## Entity#11 sprite=48 pos=(1152,793)


**Entity spawn/requirements:**
- `Complex#3 flag=1`
- `TamerFlag#17 UNSET`
- `Item(128)#402 has`

### Logic#0 — text#46 — Tamer Natsumi

> [name]Tamer Natsumi[name]I'm Natsumi, / a happy high  / school student![pause]

**If (conditions):**
- `TamerFlag#0 UNSET`

**Then (scripts):**
- `TamerFlag#0 SET`

### Logic#1 — text#51 — Tamer Natsumi

> [name]Tamer Natsumi[name]Sorry. I'm not / interested in  / younger boys.[pause]

**If (conditions):**
- `TamerFlag#0 SET`
- `Charisma[0] fail`

**Then:** *(dialogue only)*

### Logic#2 — text#52 — Tamer Natsumi

> [name]Tamer Natsumi[name]Hey, let's / Card Battle![pause][clear]I love it![pause]

**If (conditions):**
- `TamerFlag#0 SET`
- `Charisma[0] pass`
- `Charisma[2] fail`

**Then (scripts):**
- `CardBattle#5`

### Logic#3 — text#53 — Tamer Natsumi

> [name]Tamer Natsumi[name]Hey, let's / Digimon Battle![pause][clear]I'm really good![pause]

**If (conditions):**
- `TamerFlag#0 SET`
- `Charisma[0] pass`
- `Charisma[2] pass`
- `BattledTamer#1 UNSET`

**Then (scripts):**
- `BattledTamer#1 SET`
- `ScriptedBattle#0`

### Logic#4 — text#54 — Tamer Natsumi

> [name]Tamer Natsumi[name]I'll lose if I  / duel with you.[pause][clear]So I'll pass this / time around![pause]

**If (conditions):**
- `TamerFlag#0 SET`
- `Charisma[0] pass`
- `Charisma[2] pass`
- `Item(128)#18 lacks`
- `BattledTamer#1 SET`

**Then:** *(dialogue only)*

### Logic#5 — text#55 — Tamer Natsumi

> [name]Tamer Natsumi[name]You're strong. / I know I won't be / able to win.[pause][clear]So maybe next / time![pause]

**If (conditions):**
- `TamerFlag#0 SET`
- `Charisma[0] pass`
- `Charisma[2] pass`
- `Item(128)#18 has`
- `Charisma[4] fail`
- `BattledTamer#1 SET`

**Then:** *(dialogue only)*

### Logic#6 — text#106 — Tamer Natsumi

> [name]Tamer Natsumi[name]Hey you, you / look pretty / strong![pause][clear]Come and Card / Battle with me / for a while!

**If (conditions):**
- `TamerFlag#0 SET`
- `Charisma[0] pass`
- `Charisma[2] pass`
- `Item(128)#18 has`
- `Charisma[4] pass`
- `BattledTamer#1 SET`

**Then (scripts):**
- `StrongerCardBattle#5`

## Entity#12 sprite=48 pos=(1152,793)

**Entity spawn/requirements:**
- `Complex#9 flag=1`
- `TamerFlag#17 SET`
- `Item(128)#402 has`

### Logic#0 — text#46 — Tamer Natsumi

> [name]Tamer Natsumi[name]I'm Natsumi, / a happy high  / school student![pause]

**If (conditions):**
- `TamerFlag#17 UNSET`

**Then:** *(dialogue only)*

### Logic#1 — text#56 — Tamer Natsumi

> [name]Tamer Natsumi[name]Yeah! I won![pause][clear]I'll play with you / any time![pause]

**If (conditions):**
- `TamerFlag#16 UNSET`
- `TamerFlag#17 SET`

**Then (scripts):**
- `TamerFlag#17 UNSET`

### Logic#2 — text#57 — Tamer Natsumi

> [name]Tamer Natsumi[name]What!  / I lost?![pause][clear]That bites! / I'll beat you / next time![pause]

**If (conditions):**
- `TamerFlag#16 SET`
- `TamerFlag#17 SET`

**Then (scripts):**
- `TamerFlag#17 UNSET`
- `TamerFlag#16 UNSET`

## Entity#13 sprite=48 pos=(1152,793)

**Entity spawn/requirements:**
- `Complex#4 flag=1`
- `TamerFlag#17 UNSET`
- `Item(128)#402 has`

### Logic#0 — text#50 — Tamer Natsumi

> [name]Tamer Natsumi[name]I'm Natsumi! / I wish someone / would play with me.[pause]

**If (conditions):**
- `TamerFlag#0 UNSET`

**Then (scripts):**
- `TamerFlag#0 SET`

### Logic#1 — text#51 — Tamer Natsumi

> [name]Tamer Natsumi[name]Sorry. I'm not / interested in  / younger boys.[pause]

**If (conditions):**
- `TamerFlag#0 SET`
- `Charisma[0] fail`

**Then:** *(dialogue only)*

### Logic#2 — text#52 — Tamer Natsumi

> [name]Tamer Natsumi[name]Hey, let's / Card Battle![pause][clear]I love it![pause]

**If (conditions):**
- `TamerFlag#0 SET`
- `Charisma[0] pass`
- `Charisma[2] fail`

**Then (scripts):**
- `CardBattle#5`

### Logic#3 — text#53 — Tamer Natsumi

> [name]Tamer Natsumi[name]Hey, let's / Digimon Battle![pause][clear]I'm really good![pause]

**If (conditions):**
- `TamerFlag#0 SET`
- `Charisma[2] pass`
- `Charisma[0] pass`
- `BattledTamer#1 UNSET`

**Then (scripts):**
- `ScriptedBattle#0`
- `BattledTamer#1 SET`

### Logic#4 — text#54 — Tamer Natsumi

> [name]Tamer Natsumi[name]I'll lose if I  / duel with you.[pause][clear]So I'll pass this / time around![pause]

**If (conditions):**
- `TamerFlag#0 SET`
- `Charisma[0] pass`
- `Charisma[2] pass`
- `Item(128)#18 lacks`
- `BattledTamer#1 SET`

**Then:** *(dialogue only)*

### Logic#5 — text#55 — Tamer Natsumi

> [name]Tamer Natsumi[name]You're strong. / I know I won't be / able to win.[pause][clear]So maybe next / time![pause]

**If (conditions):**
- `TamerFlag#0 SET`
- `Charisma[0] pass`
- `Charisma[2] pass`
- `Item(128)#18 has`
- `Charisma[4] fail`
- `BattledTamer#1 SET`

**Then:** *(dialogue only)*

### Logic#6 — text#106 — Tamer Natsumi

> [name]Tamer Natsumi[name]Hey you, you / look pretty / strong![pause][clear]Come and Card / Battle with me / for a while!

**If (conditions):**
- `TamerFlag#0 SET`
- `Charisma[0] pass`
- `Charisma[2] pass`
- `Item(128)#18 has`
- `Charisma[4] pass`
- `BattledTamer#1 SET`

**Then (scripts):**
- `StrongerCardBattle#5`

## Entity#14 sprite=48 pos=(1152,793)

**Entity spawn/requirements:**
- `Quest == 38`
- `TamerFlag#17 UNSET`
- `Item(128)#402 has`

### Logic#0 — text#48 — Tamer Natsumi

> [name]Tamer Natsumi[name]I'm Natsumi! / I want a cool / tamer boyfriend.[pause]

**If (conditions):**
- `TamerFlag#0 UNSET`

**Then (scripts):**
- `TamerFlag#0 SET`

### Logic#1 — text#51 — Tamer Natsumi

> [name]Tamer Natsumi[name]Sorry. I'm not / interested in  / younger boys.[pause]

**If (conditions):**
- `TamerFlag#0 SET`
- `Charisma[0] fail`

**Then:** *(dialogue only)*

### Logic#2 — text#52 — Tamer Natsumi

> [name]Tamer Natsumi[name]Hey, let's / Card Battle![pause][clear]I love it![pause]

**If (conditions):**
- `TamerFlag#0 SET`
- `Charisma[0] pass`
- `Charisma[2] fail`

**Then (scripts):**
- `CardBattle#5`

### Logic#3 — text#53 — Tamer Natsumi

> [name]Tamer Natsumi[name]Hey, let's / Digimon Battle![pause][clear]I'm really good![pause]

**If (conditions):**
- `TamerFlag#0 SET`
- `Charisma[2] pass`
- `Charisma[0] pass`
- `BattledTamer#1 UNSET`

**Then (scripts):**
- `ScriptedBattle#0`
- `BattledTamer#1 SET`

### Logic#4 — text#54 — Tamer Natsumi

> [name]Tamer Natsumi[name]I'll lose if I  / duel with you.[pause][clear]So I'll pass this / time around![pause]

**If (conditions):**
- `TamerFlag#0 SET`
- `Charisma[0] pass`
- `Charisma[2] pass`
- `Item(128)#18 lacks`
- `BattledTamer#1 SET`

**Then:** *(dialogue only)*

### Logic#5 — text#55 — Tamer Natsumi

> [name]Tamer Natsumi[name]You're strong. / I know I won't be / able to win.[pause][clear]So maybe next / time![pause]

**If (conditions):**
- `TamerFlag#0 SET`
- `Charisma[0] pass`
- `Charisma[2] pass`
- `Item(128)#18 has`
- `Charisma[4] fail`
- `BattledTamer#1 SET`

**Then:** *(dialogue only)*

### Logic#6 — text#106 — Tamer Natsumi

> [name]Tamer Natsumi[name]Hey you, you / look pretty / strong![pause][clear]Come and Card / Battle with me / for a while!

**If (conditions):**
- `TamerFlag#0 SET`
- `Charisma[0] pass`
- `Charisma[2] pass`
- `Item(128)#18 has`
- `Charisma[4] pass`
- `BattledTamer#1 SET`

**Then (scripts):**
- `StrongerCardBattle#5`

## Entity#15 sprite=48 pos=(1152,793)

**Entity spawn/requirements:**
- `Item(128)#402 lacks`
- `Complex#9 flag=1`
- `Complex#26 flag=0`

### Logic#0 — text#624 — Tamer Natsumi

> [name]Tamer Natsumi[name]I'm Natsumi, / a happy high  / school student![pause]

**If:** *(no conditions)*

**Then:** *(dialogue only)*

## Entity#37 sprite=157 pos=(1152,793)

**Entity spawn/requirements:**
- `Complex#26 flag=1`

### Logic#0 — text#49 — Tamer Natsumi

> [name]Tamer Natsumi[name]Oin, oink! / Oi, oink![pause]

**If (conditions):**
- `TamerFlag#0 UNSET`

**Then:** *(dialogue only)*

### Logic#1 — text#49 — Tamer Natsumi

> [name]Tamer Natsumi[name]Oin, oink! / Oi, oink![pause]

**If (conditions):**
- `TamerFlag#0 SET`
- `Charisma[0] fail`

**Then:** *(dialogue only)*

### Logic#2 — text#49 — Tamer Natsumi

> [name]Tamer Natsumi[name]Oin, oink! / Oi, oink![pause]

**If (conditions):**
- `TamerFlag#0 SET`
- `Charisma[0] pass`
- `Charisma[2] fail`

**Then (scripts):**
- `CardBattle#5`

### Logic#3 — text#49 — Tamer Natsumi

> [name]Tamer Natsumi[name]Oin, oink! / Oi, oink![pause]

**If (conditions):**
- `TamerFlag#0 SET`
- `Charisma[0] pass`
- `Charisma[2] pass`
- `BattledTamer#1 UNSET`

**Then (scripts):**
- `BattledTamer#1 SET`

### Logic#4 — text#49 — Tamer Natsumi

> [name]Tamer Natsumi[name]Oin, oink! / Oi, oink![pause]

**If (conditions):**
- `TamerFlag#0 SET`
- `Charisma[0] pass`
- `Charisma[2] pass`
- `Item(128)#18 lacks`
- `BattledTamer#1 SET`

**Then:** *(dialogue only)*

### Logic#5 — text#49 — Tamer Natsumi

> [name]Tamer Natsumi[name]Oin, oink! / Oi, oink![pause]

**If (conditions):**
- `TamerFlag#0 SET`
- `Charisma[0] pass`
- `Charisma[2] pass`
- `Item(128)#18 has`
- `Charisma[4] fail`
- `BattledTamer#1 SET`

**Then:** *(dialogue only)*

### Logic#6 — text#49 — Tamer Natsumi

> [name]Tamer Natsumi[name]Oin, oink! / Oi, oink![pause]

**If (conditions):**
- `TamerFlag#0 SET`
- `Charisma[0] pass`
- `Charisma[2] pass`
- `Item(128)#18 has`
- `Charisma[4] pass`
- `BattledTamer#1 SET`

**Then (scripts):**
- `StrongerCardBattle#5`

## Index summary (this map, Natsumi entities only)

| Kind | Values seen |
|------|-------------|
| TamerFlag (index, set?) | [(0, 0), (0, 1), (16, 0), (16, 1), (17, 0), (17, 1)] |
| BattledTamer (index, set?) | [(1, 0), (1, 1)] |
| CardBattle # | [5] |
| StrongerCardBattle # | [5] |
| ScriptedBattle # | [0] |

## Confirmed RAM (duel snaps 2026-08-27)

Snaps: `before/after-natsumi-digimon-battle.bin`,
`before/after-natsumi-card-battle.bin` (digimon first, then card at lower CHA).

### Digimon (`BattledTamer#1`)

| Address | BitMask | Evidence |
|---------|---------|----------|
| `0x0004B39A` | `0x02` | `0x00 → 0x02` on digimon win; still `0x02` after later card win |

Wired in `NpcAddresses.json` → `natsumi.DigimonBattles.first`.

Genji digimon remains `0x4B3DF` / `0x20` (`BattledTamer#0` region — different byte than Natsumi `#1`).

### Card (`CardBattle#5`)

No sticky progress bit in `0x4B300–0x4B500`. Quest-band lasting change is
`0x48F19` `0→1` = **Booster 02a** inventory (RA notes), not a duel flag.
`NpcAddresses` card entry for Natsumi cleared until a real flag is found.

**Lose** (`before/after-lose.bin`, digimon bit still clear): `0x48F19` stays `0`;
flag band identical; win∩lose has no sticky quest rematch lock. Logic#2 has no
completion gate — blocked rematch is CHA/dialogue, not a missing flag address.

## Snapshot hunt hints


When comparing RAM after Natsumi events, look for bits that match
script **SET** actions (`TamerFlag#0 SET`, `BattledTamer#1 SET`),
not booster qty (`0x48F19` = Booster 02a per RA notes).

Suggested snap pairs (keep MapId / unrelated noise low):

1. **Intro only:** never met → first talk (expect `TamerFlag#0`)
2. **Digimon:** `BattledTamer#1` UNSET → win digimon (expect bit `#1` in BattledTamer group; Genji digimon was `#0` @ `0x4B3DF` bit `0x20` — Natsumi `#1` may be adjacent bit)
3. **Card win:** digimon already done optional; charisma in normal-card band; before/after card win — hunt sticky bit **not** explained by booster / `TamerFlag#16/#17` transient

See full branch dump above; cross-check live dialogue text against Logic# quotes.

## Live snaps 2026-08-27 (talk only — no digimon fight yet)

Files: `before/after-natsumi-first-talk.bin`,
`before/after-natsumi-digimon-battle.bin` (misnamed: second pair is
**talk at 227 CHA without starting the fight**).

| Pair | Gameplay | Progress-band result |
|------|----------|----------------------|
| first-talk | CHA 12, “never met”; after = “younger boys” | **No** change in `0x4B300–0x4B500`. Only transient `0x48ABC` + map/UI noise |
| digimon-talk | CHA 227; after = user reports intro line again; **no battle** | Again **no** `0x4B3xx` change. `BattledTamer` correctly not set |

All four snaps already have `0x4B3DF = 0x20` (Genji digimon `BattledTamer#0` / known bit).

**Interpretation:** `TamerFlag#0` is **global**. Genji (and Nacky) also use `#0`. With Genji already done on this save, Natsumi’s first talk correctly skipped Logic#0 (intro+SET) and hit Logic#1 (met + low CHA → “younger boys”) — so **no new sticky flag** on A→B is expected.

The re-intro at 227 **without** a `0x4B3xx` flip means either (a) dialogue was not Logic#0 / misremembered vs digimon-offer text, (b) a different entity clone, or (c) `TamerFlag` base is **not** in the Genji digimon byte region (still unmapped). It does **not** look like `BattledTamer#1` yet — that should flip when `ScriptedBattle` actually starts (script SETs `#1` as the fight begins).

Between first-talk and digimon-before the save was heavily edited (many item qty → `0x63`); treat B→C as noise.
