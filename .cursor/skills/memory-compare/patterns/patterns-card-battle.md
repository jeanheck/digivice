# Known Patterns — Tamer and Card Battles

Tamer digimon battles, card battle win counters, card battle screen and config id.

Maintained by the memory-compare skill (retrofeed). Mark `(confirmed)` or `(suspected)`. Append; do not delete without evidence.

---

## Tamer battles (suspected)

Tutorial Genji digimon battle (`genji-before-first` → `genji-after-first`,
MapId `0200`, MQ step 1 still unset):

| Address | BitMask | Evidence | Notes |
|---------|---------|----------|-------|
| `0x0004B3DF` | `0x20` | `0x00 → 0x20` | Same byte as MQ step 44 (`0x10`); bit unused by Definitions. Strongest progress-like signal in quest region. |

`analyze-pair`: no tracked main-quest bits changed. Location data already
lists Genji with `lastMainQuestStepDone: 0`.

Noise in same compare: Monmon EXP `0x49C6C` `0→4`, HP/MP drop, Bits `+0x32`,
spawn/`PreviousMapId`, encounter cache.

Already set pre-Genji (unchanged, not from this fight): `0x4B3DA=0x80`,
`0x4B3DB=0x03`, `0x4B3AC=0x01` — possible earlier tutorial bits (unconfirmed).

### Card battles (confirmed 2026-08-26; counters 2026-08-27)

Asuka card-shop / tutorial card wins share **win counters**, not per-NPC
flags. Order of opponents does **not** matter for the stored value — only
how many wins in that counter’s group.

| Address | Group (observed) | Behavior |
|---------|------------------|----------|
| `0x00048F19` | Natsumi, Wong, Gloria | Increments `+1` per win |
| `0x00048E0B` | Nacky, Steve (also Genji earlier) | Increments `+1` per win |

**Order-independence proof (2026-08-27):** Gloria fought **first** with
`0x48F19 = 0` → after win `0x48F19 = 1` (not `3`). Snaps:
`before/after-defeat-gloria-first.bin`. `0x48E0B` stayed `0`.

Earlier evening chain (order coincidental with counter values):

| Address | Chain |
|---------|-------|
| `0x48F19` | Natsumi `0→1`, Wong `1→2`, Gloria `2→3` |
| `0x48E0B` | Nacky `0→1`, Steve `1→2` (Genji `0→1` earlier) |

**Caveat:** `0x48E0B` was `1` after Natsumi then `0` at `before-nacky` —
not fully sticky across sessions/reloads.

**No unique sticky `0→1` per NPC** in `0x48000–0x4A000` for
Nacky/Wong/Gloria/Steve. Do **not** wire multiple NPCs with BitMask
`0x01` on these counters — cannot express per-tamer identity; need
group membership + threshold (`value >= N` is still wrong for
unordered fights) or true per-NPC flags elsewhere.

Implication for Digivice: a shared counter alone **cannot** mark which
specific tamers are done when fight order is free — only “how many wins
in this group.”

Lose vs win (Natsumi): lasting quest-region difference is still
`0x48F19` (win=`1`, lose=`0`). Transient: `0x48ABC`. Cannot tell
“never fought” vs “lost” from that byte alone.

Snaps: `before/after-*-card-battle.bin` for genji, natsumi, nacky,
wong, gloria, steve; `after-lose-to-natsumi`;
`before/after-defeat-gloria-first.bin`.

Current `NpcAddresses.json`: wire **DigimonBattles** only (e.g. Genji `0x4B3DF`,
Natsumi `0x4B39A`). Card battles are **not** tracked in journal RAM — wiki uses
static `npc.json` + charisma range only.

**Second pass (2026-08-27):** re-scanned all card-win snaps for
per-tamer sticky flags. Quest band `0x48000–0x4C000`: no identity byte
beyond the two counters (Gloria-first vs Natsumi both leave `0x48F19=1`
with no matching per-NPC quest delta). `0x4B3xx` digimon-battle bitfield
region: no card-win bits. Same-on-all-wins only map/session churn
(`0x48D6C–0x48D84`, `0x4BBAC`). `0x44xxx` / `0x7Fxxx` hits look like
card-UI / volatile — e.g. `0x44B71` / `0x44D8B` flip across multiple
tamers and clear on some fights. **No usable per-NPC completion address
found.**

**Natsumi digimon (confirmed 2026-08-27):** `0x0004B39A` BitMask `0x02`
(`0→2` on win; sticky through later card). Matches script
`BattledTamer#1`. See [natsumi-map-scripts.md](../../../docs/investigations/natsumi-map-scripts.md).

