---
name: memory-entity-frontend
description: >-
  Wires a brand-new backend State entity into the Digivice frontend — DTO,
  EventsMap entry, SignalR handler, model, State slot, events converter,
  syncer and Pinia store action. Use when the backend added a new
  {Entity}Changed event / StateDTO property (after memory-entity-backend) and
  the store must receive it. Do not use for a field on an existing entity
  (address-field-frontend) or journal quests (quest-pattern-frontend).
---

# Memory Entity — Frontend

Receive a new entity from SignalR and keep it in `useGameStore`. Reference
implementation: **`card-battle`** (smallest entity). For nested lists, mirror
**`npcs`**. Stops at the store unless the user asks for UI.

**User responsibility:** backend already sends `{entity}` in `InitialState`
and emits `{Entity}Changed`. Do not verify backend.

**No frontend tests** (project rule). Rules: `.cursor/rules/digivice-frontend-sync.mdc`.

## Checklist

Paths under `Frontend/src/`. Property name = backend JSON camelCase (`{entity}`).

### 1. DTO + event map

- [ ] `events/dto/{entity}.dto.ts` — `export interface {Entity}DTO` with **optional** props (`field?: type`); nested DTOs in a kebab subfolder
- [ ] `events/dto/state.dto.ts` — `{entity}: DeepRequired<{Entity}DTO>;` (never null)
- [ ] `events/events.map.ts` — import and `{Entity}Changed: {Entity}DTO;` in `EventsMap` (no DTO reexports; consumers import from `@/events/dto/...`)

### 2. Model

- [ ] `models/{entity}.ts` — required props (nullable where the backend sends `null`)
- [ ] `models/state.ts` — `{entity}: {Entity};` (never null)
- [ ] `models/index.ts` — reexport

### 3. Converter + syncer

- [ ] `events/converters/{entity}.converter.ts` — `static convert(dto: DeepRequired<{Entity}DTO>): {Entity}`; direct pass-through, no fallback (`?? 0`, `=== true`)
- [ ] `stores/syncers/{entity}.syncer.ts` — `static sync(previous, dto)`, one `if (dto.x !== undefined) { previous.x = dto.x; }` per prop (golden rule); nested objects delegate to child syncers

### 4. Store + handler

- [ ] `stores/use-game-store.ts`:
  - `setInitialState` — `{entity}: {Entity}Converter.convert(state.{entity}),`
  - `sync{Entity}(dto)` — `const state = getStateOrWarn("{Entity}Changed"); if (!state) { return; }` then `{Entity}Syncer.sync(state.{entity}, dto)`; add to the returned object
- [ ] `events/signalr.handlers.ts` — `signalRService.on("{Entity}Changed", (data) => { store.sync{Entity}(data); });`

### 5. UI (only if asked)

- [ ] Component reads `useGameState().value.{entity}` and delegates logic to its own presenter (`.cursor/rules/digivice-frontend-data.mdc`)

### 6. Manual verification

- [ ] `InitialState` populates `currentState.{entity}` (Vue devtools)
- [ ] Changing the value in the emulator triggers `{Entity}Changed` and patches the store

### 7. Docs

- [ ] Add the entity row to `address-field-frontend` entity map
