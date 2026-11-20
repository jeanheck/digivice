---
name: memory-field-integration
description: >-
  Adds a new RAM field to an existing Digivice State entity in the TypeScript
  memory layer (App/src/memory/) — Player, Party/Digimon, InBattle,
  DigimonBattle/Enemy, CardBattle, ImportantItems, Auctions, Npcs. Use when the
  user asks to integrate a confirmed address into an existing *-addresses.json,
  read a new scalar or nested value, or expose a new property on an existing
  model after memory-compare. Do not use for journal quests
  (journal-quest-integration) or a brand-new State entity
  (memory-entity-integration).
---

# Memory Field Integration

Adds a field to an **existing** entity of the TS memory layer, from the
definitions JSON to the model in `useGameStore`. Layers and conventions:
`.cursor/rules/digivice-frontend.mdc` (memory section) and
`.cursor/rules/digivice-frontend-sync.mdc`.

**Prerequisite:** address confirmed (skill `memory-compare`).

## When NOT to use

| Case | Use instead |
|------|-------------|
| Quest / side quest / legendary / DRI / Duel Island under `quests/` | `journal-quest-integration` |
| New entity on `State` (new `*-addresses.json`, new `State` property) | `memory-entity-integration` |

## Reuse rule (critical)

Add the field to the **existing** pipeline of the entity. Do **not** create a
new reader/resource/assembler for one value on an entity that already has one.

## Pipeline

```
data/memory-addresses/*.json → *AddressesRaw → *AddressesConverter → *Addresses
  → AddressesRepository → *Provider → *Reader(snapshot) → *Resource
  → *Assembler → model → StateComposer → useGameStore.applyState (StatePatcher)
```

Paths are under `App/src/memory/` unless stated otherwise.

## Entity map

| Entity | Definitions | Addresses / converter | Resource | Reader | Assembler | Model (`src/state/models/`) |
|--------|-------------|-----------------------|----------|--------|-----------|-----------------------|
| Player | `player-addresses.json` | `addresses/player.addresses.ts`, `converters/player-addresses.converter.ts` | `resources/player.resource.ts` | `readers/player.reader.ts` | `assemblers/player.assembler.ts` | `player.ts` |
| Party slots | `party-addresses.json` | `addresses/party.addresses.ts`, `addresses/parties/slot.addresses.ts` + converters | `resources/party.resource.ts`, `resources/parties/digimon-slot.resource.ts` | `readers/party.reader.ts` | `assemblers/party.assembler.ts` | `party/party.ts`, `party/digimon-slot.ts` |
| Digimon (persistent) | `parties/digimon-status-addresses.json` (offsets) + `parties/digimons-addresses.json` (block base and `blastAddress` per rookie) | `addresses/parties/digimon-status.addresses.ts`, `addresses/parties/digimon.addresses.ts`, `addresses/parties/digimons/*` + converters | `resources/parties/digimon.resource.ts`, `resources/parties/digimons/*` | `readers/digimon.reader.ts` (`memoryBlockAddress + offset`) | `assemblers/digimon.assembler.ts` | `party/digimon/*` |
| Digimon in combat | `parties/in-battle-addresses.json` | `addresses/parties/in-battle.addresses.ts`, `converters/parties/in-battle-addresses.converter.ts` | `resources/parties/digimons/in-battle.resource.ts` | `readers/in-battle.reader.ts` | `assemblers/digimon.assembler.ts` | `party/digimon/in-battle.ts` |
| Digimon battle | `battles/digimon-battle-addresses.json` | `addresses/digimon-battle.addresses.ts` + converter | `resources/digimon-battle.resource.ts` | `readers/digimon-battle.reader.ts` | `assemblers/digimon-battle.assembler.ts` | `digimon-battle.ts` |
| Enemy | `battles/enemy-addresses.json` (slot base + offsets) | `addresses/enemy.addresses.ts` + converter | `resources/battles/enemy.resource.ts` | `readers/enemy.reader.ts` (called by `DigimonBattleReader`) | `assemblers/digimon-battle.assembler.ts` | `battle/enemy.ts` |
| Card battle | `card-battle-addresses.json` | `addresses/card-battle.addresses.ts` + converter | `resources/card-battle.resource.ts` | `readers/card-battle.reader.ts` | `assemblers/card-battle.assembler.ts` | `card-battle.ts` |
| Important items | `important-items-addresses.json` | `addresses/important-items.addresses.ts` + converter | `resources/important-items.resource.ts` | `readers/important-items.reader.ts` | `assemblers/important-items.assembler.ts` | `important-items.ts` |
| Auctions | `auction-addresses.json` | `addresses/auctions.addresses.ts` + converter | `resources/auctions.resource.ts` | `readers/auctions.reader.ts` | `assemblers/auctions.assembler.ts` | `auctions.ts` |
| Npcs | `npc-addresses.json` | `addresses/npcs.addresses.ts` + converter | `resources/npcs.resource.ts` | `readers/npcs.reader.ts` | `assemblers/npcs.assembler.ts` | `npcs.ts`, `npc.ts`, `npc-battle.ts` |

`providers/party.provider.ts` passes `PartyAddresses`, `DigimonsAddresses`,
`DigimonStatusAddresses` and `InBattleAddresses` individually to
`PartyReader.read`, each from its own `AddressesRepository` getter. Confirm
exact names by searching an existing field of the entity before editing.