**BattledTamer bitfield `0x4B39A+` (confirmed 2026-08-30):** tamers after
Genji tutorial share one sticky bitfield. For `N = groupId − 200` and
`N ≥ 1`: address `0x4B39A + (N ÷ 8)`, BitMask `1 << (N mod 8)`. Genji
first stays isolated at `0x4B3DF` / `0x20` (`BattledTamer#0`). Snaps:
`before/after-{catherine,lucia,robert,akiba,chris,tomomi}.bin`,
`before/after-genji-second.bin`,
`before/after-{mitch,bob,andy,george,meilin,jessica,gordon,alice,nakano}.bin`.

| NPC | groupId | N | Address | BitMask |
|-----|---------|---|---------|---------|
| Genji (1st) | 200 | 0 | `0x4B3DF` | `0x20` |
| Natsumi | 201 | 1 | `0x4B39A` | `0x02` |
| Mitch | 202 | 2 | `0x4B39A` | `0x04` |
| Catherine | 203 | 3 | `0x4B39A` | `0x08` |
| Lucia | 204 | 4 | `0x4B39A` | `0x10` |
| Robert | 205 | 5 | `0x4B39A` | `0x20` |
| Akiba | 206 | 6 | `0x4B39A` | `0x40` |
| Bob | 207 | 7 | `0x4B39A` | `0x80` |
| Tomomi | 208 | 8 | `0x4B39B` | `0x01` |
| Chris | 209 | 9 | `0x4B39B` | `0x02` |
| Andy | 210 | 10 | `0x4B39B` | `0x04` |
| George | 211 | 11 | `0x4B39B` | `0x08` |
| Mei Lin | 212 | 12 | `0x4B39B` | `0x10` |
| Jessica | 213 | 13 | `0x4B39B` | `0x20` |
| Gordon | 214 | 14 | `0x4B39B` | `0x40` |
| Alice | 215 | 15 | `0x4B39B` | `0x80` |
| Nakano | 216 | 16 | `0x4B39C` | `0x01` |

**Genji (2nd digimon):** `0x4B39A` / `0x01` (`0x02→0x03` in
`genji-second` snaps; `0x4B3DF` unchanged). `groupId` 272 — does not
follow `groupId − 200`; treat as special rematch bit until script index
confirmed.

**Backend integrated (2026-08-31):** all confirmed tamers wired in
[`NpcAddresses.json`](../../../../Backend/Memory/Definitions/NpcAddresses.json) —
`genji` (first+second), `natsumi`, `catherine`, `lucia`, `robert`,
`akiba`, `chris`, `tomomi`, `mitch`, `bob`, `andy`, `george`, `meiLin`,
`jessica`, `gordon`, `alice`, `nakano`.

**A.o.A Attacker digimon battle (suspected 2026-09-02):**
`before/after-aoa-attacker.bin` (Secret Room `0255`, MQ steps 60–61
already complete). Current `NpcAddresses.json` entry `0x4B3CA` / `0x20`
matches **MainQuest step 61** only — byte `0xBF` in **both** snaps (bit
already set pre-fight; **no flip on win**). Best sticky quest-band delta:
`0x4B3E5` bit `0x08` (`0x53→0x5B`, single-bit add; same byte as MQ
steps 48/49/60 on other bits). Secondary: `0x48E29` `0→1` (unmapped;
lower confidence — reward/session?). Needs lose control + fresh-save
validation before wiring.

**Natsumi card:** no duel flag in those snaps — only booster `0x48F19`.
Map script does not SET a bit on `CardBattle#5`.

**Lose control (2026-08-27):** `before/after-lose.bin` (Natsumi card lose;
digimon flag `0x4B39A` still `0` on that save). Confirmations:
- `0x48F19` stays `0` on lose (win-only booster).
- Flag band `0x4B390–0x4B3E8` identical before/after — **no** sticky card bit.
- Quest-ish diffs only noise (`0x4B401`, `0x4BBAC/AD`); `0x4DE40`/`0x4DE44`
  `0→1` on lose were already `1` in the win-before snap (session/battle
  markers, not rematch lock).
- Win∩lose sticky same-value `0→N` outside booster: UI/volatile only
  (`0x44C81`, `0x5CD08–0x5CD0B`) — not quest progress.
**Verdict:** rematch lock is **not** a sticky RAM flag in these pairs;
script Logic#2 has no completion gate — blocked rematch is likely CHA
band / dialogue branch, not a missing Digivice address.

**Full card process chain (2026-08-27, CHA 124 constant):**
`antes-falar` → `depois-falar-primeira-vez` → `pos-batalha fala1` →
`pos-batalha fala2` → `duelo+dialogo fim` → `intro ensino medio de novo`.
Win (`0x48F19` `0→1` at post-battle fala1). Digimon bit stays `0`.
- Session markers `0x4DE40`/`0x4DE44`: `0→1` on first talk, **clear at intro
  re-talk** — not rematch lock.
