---
name: frontend-memory-tests
description: >-
  Concrete conventions for writing Digivice frontend tests (Vitest) for the
  memory reading layer (src/memory/) — where each test goes, naming,
  FakeSnapshotBuilder for synthetic RAM, SnapshotFixtureLoader for real 2 MiB
  .bin dumps from Tools/MemoryScanner, and the test shape of each layer.
  Use when creating or updating tests under App/tests/, after
  adding a field, entity or quest to the memory layer, or when the user asks
  for test coverage of the memory layer.
---

# Frontend Memory Tests

Rules (scope, 1:1 file mapping, naming, corner cases): `.cursor/rules/digivice-frontend-tests.mdc`.
This skill is the practical how-to. Only the memory reading layer has tests; the rest of the frontend does not.

## Stack

- Vitest (`describe`, `it`, `expect`, `vi`), environment `node`, explicit imports from `vitest`.
- Config: `App/vitest.config.ts` (merges `vite.config.ts`, so `@/`, `import.meta.glob` and JSON imports resolve exactly like the app). Includes only `tests/**/*.spec.ts`.
- Types: `App/tsconfig.vitest.json`, part of `npm run type-check`.
- Run (in `App/`): `npm run test:run` (single run) or `npm run test` (watch). Filter: `npx vitest run tests/helpers`.

## Where each test goes

Mirror `src/` under `tests/`, one spec per production file:

| Production | Test file |
|---|---|
| `src/memory/{folder}/{name}.ts` | `tests/memory/{folder}/{name}.spec.ts` |
| `src/state/stores/use-game-store.ts` (`applyState`/`applyError` only) | `tests/state/stores/use-game-store.spec.ts` |
| Test helper `tests/helpers/{name}.ts` | `tests/helpers/{name}.spec.ts` |

Helpers and fixtures:

| Path | Purpose |
|---|---|
| `tests/helpers/fake-snapshot.builder.ts` | `FakeSnapshotBuilder`: synthetic snapshot |
| `tests/helpers/snapshot-fixture.loader.ts` | `SnapshotFixtureLoader`: real `.bin` dumps |
| `tests/fixtures/snapshots/` | `.bin` fixtures + catalog `README.md` |
| `tests/fixtures/snapshots/{name}.expected.ts` | Expected `State` of a fixture, shared by provider, composer, patcher and loop specs |

`PlayStationRamSize` (2 MiB) is production code: `@/memory/snapshot/snapshot.constant`.

## Synthetic snapshot (`FakeSnapshotBuilder`)

```ts
const snapshot = new MemorySnapshot(
  new FakeSnapshotBuilder()
    .writeInt16(0x0004b3f8, 0x0600)
    .writeInt32(0x00048da0, 1500)
    .writeByte(0x00048d78, 0)
    .build(),
);
```

- Zeroed 2 MiB `ArrayBuffer` by default (custom size via constructor, only for helper-level tests).
- Little-endian. Accepts signed and unsigned ranges: `writeInt16(address, -1)` writes `0xFFFF` (use it for sentinels).
- Throws `RangeError` when the address is outside the snapshot or the value does not fit the size.
- `build()` returns a copy; later writes do not affect built snapshots.

## Real snapshot (`SnapshotFixtureLoader`)

```ts
const snapshot = new MemorySnapshot(SnapshotFixtureLoader.load("field-central-park"));
```

- Reads `tests/fixtures/snapshots/{name}.bin`, validates 2 MiB, returns a standalone `ArrayBuffer`.
- Generate with `Tools/MemoryScanner` (`dotnet run -- snapshot Snapshots/{name}.bin`), copy here and add a row to the catalog in `tests/fixtures/snapshots/README.md`. Uncatalogued fixtures are not allowed.
- Keep fixtures few and purposeful (about 2 MB each in git history).

## Mocking the outside world

- Tauri bridge: `vi.mock("@tauri-apps/api/core", () => ({ invoke: vi.fn() }))` + `vi.mocked(invoke)`. Rust errors reject with `{ code, detail }`. Reference: `tests/memory/snapshot/tauri-snapshot.provider.spec.ts`.
- Definitions: never mock. Specs that need addresses use `AddressesRepository` (real JSONs from `src/data/memory-addresses/`); converter specs build `...Raw` objects inline.

## Test shapes per layer

Reference implementation: Player.

| Layer | Spec | Shape |
|---|---|---|
| Reader | `tests/memory/readers/{x}.reader.spec.ts` | Synthetic addresses + `FakeSnapshotBuilder`; real addresses from `AddressesRepository` written at the definition offsets; signed `0xFFFF`; empty RAM; address outside the snapshot → `MemoryReadError`. Assert the whole resource with `toEqual`. |
| Assembler | `tests/memory/assemblers/{x}.assembler.spec.ts` | Build the resource inline (`createResource(overrides)`); cover every mapping and the sentinel boundaries (`-1`, `0`, `1`). No snapshot. |
| Provider | `tests/memory/providers/{x}.provider.spec.ts` | Real fixture (`SnapshotFixtureLoader`) with values confirmed in game (catalog), plus one synthetic snapshot at the real addresses. |
| Composer | `tests/memory/state.composer.spec.ts` | Real fixture → full `State`; empty RAM → `SaveNotLoadedError`. |
| Validator | `tests/memory/validators/{x}.validator.spec.ts` | `structuredClone` of a fixture's expected model, mutated per case; assert the violation text or `null`. |
| Patcher | `tests/memory/patch/state.patcher.spec.ts` | Target and source from `structuredClone` of the fixture expected values, mutate the source; assert values and preserved references (`toBe`); `vi.spyOn(storeLogger, "warn")` for the ignored transitions. |
| Store | `tests/state/stores/use-game-store.spec.ts` | `setActivePinia(createPinia())` in `beforeEach`; first tick, patch, error and recovery. |
| Loop | `tests/memory/loop/game.loop.spec.ts` | Fake `SnapshotProvider` (`{ read: vi.fn() }`), `vi.useFakeTimers()` + `await vi.advanceTimersByTimeAsync(ms)`; always `stop()` at the end of the test. |

## Corner-case pass (before finishing)

Re-read the production code and add tests for: sentinel boundaries (`-1`, `0`, `1`), `0xFFFF` read as signed, multiple bitmasks (all must match), raw-byte steps (empty bitmasks), empty collections, lookup by id vs list index, short/missing snapshot and missing definition paths.
