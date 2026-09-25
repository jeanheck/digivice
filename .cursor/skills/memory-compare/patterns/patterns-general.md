# Known Patterns — General

Cross-domain noise, item semantics and validation checklist.

Maintained by the memory-compare skill (retrofeed). Mark `(confirmed)` or `(suspected)`. Append; do not delete without evidence.

---

## Text buffer false positive (confirmed)

Several consecutive bytes become ASCII (`0x20` = space, `0x73` = `s`, etc.).
Not persistent flags — discard.

---

## Encounter cache fingerprint (confirmed)

Repeating ~4-byte pattern in **0x4B824–0x4BB00** with ptr/stage changes.
Session noise after battles or map activity. Always discard for progress flags.

---

## Item possession vs permanent progress (confirmed)

- **Permanent progress** (legendary weapon, quest step): survives reload; bit
  on progress bytes (`~0x4B38x`, `~0x4B3xx`).
- **Current possession** (common item): may clear on sell — e.g. Divine Barrier
  at `0x48EC9`. Test with sell or `intersect-changed`.

## Item quantity vs quest requisite (confirmed)

- **Stack quantity** (how many you carry): one byte per catalog item in
  `0x0004858F+` (sequential by item Val.). TNT Ball = **`0x000485BE`**
  (Val. `0x005A`, code `300485BE`).
- **Quest requisite** (has item for journal): separate byte, raw `!= 0` — e.g.
  TNT Ball requisite in `MainQuestAddresses.json` at **`0x00048E57`** (shared
  with other key items).
- Compare hunting `99 → 98` (`0x63 → 0x62`) must hit the quantity byte; a
  requisite flag at `0x48E57` will not decrement when using one from a stack.

---

---

## Validation checklist

- **Permanent progress**: reload save — flag persists
- **Possession**: sell/drop — flag clears?
- **Isolated compare**: only the target action, minimal side effects
- **Map control**: same MapId in before/after when hunting non-map flags
