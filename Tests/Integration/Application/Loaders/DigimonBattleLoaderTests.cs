namespace Tests.Integration.Application.Loaders;

using Backend.Application.Loaders;
using Backend.Memory.Readers;
using Moq;
using Backend.Memory.Readers.Interfaces;

public class DigimonBattleLoaderTests : LoaderIntegrationTestBase
{
    private const long EnemySlotBase = 0x000A44D0;
    private const long ActiveUnitIdAddress = 0x000A4558;
    private const long ActiveEnemySlotIndexAddress = 0x000A446C;
    private const int SlotStride = 0x20;

    [Fact]
    public void Load_ShouldReadEnemyFromEnemyAddresses()
    {
        var addressesRepository = CreateAddressesRepository();
        var memoryReaderMock = new Mock<IMemoryReader>();

        SetupEmptyEnemySlot(memoryReaderMock, slotIndex: 1);
        SetupEmptyEnemySlot(memoryReaderMock, slotIndex: 2);
        SetupEnemySlot(
            memoryReaderMock,
            slotIndex: 0,
            id: 122,
            maxHp: 672,
            currentHp: 600,
            condition: 0x01,
            speed: 84);
        memoryReaderMock.Setup(m => m.ReadInt16(ActiveUnitIdAddress)).Returns((short)122);
        memoryReaderMock.Setup(m => m.ReadByte(0x000A4530)).Returns((byte)0x02);

        var digimonBattleReader = new DigimonBattleReader(
            memoryReaderMock.Object,
            new EnemyReader(memoryReaderMock.Object));
        var loader = new DigimonBattleLoader(addressesRepository, digimonBattleReader);
        var resource = loader.Load();

        Assert.Equal(0x02, resource.Field);
        Assert.Equal(122, resource.Enemy.Id);
        Assert.Equal(0x01, resource.Enemy.Condition);
        Assert.Equal(84, resource.Enemy.Speed);
        Assert.Equal(600, resource.Enemy.HP.Current);
        Assert.Equal(672, resource.Enemy.HP.Max);
    }

    [Fact]
    public void Load_ShouldReturnEnemyWithIdZero_WhenOutOfBattle()
    {
        var addressesRepository = CreateAddressesRepository();
        var memoryReaderMock = new Mock<IMemoryReader>();

        SetupEmptyEnemySlot(memoryReaderMock, slotIndex: 0);
        SetupEmptyEnemySlot(memoryReaderMock, slotIndex: 1);
        SetupEmptyEnemySlot(memoryReaderMock, slotIndex: 2);
        memoryReaderMock.Setup(m => m.ReadInt16(ActiveUnitIdAddress)).Returns((short)0);
        memoryReaderMock.Setup(m => m.ReadInt16(ActiveEnemySlotIndexAddress)).Returns((short)-1);
        memoryReaderMock.Setup(m => m.ReadByte(0x000A4530)).Returns((byte)0x00);

        var digimonBattleReader = new DigimonBattleReader(
            memoryReaderMock.Object,
            new EnemyReader(memoryReaderMock.Object));
        var loader = new DigimonBattleLoader(addressesRepository, digimonBattleReader);
        var resource = loader.Load();

        Assert.Equal(0, resource.Field);
        Assert.Equal(0, resource.Enemy.Id);
    }

    private static long GetSlotBase(int slotIndex)
    {
        return EnemySlotBase + (slotIndex * SlotStride);
    }

    private static void SetupEmptyEnemySlot(Mock<IMemoryReader> memoryReaderMock, int slotIndex)
    {
        SetupEnemySlot(
            memoryReaderMock,
            slotIndex,
            id: 0,
            maxHp: 0,
            currentHp: 0,
            condition: 0,
            speed: 0);
    }

    private static void SetupEnemySlot(
        Mock<IMemoryReader> memoryReaderMock,
        int slotIndex,
        short id,
        short maxHp,
        short currentHp,
        byte condition,
        short speed)
    {
        var slotBase = GetSlotBase(slotIndex);

        memoryReaderMock.Setup(m => m.ReadInt16(slotBase + 0x00)).Returns(id);
        memoryReaderMock.Setup(m => m.ReadInt16(slotBase + 0x06)).Returns(maxHp);
        memoryReaderMock.Setup(m => m.ReadInt16(slotBase + 0x08)).Returns(currentHp);
        memoryReaderMock.Setup(m => m.ReadInt16(slotBase + 0x10)).Returns((short)0);
        memoryReaderMock.Setup(m => m.ReadInt16(slotBase + 0x12)).Returns((short)0);
        memoryReaderMock.Setup(m => m.ReadInt16(slotBase + 0x14)).Returns(speed);
        memoryReaderMock.Setup(m => m.ReadByte(slotBase + 0x1C)).Returns(condition);
    }
}
