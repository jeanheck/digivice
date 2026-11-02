# Item Shop

Shops that sell consumable items. Shared rules (location id, shop id, icon, map
marker, i18n): [SKILL.md](SKILL.md).

```
Task Progress:
- [ ] 1. Resolve consumable item ids
- [ ] 2. item-shop.json entry
- [ ] 3. map.json marker (itemShops)
- [ ] 4. i18n: ui/i18n/{locale}/shop/item-shop.json
```

## 1. Consumable item ids

Search each item name in `App/src/ui/i18n/pt-BR/consumable-item/consumable-item.json`.
Key = id (e.g. "Carga Energia" → 411, "Super Carga" → 414).

Ids are project ids, not RAM item ids. Missing item → register it (confirm the
list with the user first):

- Id: next number after the highest key, in the order given by the user.
- `App/src/data/game/consumable-item.json`: `resaleValue` = `floor(price / 2)`,
  plus `imageName` when not the default icon.
- `ui/i18n/{locale}/consumable-item/consumable-item.json`: `name` (translate
  en-US/es-ES, user confirms later) and `note: ""` until the user provides it.

Item texts come from that file: `note` is the description, `additionalInformation`
the effect explanation (shown only when present and non-empty). Do not invent
texts; the user provides them.

## 2. Shop entry

```json
"asukaBlackMarket": {
  "locationId": "020E",
  "imageName": "Wizardmon",
  "inventoryOrder": "fixed",
  "inventory": [
    { "consumableItemId": "411", "price": 500 }
  ]
}
```

`inventoryOrder` works as in equipment shops (`fixed`: column 1 items, then
column 2; `alphabetical`: sorted by translated name). Ask the user for prices
if missing — never guess.

## Item icons

Consumables use `assets/items/Pill.webp` by default
(`ConsumableItemConverter`). Discs, plugs and fields use `Disk`. For a specific icon, add `imageName` to the item
in `App/src/data/game/consumable-item.json` and the WebP file to `assets/items/`.

## Done criteria

Shop opens from search (label "Loja"), from the map marker and from
"Comprável em" on each sold consumable. Summarize: shop id, location, items
with ids, prices, coordinates used.
