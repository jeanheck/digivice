---
name: address-field-backend
description: >-
  Integrates a new memory field into an existing Digivice *Addresses.json
  pipeline on the backend — Player, Digimon/Party, Auction, and similar
  entities. Use when the user asks to add a confirmed RAM address to
  PlayerAddresses.json or another non-quest Addresses.json, wire a new
  scalar/nested field through Reader/Assembler/DTO/Differ, or extend an
  existing entity after memory-compare. Do not use for Quests/** journal
  trackers (use quest-pattern-backend).
---

# Address Field — Backend Integration

Wire a **new field** (or update an existing one) from an `*Addresses.json`
definition through the backend pipeline up to **event generation** (DTO +
Differ). Stops before SignalR / frontend.

**Prerequisite:** address confirmed (see `memory-compare`). User may say:
"add this to PlayerAddresses", "wire PreviousMapId", "integrate this field".

## When NOT to use

| Case | Use instead |
|------|-------------|
| New quest / side quest / legendary / DRI under `Quests/` | `quest-pattern-backend` |
| Frontend DTO → syncer / UI | `address-field-frontend` |
| End-to-end DRI agent | `dri-agent-integrate` |

## Reuse rule (critical)

Add the field to the **existing** entity pipeline. Do **not** create a new
Reader/Resource/Differ/DTO type for a single scalar on an entity that already
has one.

| Layer | Extend in place |
|-------|-----------------|
| JSON | Existing `*Addresses.json` |
| Addresses C# | Matching `*Addresses.cs` (+ nested address objects if needed) |
| Reader / Resource / Domain / Assembler | Same entity types |
| Events | Same `*DTO`, `*Converter`, `*Differ` |

Create new types only when introducing a **brand-new entity** (rare — ask
user first).

## Entity pipeline map

| Entity | Definitions | Addresses C# | Reader | Resource | Domain | Differ / DTO | Event |
|--------|-------------|--------------|--------|----------|--------|--------------|-------|
| Player | `PlayerAddresses.json` | `PlayerAddresses.cs` | `PlayerReader` | `PlayerResource` | `Player` | `PlayerDiffer` / `PlayerDTO` | `PlayerChanged` |
| Digimon | `Parties/DigimonStatusAddresses.json` (+ slot bases in `DigimonsAddresses.json`) | `DigimonStatusAddresses` / `DigimonAddress` | `DigimonReader` | `DigimonResource` | `Digimon` | `DigimonDiffer` / `DigimonDTO` | `PartyChanged` |
| Party | `PartyAddresses.json` | `PartyAddresses` | `PartyReader` | `PartyResource` | `Party` | party differs / DTOs | `PartyChanged` |
| Auction | `AuctionAddresses.json` | `AuctionAddresses` | `AuctionReader` | auction resources | `Auction` | `AuctionDiffer` / `AuctionDTO` | auction events |

Paths root: `Backend/Memory/Definitions/`, `Backend/Memory/Addresses/`,
`Backend/Memory/Readers/`, `Backend/Memory/Resources/`,
`Backend/Domain/`, `Backend/Events/`.

## Type reference (live examples)

Match an existing field of the same RAM shape:

| Pattern | Read | Resource | Domain / Assembler | DTO |
|---------|------|----------|--------------------|-----|
| Map id (Int16 → hex string) | `ReadInt16` | `short?` | `string` via `.ToString("X4")` | `Optional<string>` — e.g. `MapId`, `PreviousMapId` |
| Byte flag / route | `ReadBytes(addr, 1)[0]` | `byte?` | `byte` (`?? 0`) | `Optional<byte>` — e.g. `SeabedRoute`, `MapVariant` |
| Int32 (money, etc.) | `ReadInt32` | `int?` | `int` (`?? 0`) | `Optional<int>` — e.g. `Bits` |
| Nested Digimon offset | base + status offset | as above | as above | Digimon DTO field |

**Naming:** keep domain/DTO names aligned with the JSON key unless an existing
rename exists (`MapId` → DTO `Location`). New fields default to the same name
everywhere (e.g. `PreviousMapId` stays `PreviousMapId` on DTO).

**Mirrors:** do not add duplicate mirror addresses (e.g. `0x48D68` for
PreviousMapId) unless the user explicitly wants both.

## JSON address format

```json
"PreviousMapId": "0x0004B400"
```

C# property must use `[JsonConverter(typeof(HexStringToLongConverter))]`
and type `long` (absolute) or the nested offset convention already used by
that Addresses class.

Nested Digimon-style offsets stay relative hex strings inside objects
(`"Vitals": { "CurrentHP": "0x20" }`) — mirror an adjacent field in the same
JSON object.

---

## Integration checklist

Copy and track. Identify the **entity** and a **reference field** on the same
pipeline first.

### 0. Preconditions

- [ ] Address confirmed; choose primary (not mirror) unless asked otherwise
- [ ] Target `*Addresses.json` and entity pipeline identified
- [ ] Reference field chosen (same read width / domain shape)
- [ ] Confirm **not** a `Quests/**` tracker

### 1. Definitions JSON

- [ ] Add key to the correct `*Addresses.json`
- [ ] Hex string with `0x` prefix; match existing formatting in that file

### 2. Addresses C#

- [ ] Property on matching `*Addresses.cs` (or nested addresses class)
- [ ] `HexStringToLongConverter` (or existing nested converter pattern)

**SKIP** `AddressesRepository` / `IAddressesRepository` when the JSON file is
already loaded (Player, DigimonStatus, Party, Auction). Only touch the
repository when wiring a **new** definitions file.

### 3. Reader + Resource

- [ ] Read in `*Reader` using the same API as the reference field
- [ ] Nullable field on `*Resource` (`short?`, `byte?`, `int?`, …)

### 4. Domain + Assembler

- [ ] Property on domain model
- [ ] Assemble with the same null fallback as the reference field

### 5. Events (DTO + Converter + Differ)

- [ ] `Optional<T>` on `*DTO` with `JsonIgnore(Condition = WhenWritingDefault)`
- [ ] Full map in `*Converter.ToDTO`
- [ ] Delta in `*Differ` when value differs from previous state

### 6. Tests (Backend)

Update every test that constructs the addresses / resource / domain / DTO for
this entity. Add focused cases:

- [ ] Addresses deserialize (repository or serialize round-trip) includes new address
- [ ] Reader maps the new field
- [ ] Assembler maps + null fallback
- [ ] Converter includes field on full DTO
- [ ] Differ: empty when unchanged; delta when only this field changes; full DTO when previous is null
- [ ] Loader integration mock reads the new address (if LoaderIntegrationTestBase uses real JSON)

Follow CODE_RULES: corner cases, no unused usings, primary constructors where
applicable, no `_` prefixes, collection expressions.

### 7. Docs (when field came from investigation)

- [ ] Mark integrated in the relevant `memory-compare/*.md` checklist
- [ ] Note in `memory-regions.md` / `known-patterns.md` that the address is in
  `*Addresses.json`

**Stop.** Do not touch frontend. Hand off to `address-field-frontend`.

---

## Reference walkthrough — Player `SeabedRoute` / `MapId`

Canonical “add Player field” examples:

1. JSON + `PlayerAddresses.cs`
2. `PlayerReader` / `PlayerResource`
3. `Player` / `PlayerAssembler`
4. `PlayerDTO` / `PlayerConverter` / `PlayerDiffer`
5. Tests under `Tests/Unit/Memory|Domain|Events/...` and
   `Tests/Integration/Application/Loaders/PlayerLoaderTests.cs`

For Int16 → `X4` string fields, mirror **`MapId`** (and **`PreviousMapId`**
once wired). For single-byte fields, mirror **`SeabedRoute`**.
