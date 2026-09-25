# Natsumi card rematch — investigation plan

Domain: NPC card battle / rematch lock (Natsumi, map `0x021D`)
Status: **completed** (Stages 1–5 done)
Created: 2026-08-27

This file holds **investigation steps only**. Confirmed findings stay in
`known-patterns.md` / `natsumi-map-scripts.md` / `memory-regions.md`.

## Goal

Explain why, at fixed mid charisma (e.g. 124), after a Natsumi card duel
(win or lose), talking again yields the intro line and **no** rematch, even
though map Logic#2 (`CardBattle#5`) has no completion SET in the script.

## Anchor snapshots (chain, CHA 124)

| Id | File | Meaning |
|----|------|---------|
| S0 | `antes-falar-natsumi-primeira-vez.bin` | Before first talk |
| S1 | `depois-falar-natsumi-primeira-vez.bin` | After intro only (met; still on map; **no card yet**) |
| S2 | `imediatamente-apos-batalha-carta-natsumi-acabar-no-meio-da-primeira-fala-do-dialogo.bin` | Post-battle, mid outcome line 1 |
| S3 | `…segunda-fala-do-dialogo.bin` | Post-battle, mid outcome line 2 |
| S4 | `apos-duelo-acabar-e-dialogo-acabar-tambem.bin` | Back on map; duel + outcome dialogue done |
| S5 | `apos-ela-dizer-que-estudante-ensino-medio-novamente.bin` | Talked again → intro loop (blocked rematch) |

Primary diff for stages 1–3: **S1 → S4** (met, not carded → carded), require sticky through **S5**.

Already set aside (not rematch lock): `0x48ABC` (met/talk), `0x48F19` (Booster 02a on win).

## Stages

### Stage 1 — Complex / entity spawn flags

**Status:** completed (2026-08-27)

**Hypothesis:** After the card duel, map `Complex#N` (or related spawn state)
switches which Natsumi entity clone is active (script uses Complex#3 / #4 / #9).
Player then hits a clone/branch that only shows intro (or never offers
`CardBattle`), without a classic “card completed” quest bit.

**Zone:** Unknown — hunt sticky single-bit / `0→N` on **S1→S4==S5**, especially
near quest/flag bands (`0x4B3xx`, story bits near `0x4B40C`), not UI/heap noise.

**Method:**

1. Diff S1 vs S4; keep addresses unchanged S4→S5.
2. Prefer single-bit add/clear or clean `0→N`.
3. Cross-check against Entity spawn conditions in `natsumi-map-scripts.md`.
4. Note reload test as follow-up (not required to close first pass).

**Done when:** Candidates listed (or explicitly none in scanned bands) + stage
marked completed below.

**Result:**

- Dump note: spawn text `Complex#N flag=…` is **not** a free RAM bitfield named
  “Complex”. In dmw3-tools it indexes `complex_steps` (party level / digimon
  lock / quest-range style checks). Card duel is unlikely to flip those inputs
  by itself.
- **Quest / flag band (`0x48000–0x4C000`, `0x4B300–0x4B500`):** after discarding
  met (`0x48ABC`) and booster (`0x48F19`), the **only** sticky S1→S4 (==S5)
  change is **`0x4B401`**: `14 → 07` (clears bit `0x10` at S2). Adjacent to
  `PreviousMapId` (`0x4B400`). **No** single-bit ADD anywhere in
  `0x4B000–0x4B500` on this jump. **No** evidence of a Complex/spawn toggle bit.
- **`0x4B40C`:** already `00` at S1 (cleared on first talk) — not a card/Complex
  switch.
- **`0x4DE70–0x4DE82`:** sticky-ish after S2; looks like card/runtime leftovers,
  not entity spawn (defer to Stages 2–3).
- **`0x50–0x5F` / `0x60–0x9F`:** thousands of “interesting” bit flips at S2
  (heap/graphics). Unusable as Complex candidates without a known base address.
- **`0xE2xx`:** position/facing-style churn after battle return — negative
  control for spawn flags.

**Stage 1 verdict:** Hypothesis **not supported** by current snaps in the
quest/flag bands. No Complex/spawn sticky bit found that explains the intro
loop. Rematch lock (if any) is more likely card-engine state (Stage 2) or
session classification (Stage 3), not a map Complex flip.

---

### Stage 2 — Card engine / collection (opponent #5)

**Status:** completed (2026-08-27)

**Hypothesis:** “Already ran CardBattle#5” lives in the card subsystem (like
in-battle HP vs overworld HP), not in the quest `BattledTamer` band.

