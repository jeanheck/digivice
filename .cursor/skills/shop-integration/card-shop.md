# Card Shop

Shared rules (location id, shop id, icon, map marker, i18n): [SKILL.md](SKILL.md).

```
Task Progress:
- [ ] 1. Resolve card ids
- [ ] 2. card-shop.json entry
- [ ] 3. price in tcg/card.json
- [ ] 4. map.json marker (cardShops)
- [ ] 5. i18n: ui/i18n/{locale}/tcg/card-shop.json
```

## 1. Card ids

Search each card name in `App/src/ui/i18n/pt-BR/tcg/card.json`. Key = id.
Card names are Digimon names (proper nouns); user typos are common
("Stringmon" → `Stingmon` 1081). Ask only if no close match exists.

## 2. Shop entry

The inventory lives inside `phases`, each bound to a main quest window
(`MainQuestAvailabilityWindowRaw`: `starts` = main quest step, optional `ends`):

```json
"byakkoCardShop": {
  "locationId": "025E",
  "imageName": "Divermon",
  "phases": [
    {
      "mainQuestAvailabilityWindow": { "starts": 0 },
      "inventory": ["1208", "1166"]
    }
  ]
}
```

- Default: one phase with `{ "starts": 0 }` (available all game), unless the
  user says the stock changes with the main quest (see `asYellowCruiser`:
  phase `starts 0, ends 27`, then `starts 27`). Mention the default in the summary.
- Keep the user's card order.

## 3. Card price

Add `price` right after `"type"` in `App/src/data/game/tcg/card.json` for each
card without it. A card sold by several shops or phases has a single price
(shared price rule in [SKILL.md](SKILL.md)); a card without `price` is not
rendered in the shop.

## 4–5. Map marker and i18n

- Map key: `cardShops` (same shape as `equipmentShops`).
- i18n key: `cardShop.{id}.name`. In-game, every card shop is named just
  "Loja de Cartas" / "Card Shop" / "Tienda de Cartas" (es translates from en),
  except special ones with their own name ("Cruzeiro Amarelo AS").

## Done criteria

Shop opens from search, from the map marker and from the shop list on each
sold card's panel. Summarize: shop id, location, cards with ids and prices, phases used,
coordinates used, assumptions on names.
