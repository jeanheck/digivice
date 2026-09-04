namespace Tests.Memory.Readers;

using Backend.Memory.Addresses;
using Backend.Memory.Addresses.Parties.Digimons;
using Backend.Memory.Readers;
using Moq;
using Xunit;
using Backend.Memory.Readers.Interfaces;

public class EnemyReaderTests
{
    private const long EnemySlotBase = 0x000A44D0;
    private const long ActiveUnitIdAddress = 0x000A4558;
    private const long ActiveEnemySlotIndexAddress = 0x000A446C;
    private const long GroupIdAddress = 0x00042B2C;
    private const int SlotStride = 0x20;

    [Fact]
    public void Read_ShouldReadSlotZero_WhenOnlyFirstSlotHasLiveEnemy()
    {
        var addresses = CreateAddresses();
        var memoryReaderMock = CreateMemoryReaderMock();
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
        memoryReaderMock.Setup(m => m.ReadInt16(GroupIdAddress)).Returns((short)201);

        var reader = new EnemyReader(memoryReaderMock.Object);
        var result = reader.Read(addresses);

        Assert.Equal(122, result.Id);
        Assert.Equal(201, result.GroupId);
        Assert.Equal(600, result.HP.Current);
        Assert.Equal(672, result.HP.Max);
        Assert.Equal(0x01, result.Condition);
        Assert.Equal(84, result.Speed);
    }

    [Fact]
    public void Read_ShouldReadSecondSlot_WhenFirstSlotIsKnockedOut()
    {
        var addresses = CreateAddresses();
        var memoryReaderMock = CreateMemoryReaderMock();
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
            condition: 0,
            speed: 90);
        SetupEmptyEnemySlot(memoryReaderMock, slotIndex: 2);
        memoryReaderMock.Setup(m => m.ReadInt16(ActiveUnitIdAddress)).Returns((short)386);

        var reader = new EnemyReader(memoryReaderMock.Object);
        var result = reader.Read(addresses);