- Transient `0x48ABE` `0→3` during post-battle lines, clears when dialogue ends.
- Sticky quest-ish 0→final: booster; `0x48ABC` `0→1` (set on first talk, still
  set when intro loops — cannot alone explain lock, since card already started
  with it set); `0x4B40C` `1→0` on first talk (stays 0); EXP/map noise.
- `0x4B300–0x4B500`: only `0x4B401` / `0x4B40C` change — **no** new BattledTamer-style bit.
- Intro-loop snap vs post-dialogue-done: **no** new quest progress bit (lock
  already present before the second talk, if it exists at all).

---

## Card battle screen (suspected)

Snapshots: `card-battle-genji.bin`, `card-battle-natsumi.bin` vs digimon-battle
pairs `kuwagamon-genji.bin`, `betamon-natsumi.bin`.

### MapId (confirmed)

- **`0x4B3F8`** (Int16): `0x0700` while in card battle UI (same pattern as
  `0x0600` digimon battle).

### PreviousMapId (confirmed)

- **`0x4B400`** / mirror **`0x4B410`**: world MapId before entering card battle
  (Genji snap `0x0200`, Natsumi snap `0x021D`). Useful when only one card-battle
  NPC exists on that map; **not** enough alone on Yellow Cruiser (`0x0211` × 4 NPCs).

### Card battle config id (confirmed)

On transition **`0x0600 → 0x0700`** (card battle screen), **`0x4B404`** (Int32) holds a
**card battle config id** (which fight/deck setup), **not** a unique NPC id.
**`0`** outside card battle (`0x0600` digimon battle).

Backend: `CardBattle.Id` via [`CardBattleAddresses.json`](../../../../Backend/Memory/Definitions/CardBattleAddresses.json) `Id`.
Frontend static: `cardBattles.first` / `cardBattles.second` → `id` in
`tamer.json` / `duel-island.json` (lookup NPC by scanning those ids).

**Genji (confirmed 2026-09-12):**
| Battle | `0x4B404` / `cardBattles.*.id` | Snaps |
|--------|-------------------------------|-------|
| `first` (CHA 60–209) | `1` | `genji-1..8` mid-battle |
| `second` (CHA ≥378 + Asuka Trophy) | `2` | `genji-new-1..8` mid-battle |

Same MapId `0x0700` / Prev `0x0200` in both series. Other deltas first→second:
`0x4B40C` `1→0`, `0x4B420` `0→13`. Stable across both: `0x4B408` bytes
`01 08 00 3C` (`0x3C`=60 CHA min), `0x4B41C`=`4` (deck level suspected).

**Provisional `first.id` (other NPCs — re-snap `second` TBD):**

| `0x4B404` / `first.id` | Digivice `npcId` | `PreviousMapId` |
|------------------------|------------------|-----------------|
| `3` | nacky | `0x0211` |
| `5` | wong | `0x0211` |
| `7` | steve | `0x0211` |
| `9` | gloria | `0x0211` |
| `11` | natsumi | `0x021D` |
| `31` | divermon1 | Duel Island |
| `29` | divermon2 | Duel Island |
| `27` | divermon3 | Duel Island |
| `25` | divermon4 | Duel Island |
| `23` | divermon5 | Duel Island |
| `21` | kingDivermon | Duel Island |
| `14` | mitch | mid-battle |
| `16` | catherine | mid-battle |
| `18` | lucia | mid-battle |
| `20` | robert | mid-battle |
| `34` | akiba | mid-battle |
| `36` | bob | mid-battle |
| `38` | tomomi | mid-battle |
| `40` | chris | mid-battle |
| `42` | andy | mid-battle |
| `44` | george | mid-battle |
| `46` | meiLin | mid-battle |
| `48` | jessica | mid-battle |
| `50` | gordon | mid-battle |
| `52` | alice | mid-battle |
| `54` | nakano | mid-battle |

Sources: early card-battle / Duel Island / tamer mid-battle snaps (2026-08/09).
All non-Genji `cardBattles.second.id` are **`null`** until re-snap.

**Secondary field `0x4B420` (Int32):** varies by fight (Genji first `0`, second `13`;
other series often `35`). Not the config id.

`PreviousMapId` alone is insufficient on `0x0211` (four NPCs share it).

Card catalog ids (e.g. `1095`, `1005`) were **not** found as Int16 in `0x40000–0x50000`.
RetroAchievements **`0xABD9D`** = deck level (Genji `4`) or heap noise — prefer **`0x4B404`**.

---

