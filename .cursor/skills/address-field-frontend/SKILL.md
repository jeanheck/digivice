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
| Journal / quests / DRI cards / static quest JSON | `quest-pattern-frontend` |
| Backend Reader / Assembler / Differ | `address-field-backend` |
| End-to-end DRI agent | `dri-agent-integrate` |

## Reuse rule (critical)

Extend the **existing** entity DTO / model / converter / syncer. Do **not**
create a parallel syncer or DTO for one new scalar.

| Layer | Extend in place |
|-------|-----------------|
| DTO | `events/dto/{entity}.dto.ts` |
| Model | `models/{entity}.ts` (or nested under `models/`) |
| Events converter | `events/converters/{entity}.converter.ts` |
| Syncer | `stores/syncers/{entity}.syncer.ts` |

## Entity map (common)

| Entity | DTO | Model | Converter | Syncer | Store path |
|--------|-----|-------|-----------|--------|------------|
| Player | `events/dto/player.dto.ts` | `models/player.ts` | `events/converters/player.converter.ts` | `stores/syncers/player.syncer.ts` | `currentState.player` |
| Digimon | party/digimon DTOs under `events/dto/` | digimon model | digimon events converter | digimon/party syncer | party slots |
| Auction | auction DTOs | auction model | auction converter | auction syncer | auctions |

Confirm exact paths by grepping the existing field names on that entity.

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

- [ ] Map in `convert` from `Required<DTO>` → model (InitialState path)

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
| `seabedRouteType` | `seabedRouteType` | `number` |
| `previousMapId` | `previousMapId` | `string` |

Files to mirror when adding another Player field:

1. [`Frontend/src/events/dto/player.dto.ts`](Frontend/src/events/dto/player.dto.ts)
2. [`Frontend/src/models/player.ts`](Frontend/src/models/player.ts)
3. [`Frontend/src/events/converters/player.converter.ts`](Frontend/src/events/converters/player.converter.ts)
4. [`Frontend/src/stores/syncers/player.syncer.ts`](Frontend/src/stores/syncers/player.syncer.ts)
