---
name: shop-integration
description: >-
  Registers a new shop in the Digivice Wiki — equipment shops (arsenal, pawn
  shop) in equipment-shop.json and card shops in card-shop.json — with map
  marker in map.json, item icons (imageName) in equipment.json, WebP asset
  conversion and i18n names. Use when the user asks to add a shop (arsenal,
  penhores, loja de cartas), gives a shop name with its items/cards and prices,
  or asks to add items or cards to an existing shop.
---

# Shop Integration

Shops are **data-only**: adding one never requires code changes. Two types:

| Type | Sells | Data | Map key | Details |
|------|-------|------|---------|---------|
| Equipment shop | Equipment | `App/src/data/game/shop/equipment-shop.json` | `equipmentShops` | This file |
| Card shop | Cards | `App/src/data/game/shop/card-shop.json` | `cardShops` | [card-shop.md](card-shop.md) |

Shared rules for both types:

- **Location id**: search the map name in `App/src/ui/i18n/pt-BR/map.json`
  (e.g. "Torre Zephyr" → `022F`, "Cúpula Byakko" → `025E`). User spelling may
  differ slightly; confirm if ambiguous.
- **Shop id**: camelCase, location first (`seiryuArsenal`, `asukaPawnshop`,
  `byakkoCardShop`).
- **Shop icon** (`imageName`, file in `App/src/assets/shops/`): every arsenal
  uses `Gargomon`, every pawn shop `Gomamon`, every card shop `Divermon`.
  Use the user's icon when given.
- **Map marker** in the location entry of `map.json` (add the key or append):

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
  later). Markers on the same coordinates overlap — mention it. Re-read the
  location entry before editing: the user often adjusts coordinates in between.
- **i18n**: pt-BR uses the user's name; translate to en-US, and es-ES from
  en-US, following existing entries. If the user gives no name, follow the
  pattern of the type (card shops: "Loja de Cartas") and state it in the summary.
- **Map label line break**: only when the user asks, add an optional `mapLabel`
  next to `name` with `\n` at the break point, per locale (e.g.
  `"Penhores\nSuzaku"`). Markers use `mapLabel` when present; search and panels
  keep using `name`.
- Do not run build/tests (user runs them).

## Equipment shop workflow

```
Task Progress:
- [ ] 1. Resolve ids (location, equipment)
- [ ] 2. Item icon assets (convert PNG to WebP if needed)
- [ ] 3. equipment-shop.json entry
- [ ] 4. imageName in equipment.json
- [ ] 5. map.json marker (equipmentShops)
- [ ] 6. i18n: ui/i18n/{locale}/shop/equipment-shop.json
```

### 1. Equipment ids

Search each item name in `App/src/ui/i18n/pt-BR/equipment/equipment.json`
(case-insensitive). Key = id. If a name is not found or matches more than one
id, ask the user — unless the id progression settles it: each arsenal sells the
next tier of the previous arsenal's line (Espada Pequena 92 → Zanden 93 →
Crimson 94 → Mighty 95), so duplicates resolve to the id that continues the line
(Arco Angelical = 193 after Besta 192, not 196). Close names also resolve this
way ("Bastão de Madeira" was id 132, first staff). State deductions in the summary.

### 2. Item icon assets

- Item icons live in `assets/weapons/` (weapons and armor) and `assets/pawn/`.
  `ImageCatalog.getEquipmentImageUrl` looks in `weapons` first, then `pawn`.
- Check the file exists. Names are exact and start with uppercase. Map loose
  user names to real files ("OneHand1" → `OneHandSword1`, "head3" → `Head3`;
  plain "Shield" on a shield → `Shield1`).
- New PNGs must be converted to lossless WebP and the PNG deleted. Run from
  `App/` (adjust `dir`):

```bash
node -e "const sharp=require('sharp');const fs=require('fs');const path=require('path');const dir='src/assets/weapons';(async()=>{for(const file of fs.readdirSync(dir).filter(f=>f.toLowerCase().endsWith('.png'))){const source=path.join(dir,file);await sharp(source).webp({lossless:true}).toFile(source.replace(/\.png$/i,'.webp'));fs.unlinkSync(source);}})()"
```

### 3. Shop entry

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
- Arsenals are `fixed`, pawn shops `alphabetical`. Ask for other kinds.
- Pawn shops may share the exact inventory of another pawn shop (Penhores
  Suzaku = Penhores Asuka): copy it as-is; items already have icons.
- Prices are in Bits, integer. Equipment shop stock never varies with the main quest.

### 4. Item icons

For each item without `imageName`, add it right after `"type"` in `equipment.json`:

```json
"93": {
  "type": "oneHandedWeapon",
  "imageName": "OneHandSword1",
```

If the item already has a different `imageName`, ask before overwriting (the
icon belongs to the equipment, so it shows the same in every shop).

### Done criteria

Shop opens from search (label "Loja"), from the map marker and from
"Comprável em" on each sold item. Summarize: shop id, location, items with ids
and icons, coordinates used, assumptions on names.