**Zone:** Card/deck/collection-like regions; bytes that flip at S1→S2 (outcome /
booster moment) and stick to S5. Avoid `0x44xxx` UI and Digimon battle strip
`0x42Bxx`.

**Needs (ideal):** paired lose chain; optional reload. First pass can use
current win chain + older card snaps as noise filter.

**Result:**

Primary lens: **S1→S2** sticky through **S4 and S5** (outcome / booster
moment). Discarded met `0x48ABC` and booster `0x48F19`. Older
`before/after-*-card-battle` pairs used as cross-noise (lose chain not in
folder for this pass).

- **Inventory `0x48E00–0x49000`:** no sticky change except known booster
  `0x48F19`. Empty for any other card-collection / opponent mark.
- **Quest/flag structured band:** only sticky interesting hit remains
  **`0x4B401`** (`14→07` at S2) — same weak Stage 1 leftover, not a card
  opponent id / bitmask.
- **Bitmask shaped like CardBattle index #5 (`+0x20`):** **zero** sticky hits
  in `0x40000–0x60000` (excl. UI / party / encounter / battle strip).
- **Single-bit ADD sticky in `0x48000–0x50000`:** only `0x48F19` (`+0x01`).
- **`0→1` u8 sticky** in structured `0x40000–0x50000` (excl. noise): **none**
  beyond discarded addresses.
- **u16 `0→1` / `0→5` sticky** in `0x47000–0x50000`: **none**.
- **`0x4DE70–0x4DE82`:** sticky after S2; multi-byte runtime/pointer-shaped —
  candidate for Stage 3 session classification, not a clean “#5 done” flag.
- **`0x5C2D9` `02→05`:** only “value 5” curiosity in mid RAM; surrounding u16
  is `0x02FA→0x0540` (counter/pointer churn), not opponent index storage.
- **Heap `0x50–0x9F`:** tens of thousands of sticky-looking flips; “unique vs
  other tamer pairs” is meaningless here (different heap bases / sessions).
  Not usable without an engine base pointer from Stage 4 / RE.

**Stage 2 verdict:** Hypothesis **not supported** in inventory / quest /
near-quest bands with current snaps. No trackable “CardBattle#5 completed”
address found. Rematch lock is still unexplained by card-collection RAM of
the Digivice-friendly shape. Next: Stage 3 (session vs sticky on leftovers
like `0x4DExx`), and/or new snaps (lose chain, reload, in-duel).

---

### Stage 2b — Shared byte + distinct BitMask per tamer (cross-tamer matrix)

**Status:** completed (2026-08-27)

**Hypothesis:** Card completion uses the main-quest / DRI pattern — one byte,
different `BitMask` per NPC (like `0x4B38C` for DRI agents or `0x4B39A` /
`0x4B3DF` for digimon `BattledTamer`).

**Method:** Compare all six `before/after-*-card-battle.bin` pairs (Genji,
Natsumi, Nacky, Wong, Gloria, Steve). Hunt addresses where **2+ tamers** each
add a **single power-of-two bit**, ideally **different bits** on the same byte.
Regions: `0x4B360–0x4B450`, `0x48000–0x4C000`, `0x47000–0x50000` (excl. party,
encounter cache, UI).

**Result:**

- **Strict DRI-style (single-bit ADD only, no clears):** **zero** hits in
  quest/flag bands.
- **Relaxed (single-bit ADD per tamer, clears allowed):** **zero** hits in
  `0x4B360–0x4B450` and `0x48000–0x4C000`.
- **“Distinct bits, same byte”** (2+ tamers, different `+flag`):
  only **`0x48ABC`**, **`0x48E0B`**, **`0x48F19`** — all already known:
  - `0x48ABC` — met/talk (`+0x01` Genji/Natsumi; not card-per-NPC).
  - `0x48E0B` / `0x48F19` — **shared win counters** (`+0x01` or `+0x02` when
    byte was already non-zero from a prior win in the chain). Not independent
    bitmasks per tamer.
- **`0x4B360–0x4B430` per tamer:**
  - Genji / Natsumi / Nacky: same clears on `0x4B400`, `0x4B40C`; same
    `0x4B401` `02→07` (`+0x05`, not power-of-two). Genji also clears `0x4B420`.
  - **Wong / Gloria / Steve: zero changes** in this band on card win.
