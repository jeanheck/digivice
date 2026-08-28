namespace Tests.Integration.Application.Loaders;

using Backend.Application.Loaders;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Battles;
using Moq;
using Xunit;

public class BattleLoaderTests : LoaderIntegrationTestBase
{
    private const long EnemySlotBase = 0x000A44D0;
    private const long ActiveUnitIdAddress = 0x000A4558;
    private const long GroupIdAddress = 0x00042B2C;
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
        memoryReaderMock.Setup(m => m.ReadBytes(0x000A4530, 1)).Returns([0x02]);
        memoryReaderMock.Setup(m => m.ReadInt16(GroupIdAddress)).Returns((short)201);

        var loader = new BattleLoader(addressesRepository, new EnemyReader(memoryReaderMock.Object), memoryReaderMock.Object);
        var resource = loader.Load();

        Assert.Equal(0x02, resource.Field);
        Assert.Equal(201, resource.GroupId);
        Assert.Equal(122, resource.Enemy.Id);
        Assert.Equal(0x01, resource.Enemy.Condition);
        Assert.Equal(84, resource.Enemy.Speed);
        Assert.Equal(600, resource.Enemy.HP.Current);
        Assert.Equal(672, resource.Enemy.HP.Max);
    }

    [Fact]
    public void Load_ShouldReadActiveEnemyFromSecondSlot_WhenFirstSlotIsKnockedOut()
    {
        var addressesRepository = CreateAddressesRepository();
        var memoryReaderMock = new Mock<IMemoryReader>();

        SetupEnemySlot(
            memoryReaderMock,
            slotIndex: 0,
            id: 100,
            maxHp: 500,
            currentHp: 0,
            condition: 0,
            speed: 0);
        SetupEnemySlot(
            memoryReaderMock,
            slotIndex: 1,
            id: 200,
            maxHp: 800,
            currentHp: 400,
            condition: 0x02,
            speed: 90);
        SetupEmptyEnemySlot(memoryReaderMock, slotIndex: 2);
        memoryReaderMock.Setup(m => m.ReadInt16(ActiveUnitIdAddress)).Returns((short)386);
        memoryReaderMock.Setup(m => m.ReadBytes(0x000A4530, 1)).Returns([0x02]);
        memoryReaderMock.Setup(m => m.ReadInt16(GroupIdAddress)).Returns((short)272);

        var loader = new BattleLoader(addressesRepository, new EnemyReader(memoryReaderMock.Object), memoryReaderMock.Object);
        var resource = loader.Load();

        Assert.Equal(200, resource.Enemy.Id);
        Assert.Equal(400, resource.Enemy.HP.Current);
        Assert.Equal(800, resource.Enemy.HP.Max);
        Assert.Equal(0x02, resource.Enemy.Condition);
        Assert.Equal(90, resource.Enemy.Speed);
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
        memoryReaderMock.Setup(m => m.ReadBytes(slotBase + 0x1C, 1)).Returns([condition]);
    }
}