        Assert.Equal(200, result.Id);
        Assert.Equal(400, result.HP.Current);
        Assert.Equal(800, result.HP.Max);
        Assert.Equal(90, result.Speed);
    }

    [Fact]
    public void Read_ShouldPreferActiveUnitIdMatch_WhenMultipleLiveEnemySlotsExist()
    {
        var addresses = CreateAddresses();
        var memoryReaderMock = CreateMemoryReaderMock();
        SetupEnemySlot(
            memoryReaderMock,
            slotIndex: 0,
            id: 100,
            maxHp: 500,
            currentHp: 300,
            condition: 0,
            speed: 70);
        SetupEnemySlot(
            memoryReaderMock,
            slotIndex: 1,
            id: 200,
            maxHp: 800,
            currentHp: 400,
            condition: 0,
            speed: 90);
        SetupEmptyEnemySlot(memoryReaderMock, slotIndex: 2);
        memoryReaderMock.Setup(m => m.ReadInt16(ActiveUnitIdAddress)).Returns((short)200);

        var reader = new EnemyReader(memoryReaderMock.Object);
        var result = reader.Read(addresses);

        Assert.Equal(200, result.Id);
        Assert.Equal(400, result.HP.Current);
    }

    [Fact]
    public void Read_ShouldReadLastOccupiedSlot_WhenAllEnemySlotsAreKnockedOut()
    {
        var addresses = CreateAddresses();
        var memoryReaderMock = CreateMemoryReaderMock();
        SetupEnemySlot(
            memoryReaderMock,
            slotIndex: 0,
            id: 100,
            maxHp: 500,
            currentHp: 0,
            condition: 0,
            speed: 70);
        SetupEnemySlot(
            memoryReaderMock,
            slotIndex: 1,
            id: 200,
            maxHp: 800,
            currentHp: 0,
            condition: 0,
            speed: 90);
        SetupEmptyEnemySlot(memoryReaderMock, slotIndex: 2);
        memoryReaderMock.Setup(m => m.ReadInt16(ActiveUnitIdAddress)).Returns((short)386);

        var reader = new EnemyReader(memoryReaderMock.Object);
        var result = reader.Read(addresses);

        Assert.Equal(200, result.Id);
        Assert.Equal(0, result.HP.Current);
    }

    [Fact]
    public void Read_ShouldStayOnKnockedOutEnemy_WhenActiveUnitIdStillPointsToThem()
    {
        var addresses = CreateAddresses();
        var memoryReaderMock = CreateMemoryReaderMock();
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
            condition: 0,
            speed: 90);
        SetupEmptyEnemySlot(memoryReaderMock, slotIndex: 2);
        memoryReaderMock.Setup(m => m.ReadInt16(ActiveUnitIdAddress)).Returns((short)100);

        var reader = new EnemyReader(memoryReaderMock.Object);
        var result = reader.Read(addresses);

        Assert.Equal(100, result.Id);
        Assert.Equal(0, result.HP.Current);
    }

    [Fact]
    public void Read_ShouldReadThirdSlot_WhenAllThreeEnemiesAreKnockedOut()
    {
        var addresses = CreateAddresses();
        var memoryReaderMock = CreateMemoryReaderMock();
        SetupEnemySlot(
            memoryReaderMock,
            slotIndex: 0,
            id: 100,
            maxHp: 500,
            currentHp: 0,
            condition: 0,
            speed: 70);
        SetupEnemySlot(
            memoryReaderMock,
            slotIndex: 1,
            id: 200,
            maxHp: 800,
            currentHp: 0,
            condition: 0,
            speed: 90);
        SetupEnemySlot(
            memoryReaderMock,
            slotIndex: 2,
            id: 300,
            maxHp: 600,
            currentHp: 0,
            condition: 0,
            speed: 80);
        memoryReaderMock.Setup(m => m.ReadInt16(ActiveUnitIdAddress)).Returns((short)386);

        var reader = new EnemyReader(memoryReaderMock.Object);
        var result = reader.Read(addresses);

        Assert.Equal(300, result.Id);
        Assert.Equal(0, result.HP.Current);
    }

    [Fact]
    public void Read_ShouldReadThirdSlot_WhenActiveEnemySlotIndexPointsToFrontAndPriorEnemiesAreAlive()
    {
        var addresses = CreateAddresses();
        var memoryReaderMock = CreateMemoryReaderMock();
        SetupEnemySlot(
            memoryReaderMock,
            slotIndex: 0,
            id: 197,
            maxHp: 528,
            currentHp: 0,
            condition: 0,
            speed: 0);
        SetupEnemySlot(
            memoryReaderMock,
            slotIndex: 1,
            id: 132,
            maxHp: 528,
            currentHp: 222,
            condition: 0,
            speed: 0);
        SetupEnemySlot(
            memoryReaderMock,
            slotIndex: 2,
            id: 110,
            maxHp: 552,
            currentHp: 552,
            condition: 0,
            speed: 0);
        memoryReaderMock.Setup(m => m.ReadInt16(ActiveUnitIdAddress)).Returns((short)386);
        memoryReaderMock.Setup(m => m.ReadInt16(ActiveEnemySlotIndexAddress)).Returns((short)2);

        var reader = new EnemyReader(memoryReaderMock.Object);
        var result = reader.Read(addresses);

        Assert.Equal(110, result.Id);
        Assert.Equal(552, result.HP.Current);
        Assert.Equal(552, result.HP.Max);
    }

    private static Mock<IMemoryReader> CreateMemoryReaderMock()
    {
        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadInt16(GroupIdAddress)).Returns((short)0);
        memoryReaderMock.Setup(m => m.ReadInt16(ActiveEnemySlotIndexAddress)).Returns((short)-1);
        return memoryReaderMock;
    }

    private static EnemyAddresses CreateAddresses()
    {
        return new EnemyAddresses
        {
            EnemySlotBase = EnemySlotBase,
            SlotStride = SlotStride,
            SlotCount = 3,
            ActiveUnitId = ActiveUnitIdAddress,
            ActiveEnemySlotIndex = ActiveEnemySlotIndexAddress,
            GroupId = GroupIdAddress,
            Id = 0x00,
            Condition = 0x1C,
            Strength = 0x10,
            Defense = 0x12,
            Speed = 0x14,
            HP = new VitalAddresses
            {
                Max = 0x06,
                Current = 0x08,
            },
        };
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
