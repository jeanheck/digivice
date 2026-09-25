---
name: backend-tests
description: >-
  Concrete conventions for writing Digivice backend tests (xUnit + Moq) — where
  each test file goes, namespaces, naming, IMemoryReader mocks,
  LoaderIntegrationTestBase with real Definitions JSON, Differ/Converter/
  Assembler test shapes and corner cases. Use when creating or updating tests
  under Tests/, after backend integrations, or when the user asks for test
  coverage of a backend class.
---

# Backend Tests

Rules (1:1 file mapping, corner cases): `.cursor/rules/digivice-tests.mdc`.
This skill is the practical how-to. Frontend has **no** tests.

## Stack

- xUnit (`[Fact]`, `[Theory]` + `TheoryData`), Moq, project `Tests/Tests.csproj`
- Run: `dotnet test Tests/Tests.csproj -p:UseAppHost=false` (filter with `--filter "FullyQualifiedName~{Class}Tests"`). If `Backend.dll` is locked, ask the user to stop the running Backend.

## Where each test goes

Mirror the production folder under `Tests/Unit/` (unit) or `Tests/Integration/` (real JSON / infrastructure).

| Production | Test file | Namespace |
|------------|-----------|-----------|
| `Backend/Memory/Readers/{X}Reader.cs` | `Tests/Unit/Memory/Readers/{X}ReaderTests.cs` | `Tests.Memory.Readers` |
| `Backend/Memory/Converters/*.cs` | `Tests/Unit/Memory/Converters/` | `Tests.Memory.Converters` |
| `Backend/Memory/Repositories/AddressesRepository.cs` | `Tests/Unit/Memory/Repositories/AddressesRepositoryTests.cs` (temp files) + `Tests/Integration/Memory/Repositories/AddressesRepositoryDefinitionsTests.cs` (real JSON) | `Tests.Memory.Repositories` / `Tests.Integration.Memory.Repositories` |
| `Backend/Application/Loaders/{X}Loader.cs` | `Tests/Integration/Application/Loaders/{X}LoaderTests.cs` (real JSON); pure composition loaders (e.g. `JournalLoader`) in `Tests/Unit/Application/Loaders/` | `Tests.Integration.Application.Loaders` / `Tests.Application.Loaders` |
| `Backend/Application/Providers/{X}Provider.cs` | `Tests/Unit/Application/{X}ProviderTests.cs` | `Tests.Application` |
| `Backend/Application/StateComposer.cs`, `GameLoopService.cs` | `Tests/Unit/Application/StateComposerTests.cs`, `Tests/Integration/Application/GameLoopServiceTests.cs` | |
| `Backend/Domain/Assemblers/**/{X}Assembler.cs` | `Tests/Unit/Domain/Assemblers/**/{X}AssemblerTests.cs` | `Tests.Domain.Assemblers[.Sub]` |
| `Backend/Domain/Models/*` (equality) | `Tests/Unit/Domain/Models/ModelEqualityTests.cs` (transversal suite) | `Tests.Domain.Models` |
| `Backend/Events/Converters/**/{X}Converter.cs` | `Tests/Unit/Events/Converters/**/{X}ConverterTests.cs` | `Tests.Events.Converters[.Sub]` |
| `Backend/Events/Diffing/**/{X}Differ.cs` | `Tests/Unit/Events/Diffing/**/{X}DifferTests.cs` | `Tests.Events.Diffing[.Sub]` |
| `Backend/Events/Factory/{X}EventFactory.cs` | `Tests/Unit/Events/Factory/{X}EventFactoryTests.cs` | `Tests.Events.Factory` |
| `Backend/Infrastructure/DependencyInjection.cs` | `Tests/Integration/Infrastructure/IoC/DependencyInjectionTests.cs` | `Tests.Integration.Infrastructure.IoC` |

Unit namespaces omit `Unit` (`Tests.Memory.Readers`); integration namespaces keep `Integration`. File-scoped `namespace` first, then `using`s. Follow the namespace already used by sibling files.

## Naming

`{Method}_Should{Outcome}[_When{Condition}]`, e.g.
`Diff_ShouldReturnExplicitNullId_WhenIdBecomesNull`,
`Load_ShouldIntegrateCardBattleAddressesAndReader`.

## Patterns

### Reader (unit)

Build the `*Addresses` inline, mock `IMemoryReader` per address, assert the resource.

```csharp
var memoryReaderMock = new Mock<IMemoryReader>();
memoryReaderMock.Setup(memoryReader => memoryReader.ReadInt32(0x0004B404)).Returns(11);
var result = new CardBattleReader(memoryReaderMock.Object).Read(addresses);
Assert.Equal(11, result.Id);
```

Bit-masked reads use `ReadByte(address, bitMask)`; when the reader calls the
masked overload but the mock only sets raw bytes, use
`LoaderIntegrationTestBase.SetupReadByteBitMaskBridge`.

### Loader (integration)

Inherit `LoaderIntegrationTestBase`; `CreateAddressesRepository()` loads the
**real** `Backend/Memory/Definitions`. Mock only `IMemoryReader` at the real
addresses from the JSON, use real readers. Helpers: `WriteInt16`, `WriteInt32`
for memory blocks. Always add the failure path:

```csharp
memoryReaderMock.Setup(memoryReader => memoryReader.ReadInt32(0x0004B404))
    .Throws(new MemoryReadException(0x0004B404, "Memory session is not connected."));
Assert.Throws<MemoryReadException>(() => cardBattleLoader.Load());
```

Adding a JSON file to a quest folder changes counts in
`AddressesRepositoryDefinitionsTests.RealDefinitions_ShouldLoadEveryQuestFolder`
and in `QuestLoaderTests`.

### Assembler (unit)

Pure `static` call. Cover the sentinel boundaries explicitly: `-1`, `0`, `1`
for `<= 0 → null` rules; `0` / non-zero for `bool` conversions; cascades and
normalizations (`MainQuestAssembler`, `DuelIslandAssembler`) with gaps and
all-done cases.

### Converter (unit)

Full DTO: every property `HasValue` with the domain value (including `null`
values mapped explicitly).

### Differ (unit) — minimum set

1. No changes → empty DTO (all `HasValue == false`)
2. Previous `null` → full DTO
3. Only field X changed → only X has value
4. Value → `null` (when nullable) → `HasValue == true` and `Value == null`
5. Nested entities: child delta propagates; unchanged children omitted

### EventFactory (unit)

Empty DTO → no events; change → one event with `EventType.{Entity}Changed`.

### Provider (unit)

Mock the loader, assert the assembled domain (provider = loader + assembler).

## When adding a field or entity

Update every test that constructs the changed type (addresses, resource,
domain, DTO) — compile errors in `Tests/` point to them. Transversal suites to
check: `StateComposerTests`, `StateEventFactoryTests`, `StateConverterTests`,
`ModelEqualityTests`, `DependencyInjectionTests`, `GameLoopServiceTests`.

## Corner-case pass (before finishing)

Re-read the production code and add tests for: `null`/empty collections,
sentinel boundaries, multiple bitmasks (all must match), raw-byte steps (empty
`BitMasks`), `MemoryReadException` / missing file / missing folder paths,
lookup by id vs list index.