- **No address changed by all 6 tamers** in flag or `0x48xxx` bands.
- Wong/Gloria/Steve each have only **3–4** quest-band diffs total (boosters +
  noise); nothing bitmask-shaped for card completion.

**Stage 2b verdict:** Shared-byte **bitmask-per-NPC** model **not found** for
card battles. Card wins either touch **quantity counters** (`0x48E0B`,
`0x48F19`) or **session/map noise** (`0x4B401`, encounter/timer bytes) — not
a `0x4B3xx` bitfield like digimon `BattledTamer`. Digimon remains the only
confirmed per-NPC bitmask pattern in this family (`0x4B3DF` / `0x4B39A`).

---

### Stage 3 — Session vs persistent classification

**Status:** completed (2026-08-27)

**Hypothesis:** Some post-card bytes are battle/session scratch (clear on next
talk or map), others belong in the save. Separating them narrows true progress
flags.

**Method:** Classify seeded candidates + scan `0x48000–0x4C000` on Natsumi chain
S0–S5. Rules: clears S4→S5 back toward S0 = **session**; stable S4==S5 with
S0≠S5 = **persistent**; active only S2–S3 = **UI pulse**; never stable =
**volatile**. Cross-check `before/after-*-card-battle` and
`after-lose-to-natsumi.bin` vs S1.

**Result:**

| Class | Addresses | Notes |
|-------|-----------|-------|
| **Session** (clears S4→S5 → S0) | `0x4DE40`, `0x4DE44` | Set on first talk/card context; **gone** on intro re-talk. Cross-tamer: flip on some wins (Nacky/Wong/Gloria `0→1`), Natsumi win pair `1→0`. Not save progress. |
| **UI pulse** (outcome dialogue only) | `0x48ABE` | `00→03` during S2–S3 post-win lines; **clears S4**. Matches transient card-outcome / `TamerFlag#16/#17` path. Zero change on all six `before/after-*-card` pairs (captured after dialogue). |
| **Reward win-only** | `0x48F19` | Sticky from S2; booster qty. |
| **Volatile / runtime** | `0x4BBAC`, `0x4BBAD`, `0x4DC04`, `0x4DC05` | Encounter/timer; never stable across chain. |
| **Persistent sticky** (not rematch lock) | `0x48ABC` | Met/talk — already known. |
| | `0x4B401`, `0x4B40C` | Map-adjacent noise; `4B40C` clears on first talk and stays `0`. Same `0x4B401` `02→07` on Genji/Natsumi/Nacky card wins. |
| | `0x4DE70–0x4DE82` | Card-runtime block: stabilizes after S2 but **also** mutates on `after-lose-to-natsumi` — battle scratch, not “card completed”. |
| | `0x49C6C`, `0x49CA8`, `0x48DA0` | Party EXP / bits — discard. |
| | `0x42B28`, `0x42B38` | Battle strip near Blast — discard. |
| | `0x00E2E4`, `0x00E320`, `0x00E321` | Map/facing mirrors — discard. |
| **Unchanged** | `0x48E0B`, `0x4B400` | On this chain. |

Quest-band automated scan:
- **Session pattern** (S1→S2 change, S4==S5, S5==S0): **only `0x48ABE`**.
- **Persistent** (S0≠S5, S4==S5, excl. boosters): six addrs above — none are
  plausible card-rematch flags after cross-tamer + lose checks.

**Stage 3 verdict:** Session vs persistent split **does not surface** a hidden
card-completion flag. Session markers (`0x4DE40/44`) **clear** when the intro
loops — so they cannot explain a **persistent** rematch block. UI pulse
(`0x48ABE`) is correctly transient. Remaining persistent bytes are met, reward,
party, map, or card-runtime scratch. **Reload test still not done** (cannot
confirm save vs RAM-only for `0x4DE70+` block), but lose snap already shows
that block tracks battle outcome, not a simple done bit.

---

### Stage 4 — In-duel card scratchpad

**Status:** completed (2026-08-27)

**Hypothesis:** Mid-card RAM holds a scratch value that is copied to a sticky
overworld address when the duel ends.

**Snaps used:**

| Label | File |
|-------|------|
| A (met, map) | `depois-falar-natsumi-primeira-vez.bin` |
| B (in-duel) | `durante-a-batalha.bin` |
| C (map after duel) | `apos-duelo-acabar-e-dialogo-acabar-tambem.bin` |

**Result:**