## Type reference (live examples)

Match an existing field with the same RAM shape. `MemorySnapshot` reads signed
little-endian values; resources hold raw `number`s; assemblers apply the rules.

| Pattern | Reader | Model / Assembler | Example |
|---------|--------|-------------------|---------|
| Map id (Int16 → hex) | `snapshot.readInt16(address)` | `string` via `toMapId` (`X4`) | `player.mapId`, `previousMapId` |
| Byte with `0` = none | `snapshot.readByte(address)` | `number \| null` (`0` → `null`) | `seabedRoute`, `mapVariant` |
| Bit flag → bool | `snapshot.readByte(address, bitMask)` | `boolean` (`!== 0`) | `auctions.divineBarrier` |
| Possession byte → bool | `snapshot.readByte(address)` | `boolean` (`!== 0`) | `importantItems.treeBoots` |
| Int32 | `snapshot.readInt32(address)` | `number` | `player.bits` |
| Id with sentinel | `readInt32` / `readInt16` | `number \| null` (`<= 0` → `null`) | `cardBattle.id`, `activeDigievolutionId`, equipments |
| Digimon offset | `readInt16(memoryBlockAddress + offset)` in `DigimonReader` | as above | `level`, `tp`, attributes |

Sentinel rules (`<= 0` → `null`, `field = 0` neutral, enemy `null` as a whole):
`.cursor/rules/digivice-business.mdc`.

**Naming:** definitions keys are camelCase (`"previousMapId"`), with the same name
in the addresses, resource and model properties (`previousMapId`).

**Mirrors:** do not add duplicate mirror addresses unless the user explicitly asks.

## Definitions format

```json
"PreviousMapId": "0x0004B400"
```

Hex string with `0x`, same formatting as the file. Nested offsets (Digimon,
Enemy) stay relative hex strings, mirroring an adjacent field
(`"HP": { "Current": "0x20", "Max": "0x22" }`). `AddressValueConverter`
rejects invalid values when the repository loads the file.

`App/src/data/memory-addresses/` is the single source of the addresses
(`Tools/MemoryScanner` also reads `quests/main-quest-addresses.json` from there).

---

## Integration checklist

### 0. Preconditions

- [ ] Address confirmed; primary (not mirror) chosen
- [ ] Entity row identified in the map above; reference field chosen
- [ ] Not a `quests/**` tracker

### 1. Definitions + addresses

- [ ] Key in the matching `data/memory-addresses/*.json`
- [ ] Property on `*AddressesRaw` (camelCase, `AddressValueRaw`) and `*Addresses` (camelCase, `number`)
- [ ] Map in `*AddressesConverter.convert` with `AddressValueConverter.convert`
- [ ] **SKIP** `AddressesRepository` — every entity file is already loaded

### 2. Reader + resource

- [ ] Property on `*Resource` (`number`, raw value)
- [ ] Read in `*Reader.read` with the same snapshot API as the reference field

### 3. Model + assembler

- [ ] Property on the model in `src/state/models/`
- [ ] Map in `*Assembler.assemble`, applying the sentinel rules if applicable (pure; never mutate the resource)

### 4. Patcher

- [ ] **SKIP** for scalars and nested objects — `StatePatcher` walks the model generically
- [ ] New **list** property → key in `ListKeys` (`memory/patch/state.patcher.ts`) + case in `tests/memory/patch/state.patcher.spec.ts`

### 5. Tests

Conventions: skill `frontend-memory-tests`.

- [ ] Reader spec: synthetic snapshot reads the new address (`FakeSnapshotBuilder`)
- [ ] Assembler spec: mapping + sentinel cases (`-1`, `0`, `1`)
- [ ] `tests/memory/repositories/addresses.repository.spec.ts` — Player addresses are asserted with `toEqual`; other entities only when the test names the field
- [ ] Real fixtures: add the value read from each `.bin` to `tests/fixtures/snapshots/*.expected.ts` (confirm the value with the user against the game) — provider and composer specs compare the full object
- [ ] `npx vitest run` — only when the user asks

### 6. UI (only when the user asks)

Components read the field through `useGameState()`; derived information goes
in `state/stores/selectors/`, composition with static data in presenters.

### 7. Docs retrofeed (when the field came from an investigation)

- [ ] Mark the address as integrated in `.cursor/skills/memory-compare/memory-regions.md` and the relevant `patterns/*.md`
- [ ] Update the investigation doc in `.cursor/docs/investigations/` if one exists

---

## Reference walkthrough — Player `seabedRoute`

1. `data/memory-addresses/player-addresses.json` → `"seabedRoute"`
2. `addresses/player.addresses.ts` (`seabedRoute` in `PlayerAddressesRaw` and `PlayerAddresses`) + `converters/player-addresses.converter.ts`
3. `resources/player.resource.ts` + `readers/player.reader.ts` (`readByte`)
4. `src/state/models/player.ts` + `assemblers/player.assembler.ts` (`0` → `null`)
5. `tests/memory/readers/player.reader.spec.ts`, `tests/memory/assemblers/player.assembler.spec.ts`, `field-central-park.expected.ts`

Int16 → `X4` string: mirror `mapId` / `previousMapId`. Single byte: mirror `seabedRoute`.
