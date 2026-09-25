---
name: address-field-backend
description: >-
  Integrates a new memory field into an existing Digivice entity pipeline on
  the backend — Player, Party/Digimon, DigimonBattle/Enemy, CardBattle,
  ImportantItems, Auctions, Npcs. Use when the user asks to add a confirmed RAM
  address to PlayerAddresses.json or another non-quest *Addresses.json, wire a
  new scalar/nested field through Reader/Assembler/DTO/Differ, or extend an
  existing entity after memory-compare. Do not use for Quests/** journal
  trackers (use quest-pattern-backend) or brand-new entities (use
  memory-entity-backend).
---

# Address Field — Backend Integration

Canonical memory layers: `.cursor/rules/digivice-backend.mdc`. This skill is the
**how-to** for adding a field to an **existing** entity pipeline, up to event
generation (DTO + Differ). Stops before SignalR / frontend.

**Prerequisite:** address confirmed (skill `memory-compare`).

## When NOT to use

| Case | Use instead |
|------|-------------|
| Quest / side quest / legendary / DRI / Duel Island under `Quests/` | `quest-pattern-backend` |
| New entity on `State` (new `*Addresses.json`, new event) | `memory-entity-backend` |
| Frontend DTO → syncer | `address-field-frontend` |

## Reuse rule (critical)

Add the field to the **existing** entity pipeline. Do **not** create a new
Reader/Resource/Differ/DTO for one scalar on an entity that already has one.

## Entity pipeline map

Paths root: `Backend/Memory/Definitions/`, `Backend/Memory/Addresses/`,
`Backend/Memory/Readers/`, `Backend/Memory/Resources/`,
`Backend/Application/Loaders/`, `Backend/Domain/`, `Backend/Events/`.

| Entity | Definitions | Addresses C# | Loader | Reader(s) | Resource | Domain / Assembler | Converter / Differ / DTO | Event |
|--------|-------------|--------------|--------|-----------|----------|--------------------|--------------------------|-------|
| Player | `PlayerAddresses.json` | `PlayerAddresses` | `PlayerLoader` | `PlayerReader` | `PlayerResource` | `Player` / `PlayerAssembler` | `PlayerConverter` / `PlayerDiffer` / `PlayerDTO` | `PlayerChanged` |
| Party slots | `PartyAddresses.json` | `PartyAddresses`, `Parties/SlotAddresses` | `PartyLoader` | `PartyReader`, `DigimonSlotReader` | `PartyResource`, `Parties/DigimonSlotResource` | `Party` / `PartyAssembler`, `DigimonSlotAssembler` | `PartyConverter` / `PartyDiffer`, `DigimonSlotDiffer` / `PartyDTO`, `DigimonSlotDTO` | `PartyChanged` |
| Digimon (persistent) | `Parties/DigimonStatusAddresses.json` (offsets) + `Parties/DigimonsAddresses.json` (base per rookie, `BlastAddress`) | `Parties/DigimonStatusAddresses`, `Parties/DigimonAddress`, `Parties/Digimons/*Addresses` | `DigimonLoader` | `DigimonReader` (+ `DigievolutionSlotReader`, `StoredDigievolutionReader`) | `Parties/DigimonResource`, `Parties/Digimons/*Resource` | `Parties/Digimon` / `DigimonAssembler` (+ `Digimons/*Assembler`) | `DigimonConverter` / `DigimonDiffer` / `DigimonDTO` (+ `Parties/Digimons/*`) | `PartyChanged` |
| Digimon in combat | `Parties/InBattleAddresses.json` | `Parties/InBattleAddresses` | `DigimonLoader` | `InBattleReader` | `Parties/InBattleResource` | `Parties/Digimons/InBattle` | `InBattleConverter` / `InBattleDiffer` / `InBattleDTO` | `PartyChanged` |
| Digimon battle | `Battles/DigimonBattleAddresses.json` (`Field`) | `DigimonBattleAddresses` | `DigimonBattleLoader` | `DigimonBattleReader` | `DigimonBattleResource` | `DigimonBattle` / `DigimonBattleAssembler` | `DigimonBattleConverter` / `DigimonBattleDiffer` / `DigimonBattleDTO` | `DigimonBattleChanged` |
| Enemy | `Battles/EnemyAddresses.json` (slot base + offsets) | `EnemyAddresses` | `DigimonBattleLoader` | `EnemyReader` (via `DigimonBattleReader`) | `Battles/EnemyResource` | `Battles/Enemy` (inside `DigimonBattle`) | `EnemyConverter` / `EnemyDiffer` / `EnemyDTO` | `DigimonBattleChanged` |
| Card battle | `CardBattleAddresses.json` | `CardBattleAddresses` | `CardBattleLoader` | `CardBattleReader` | `CardBattleResource` | `CardBattle` / `CardBattleAssembler` | `CardBattleConverter` / `CardBattleDiffer` / `CardBattleDTO` | `CardBattleChanged` |
| Important items | `ImportantItemsAddresses.json` | `ImportantItemsAddresses` | `ImportantItemsLoader` | `ImportantItemsReader` | `ImportantItemsResource` | `ImportantItems` / `ImportantItemsAssembler` | `ImportantItemsConverter` / `ImportantItemsDiffer` / `ImportantItemsDTO` | `ImportantItemsChanged` |
| Auctions | `AuctionAddresses.json` | `AuctionsAddresses` (+ `AuctionAddresses` per item: `Address` + `BitMask`) | `AuctionsLoader` | `AuctionsReader` | `AuctionsResource` | `Auctions` / `AuctionsAssembler` | `AuctionsConverter` / `AuctionsDiffer` / `AuctionsDTO` | `AuctionsChanged` |
| Npcs | `NpcAddresses.json` | `NpcsAddresses`, `NpcAddresses`, `NpcBattleAddresses` | `NpcsLoader` | `NpcsReader` | `NpcsResource`, `NpcResource`, `NpcBattleResource` | `Npcs`, `Npc`, `NpcBattle` / `NpcsAssembler` | `NpcsConverter` / `NpcsDiffer`, `NpcDiffer`, `NpcBattleDiffer` / `NpcsDTO`, `Npcs/NpcDTO`, `Npcs/NpcBattleDTO` | `NpcsChanged` |

Confirm exact names by grepping an existing field of the entity before editing.

## Type reference (live examples)

Match an existing field of the same RAM shape. Resources are **non-nullable**;
assemblers map directly (no `?? 0`).

| Pattern | Reader | Resource | Domain / Assembler | DTO | Example |
|---------|--------|----------|--------------------|-----|---------|
| Map id (Int16 → hex) | `memoryReader.ReadInt16(addr)` | `short` | `string` via `.ToString("X4")` | `Optional<string>` | `Player.MapId` (DTO `Location`), `PreviousMapId` |
| Byte value | `memoryReader.ReadByte(addr)` | `byte` | `byte` | `Optional<byte>` | `SeabedRoute`, `MapVariant` |
| Bit flag → bool | `memoryReader.ReadByte(addr, bitMask)` | `byte` | `bool` (`!= 0`) | `Optional<bool>` | `Auctions.DivineBarrier` |
| Possession byte → bool | `memoryReader.ReadByte(addr)` | `byte` | `bool` (`!= 0`) | `Optional<bool>` | `ImportantItems.TreeBoots` |
| Int32 | `memoryReader.ReadInt32(addr)` | `int` | `int` | `Optional<int>` | `Player.Bits` |
| Id with sentinel | `ReadInt32` / `ReadInt16` | `int` / `short` | `int?` (`<= 0` → `null`) | `Optional<int?>` | `CardBattle.Id`, `ActiveDigievolutionId`, equipments |
| Digimon offset | `memoryBlockReader.ReadInt16(offset)` inside `DigimonReader` | `short` | as above | Digimon DTO | `Level`, `TP`, attributes |

Sentinel rules (`<= 0` → `null`, `Field = 0` neutral, Enemy null as a whole):
`.cursor/rules/digivice-business.mdc`.

**Naming:** keep domain/DTO names equal to the JSON key unless a rename exists
(`MapId` → DTO `Location`).

**Mirrors:** do not add duplicate mirror addresses (e.g. `0x48D68` for
PreviousMapId) unless the user explicitly asks.

## JSON address format

```json
"PreviousMapId": "0x0004B400"
```

C# property: `long` with `[JsonConverter(typeof(HexStringToLongConverter))]`.
Nested offsets (Digimon, Enemy) stay relative hex strings — mirror an adjacent
field in the same JSON object (`"HP": { "Current": "0x20", "Max": "0x22" }`).

---

## Integration checklist

### 0. Preconditions

- [ ] Address confirmed; primary (not mirror) chosen
- [ ] Entity row identified in the map above; reference field chosen
- [ ] Not a `Quests/**` tracker

### 1. Definitions + Addresses

- [ ] Key in the correct `*Addresses.json` (hex with `0x`, same formatting as the file)
- [ ] Property on the matching `*Addresses` class
- [ ] **SKIP** `AddressesRepository` — every entity file above is already loaded

### 2. Reader + Resource

- [ ] Read with the same `IMemoryReader` API as the reference field
- [ ] Property on `*Resource` (non-nullable)

### 3. Domain + Assembler

- [ ] Property on the domain model (records — value equality is automatic; if the model overrides `Equals`/`GetHashCode` (e.g. has lists), update them)
- [ ] Map in the assembler, applying sentinel rules if applicable

### 4. Events

- [ ] `Optional<T>` on `*DTO` with `[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]` and `= Optional<T>.Empty`
- [ ] Full map in `*Converter.ToDTO`
- [ ] `if (new.X != previous.X) { dto = dto with { X = new.X }; }` in `*Differ`

### 5. Tests

Conventions: skill `backend-tests`. Update every test that builds the
addresses / resource / domain / DTO of the entity, and add:

- [ ] Reader maps the new field
- [ ] Assembler maps it (+ sentinel/fallback cases)
- [ ] Converter includes it on the full DTO
- [ ] Differ: empty when unchanged; delta when only this field changes; full DTO when previous is null
- [ ] Loader integration test (`Tests/Integration/Application/Loaders/{Entity}LoaderTests.cs`) — uses real JSON via `LoaderIntegrationTestBase`, so mock the new address read
- [ ] `AddressesRepositoryDefinitionsTests` if it asserts the entity's addresses

### 6. Docs retrofeed (when the field came from an investigation)

- [ ] Mark the address as integrated in `.cursor/skills/memory-compare/memory-regions.md` and the relevant `patterns/*.md`
- [ ] Update the investigation doc in `.cursor/docs/investigations/` if one exists

**Stop.** Hand off to `address-field-frontend`.

---

## Reference walkthrough — Player `SeabedRoute`

1. `PlayerAddresses.json` + `PlayerAddresses.cs`
2. `PlayerReader` (`ReadByte`) / `PlayerResource` (`byte`)
3. `Player` / `PlayerAssembler`
4. `PlayerDTO` / `PlayerConverter` / `PlayerDiffer`
5. `Tests/Unit/Memory/Readers/PlayerReaderTests.cs`, `Tests/Unit/Domain/Assemblers/PlayerAssemblerTests.cs`, `Tests/Unit/Events/...`, `Tests/Integration/Application/Loaders/PlayerLoaderTests.cs`

Int16 → `X4` string: mirror `MapId` / `PreviousMapId`. Single byte: mirror `SeabedRoute`.