- **During duel (B):** ~124k filtered bytes differ from A — dominated by heap
  `0x60–0x9F` (card UI/engine). Expected for in-battle scratch.
- **Quest/flag band in-duel:**
  - `0x4B3F8` (MapId): `1D→00` during duel, **restores** at C — not progress.
  - `0x4B401`: `14→02` in duel, `02→07` on return — map-adjacent churn; final
    `07` ≠ met `14`, same weak hit as prior stages.
  - `0x4B40C`: `00→01` **only in duel**, back to `00` at C — session pulse.
  - `0x4B39A`, `0x48F19`: unchanged at B; booster `0x48F19` still `0` until C.
- **Migration B→C:** `0x4DE70–0x4DE82` block fills during duel (`0x4DE70`
  `0C→E4`), **clears to `00`** at C — classic scratch, not save flag.
- **Single-bit SET at B persisting at C** in `0x48000–0x4C000`: **zero** hits.
- **Single-bit SET at B persisting at C** in heap (`0x53xxxx`, `0x07Fxxxx`):
  many — card-engine structures; not mappable to Digivice quest flags without
  RE/base pointer.

**Stage 4 verdict:** Scratch exists **in heap during duel** but **does not
copy** a quest-band bitmask to overworld on exit. Booster and map-adjacent
bytes update **after** returning to map, not mid-duel. **No card-rematch /
card-done flag found.**

## Investigation summary (all stages)

With CHA 124 constant and full snap coverage (intro chain, in-duel, cross-tamer
card wins, lose sample), **no Digivice-trackable address** explains Natsumi
card rematch lock. Card progress is **not** stored like digimon `BattledTamer`
(`0x4B39A`) or DRI/main-quest shared bitfields. Only **win counters / booster**
(`0x48F19`) and **met** (`0x48ABC`) are confirmed stickies. Rematch behaviour
likely script/entity routing or engine state outside mapped save flags — or
requires reload/control snaps not yet taken.

---

### Stage 5 — Low RAM / mirrors (last resort)

**Status:** completed (2026-08-27) — **no new snaps required**

**Hypothesis:** Weak — `0xE2xx` / below `0x40000` are mostly position/facing
after returning from battle; negative control for card-rematch flags.

**Method:** Natsumi chain S0–S5 on `0x00000–0x40000`; known anchors from
`memory-regions.md`; cross-tamer `before/after-*-card-battle` on low RAM;
hunt single-bit sticky S0≠S5, S4==S5.

**Result:**

- **`0x0E2E0`:** facing byte — changes S0→S1 then stable (`03→01`); documented
  as discard for logic/area index.
- **`0x0E2E4–0x0E2E7`, `0x0E318–0x0E337`, `0x0E320–0x0E325`:** map/coord
  mirrors — pulse during card return (S1 clears to `00`, restore by S4/S5).
  Cross-tamer: **all six** card wins touch `0xE2AC/E2B0/E2B1` etc. differently
  per session — classic map-transition noise, not NPC progress.
- **`0x0E1408–0x0E1410`:** HUD HP mirrors — change on card chain; discard per
  `memory-regions.md`.
- **Single-bit sticky S0→S5 in `<0x40000`:** only **`0x0E2D8`**, **`0x0E2E0`**,
  **`0x0E304`** — all are **bit clears** (`-0x02`), facing/map-adjacent, not
  card-completion sets.
- **S1→S2 sticky single-bit (card outcome moment):** only `0x0E321` (+0x01),
  `0x0E337` (+0x20) — map mirror churn; not rematch-shaped.

**Stage 5 verdict:** Low RAM **confirmed negative control**. Nothing below
`0x40000` qualifies as a card-rematch / card-done flag. Investigation without
in-duel snap has exhausted Digivice-friendly bands (quest, inv, bitmask matrix,
session split, low RAM).

## Authorization log

| Stage | Authorized | Ran | Outcome |
|-------|------------|-----|---------|
| 1 | yes (2026-08-27) | yes | No Complex/spawn sticky bit; only weak `0x4B401` |
| 2 | yes (2026-08-27) | yes | No card-engine #5 flag in inv/quest; booster only |
| 2b | yes (2026-08-27) | yes | No shared-byte bitmask per tamer; counters only |
| 3 | yes (2026-08-27) | yes | Session/UI/runtime classified; no card-done flag |
| 4 | yes (2026-08-27) | yes | In-duel scratch in heap; no quest flag migration |
| 5 | yes (2026-08-27) | yes | Low RAM = map/HUD/facing; negative control |
