---
name: address-field-frontend
description: >-
  Integrates a new memory field into Digivice frontend event sync — DTO,
  model, events converter, and Pinia syncer for Player, Digimon/Party,
  Auction, and similar entities. Use when the user asks to wire a backend
  PlayerDTO/DigimonDTO field to the store, add previousMapId or seabedRoute
  style properties, or consume a new address-field after backend integration.
  Do not use for journal/quest UI (use quest-pattern-frontend).
---

# Address Field — Frontend Integration

Wire a **new field** from SignalR DTOs through the Pinia store. Stops at the
syncer unless the user explicitly asks for UI / presenter consumption.

**User responsibility:** backend already emits the field in `InitialState` and
the entity change event. Do **not** invoke or verify `address-field-backend`.

**No frontend tests** (project rule).

## When NOT to use

| Case | Use instead |
|------|-------------|
| Journal / quests / static quest JSON | `quest-pattern-frontend` |
| Backend Reader / Assembler / Differ | `address-field-backend` |
| New entity / new SignalR event in the store | `memory-entity-frontend` |

## Reuse rule (critical)

Extend the **existing** entity DTO / model / converter / syncer. Do **not**
create a parallel syncer or DTO for one new scalar.

| Layer | Extend in place |
|-------|-----------------|
| DTO | `events/dto/{entity}.dto.ts` |
| Model | `models/{entity}.ts` (or nested under `models/`) |
| Events converter | `events/converters/{entity}.converter.ts` |
| Syncer | `stores/syncers/{entity}.syncer.ts` |

## Entity map

Paths relative to `Frontend/src/`. File suffixes: `events/dto/*.dto.ts`,
`events/converters/*.converter.ts`, `stores/syncers/*.syncer.ts`.

| Entity | DTO | Model | Converter | Syncer | Store path |
|--------|-----|-------|-----------|--------|------------|
| Player | `events/dto/player` | `models/player.ts` | `events/converters/player` | `stores/syncers/player` | `currentState.player` |
| Party | `events/dto/party`, `parties/digimon-slot` | `models/party/party.ts`, `party/digimon-slot.ts` | `party`, `parties/digimon-slot` | `party`, `parties/digimon-slot` | `currentState.party` |
| Digimon | `events/dto/parties/digimon` (+ `parties/digimons/*`) | `models/party/digimon/digimon.ts` (+ siblings) | `parties/digimon` (+ `parties/digimons/*`) | `parties/digimon` (+ `parties/digimons/*`) | `party.slots[n].digimon` |
| Digimon in combat | `parties/digimons/in-battle` | `models/party/digimon/in-battle.ts` | `parties/digimons/in-battle` | `parties/digimons/in-battle` | `digimon.inBattle` |
| Digimon battle | `digimon-battle` | `models/digimon-battle.ts` | `digimon-battle` | `digimon-battle` | `currentState.digimonBattle` |
| Enemy | `battles/enemy` | `models/battle/enemy.ts` | `battles/enemy` | `battles/enemy` | `digimonBattle.enemy` |
| Card battle | `card-battle` | `models/card-battle.ts` | `card-battle` | `card-battle` | `currentState.cardBattle` |
| Important items | `important-items` | `models/important-items.ts` | `important-items` | `important-items` | `currentState.importantItems` |
| Auctions | `auctions` | `models/auctions.ts` | `auctions` | `auctions` | `currentState.auctions` |
| Npcs | `npcs`, `npcs/npc`, `npcs/npc-battle` | `models/npcs.ts`, `npc.ts`, `npc-battle.ts` | `npcs` | `npcs` | `currentState.npcs` |

Confirm exact paths by grepping an existing field of the entity.

## Naming and types

- Files: kebab-case with suffixes (`.dto.ts`, `.converter.ts`, `.syncer.ts`).
- Properties: **camelCase** in TypeScript.
- Prefer **same semantic name** as backend DTO JSON (System.Text.Json camelCase):
  `PreviousMapId` → `previousMapId`, `SeabedRoute` → `seabedRoute`.
- Exception already in codebase: backend domain `MapId` is DTO/frontend
  `location` — do not invent new renames unless the backend DTO already renamed.
- Strings for map ids (`location`, `previousMapId`); numbers for byte/int fields
  (`seabedRoute`, `bits`).

## Golden syncer rule

If the DTO property is `undefined` (omitted / unchanged), the syncer **must
not** write the model field.

```typescript
if (newPlayerDto.previousMapId !== undefined) {
  previousPlayer.previousMapId = newPlayerDto.previousMapId;
}
```

## CODE_RULES (frontend)

- Double quotes for strings; semicolons; imports via `@/` when applicable.
- No automated frontend tests.
- Do not add UI styles or presenters unless asked.

---

## Integration checklist

### 0. Preconditions

- [ ] Backend field name + JSON type known (string / number / boolean)
- [ ] Target entity pipeline identified
- [ ] Confirm **not** a journal/quest field

### 1. DTO

- [ ] Optional property on the events DTO interface (`field?: type`)

### 2. Model

- [ ] Required property on the domain model used by the store

### 3. Events converter

- [ ] Map in `convert` from `DeepRequired<DTO>` → model (InitialState path) — direct pass-through, no fallback (`?? 0`, `=== true`)

### 4. Syncer

- [ ] Guard with `!== undefined` then assign

### 5. UI / presenters (only if user asked)

- [ ] Read from `useGameStore` / computed
- [ ] Pass into existing presenter or component
- [ ] Reuse DW3 classes from `style.css` — no new visual language

**Default stop:** after syncer. Ask before touching Vue templates.

---

## Reference walkthrough — Player seabed + map fields

| Backend DTO | Frontend | Type |
|-------------|----------|------|
| `location` (from `MapId`) | `location` | `string` |
| `seabedRoute` | `seabedRoute` | `number` |
| `mapVariant` | `mapVariant` | `number` |
| `previousMapId` | `previousMapId` | `string` |

Files to mirror when adding another Player field:

1. [`Frontend/src/events/dto/player.dto.ts`](../../../Frontend/src/events/dto/player.dto.ts)
2. [`Frontend/src/models/player.ts`](../../../Frontend/src/models/player.ts)
3. [`Frontend/src/events/converters/player.converter.ts`](../../../Frontend/src/events/converters/player.converter.ts)
4. [`Frontend/src/stores/syncers/player.syncer.ts`](../../../Frontend/src/stores/syncers/player.syncer.ts)
