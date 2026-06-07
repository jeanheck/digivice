# [Title] — memory investigation

Domain: [quest | legendary weapon | DRI agent | player | party | digimon | map | item | auction | other]
Item/Event: [name]
Location: [where in game]

## Gameplay context

- [prerequisites, one-time vs repeatable, what changed between snapshots]

## Confirmed addresses

### 1. [Layer name — e.g. permanent progress / current possession / instance]

Address: 0x________
Bitmask: 0x__ (or raw byte / offset)
Behavior:

- [when it sets / clears]

Suggested read: `(byte & 0x__) != 0`

## Suggested JSON

Path: `Backend/Memory/Definitions/...`

```json

```

## Discarded / secondary

| Address | Reason |
|---------|--------|

## Related (same region)

- 0x... — [purpose]

## Status

- [ ] Confirmed manually
- [ ] Integrated in Definitions JSON
- [ ] Retrofed to memory-regions.md / known-patterns.md
