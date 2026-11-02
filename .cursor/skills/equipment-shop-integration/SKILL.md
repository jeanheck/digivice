---
name: equipment-shop-integration
description: >-
  Registers a new equipment shop (weapon shop, pawn shop, arsenal) in the
  Digivice Wiki — equipment-shop.json entry, map marker in map.json, item icons
  (imageName) in equipment.json, WebP asset conversion and i18n names. Use when
  the user asks to add a shop that sells equipment, gives a shop name with its
  items/prices/icons, or asks to add items to an existing equipment shop. Do not
  use for card shops (cardShop, separate structure).
---

# Equipment Shop Integration

Equipment shops are **data-only**: one shared type (`equipmentShop`) drives the
shop panel, search, map marker and the "Comprável em" section of the equipment
panel. Adding a shop never requires code changes.

## When NOT to use

| Case | Instead |
|------|---------|
| Card shop (sells cards/boosters) | `card-shop.json` + `cardShops` in `map.json` (separate flow) |
| Shop needs behavior the shared type lacks (stock varying by quest, etc.) | Plan a code change first; ask the user |

## Files touched

| What | File |
|------|------|
| Shop entry | `App/src/data/game/shop/equipment-shop.json` |
| Map marker | `App/src/data/game/map.json` (`equipmentShops` of the location) |
| Item icons | `App/src/data/game/equipment.json` (`imageName`) |
| Shop name | `App/src/ui/i18n/{pt-BR,en-US,es-ES}/shop/equipment-shop.json` |
| Shop icon asset | `App/src/assets/shops/{Name}.webp` |
| Item icon assets | `App/src/assets/weapons/` or `App/src/assets/pawn/` |

## Workflow

```
Task Progress:
- [ ] 1. Resolve ids (location, equipment)
- [ ] 2. Assets (convert PNG to WebP if needed)
- [ ] 3. equipment-shop.json entry
- [ ] 4. imageName in equipment.json
- [ ] 5. map.json marker
- [ ] 6. i18n names (3 locales)
```

### 1. Resolve ids

- **Location id**: search the map name in `App/src/ui/i18n/pt-BR/map.json`
  (e.g. "Torre Zephyr" → `022F`). User spelling may differ slightly; confirm if ambiguous.
- **Equipment ids**: search each item name in
  `App/src/ui/i18n/pt-BR/equipment/equipment.json` (case-insensitive). Key = id.
  If a name is not found or matches more than one id, ask the user — unless the
  id progression settles it: each arsenal sells the next tier of the previous
  arsenal's line (Espada Pequena 92 → Zanden 93 → Crimson 94 → Mighty 95), so
  duplicate names resolve to the id that continues the line (Arco Angelical =
  193 after Besta 192, not 196). State such deductions in the summary.
- **Shop id**: camelCase from the name, location first (e.g. "Arsenal Seiryu" →
  `seiryuArsenal`, "Penhores Asuka" → `asukaPawnshop`).

### 2. Assets

- Shop icon conventions: every arsenal (weapon shop) uses `Gargomon`; every
  pawn shop uses `Gomamon`. Use the user's icon when given.
- Pawn shops may share the exact inventory of another pawn shop (Penhores
  Suzaku = Penhores Asuka). Copy the inventory as-is; items already have icons.
- Shop icons live in `assets/shops/`; item icons in `assets/weapons/` (weapons
  and armor) and `assets/pawn/`. `ImageCatalog.getEquipmentImageUrl` looks in
  `weapons` first, then `pawn`.
- Check the file exists. Names are exact and start with uppercase. Map loose
  user names to real files (e.g. "OneHand1" → `OneHandSword1`, "shield1" → `Shield1`).
- New PNGs must be converted to lossless WebP and the PNG deleted. Run from `App/`:

```bash
node -e "const sharp=require('sharp');const fs=require('fs');const path=require('path');const dir='src/assets/pawn';(async()=>{for(const file of fs.readdirSync(dir).filter(f=>f.toLowerCase().endsWith('.png'))){const source=path.join(dir,file);await sharp(source).webp({lossless:true}).toFile(source.replace(/\.png$/i,'.webp'));fs.unlinkSync(source);}})()"
```

### 3. Shop entry

Add to `equipment-shop.json`:

```json
"seiryuArsenal": {
  "locationId": "022F",
  "imageName": "Gargomon",
  "inventoryOrder": "fixed",
  "inventory": [
    { "equipmentId": "93", "price": 500 }
  ]
}
```

- `inventoryOrder`:
  - `"fixed"`: game order. The inventory renders in two columns; the left one
    gets the first `ceil(n / 2)` items. List column 1 items, then column 2 items.
  - `"alphabetical"`: sorted by translated name at render time; JSON order is irrelevant.
- Ask which order applies if the user did not say (weapon shops have so far
  been `fixed`, pawn shops `alphabetical`).
- Prices are in Bits, integer. Weapon shop stock never varies with the main quest.

### 4. Item icons

For each item without `imageName`, add it right after `"type"` in `equipment.json`:

```json
"93": {
  "type": "oneHandedWeapon",
  "imageName": "OneHandSword1",
```

If the item already has a different `imageName`, ask before overwriting
(icon belongs to the equipment, so it shows the same in every shop).

### 5. Map marker

In the location entry of `map.json`, add (or append to) `equipmentShops`:

```json
"equipmentShops": [
  {
    "id": "seiryuArsenal",
    "coordinates": { "x": 50, "y": 50 },
    "labelPlacement": "below"
  }
]
```

Use the user's coordinates; default `50`/`50` when not given (the user adjusts
later). Two shops on the same coordinates overlap — mention it.

### 6. i18n

Add `equipmentShop.{id}.name` to the three `shop/equipment-shop.json` files.
pt-BR uses the user's name; translate to en-US / es-ES following existing
entries ("Arsenal Asuka" / "Asuka Arsenal" / "Arsenal Asuka").

Only when the user asks for a line break on the map label, add an optional
`mapLabel` next to `name` with `\n` at the break point, per locale (e.g.
`"Loja de Cartas\nSuzaku"` / `"Suzaku\nCard Shop"`). Map markers use
`mapLabel` when present; search and panels keep using `name`.

## Done criteria

- Shop opens from search (label "Loja"), from the map marker and from
  "Comprável em" on each sold item.
- Do not run build/tests (user runs them). Summarize: shop id, location,
  items with ids and icons, coordinates used, any assumption made on names.
