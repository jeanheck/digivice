namespace Tests.Integration.Application.Loaders;

using Moq;
using Backend.Application.Loaders;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Interfaces;

public class PartyLoaderTests : LoaderIntegrationTestBase
{
    private const byte EmptySlotId = 0xFF;

    [Fact]
    public void Load_ShouldIntegratePipelineAndSkipEmptySlots()
    {
        var addressesRepository = CreateAddressesRepository();

        var memoryReaderMock = new Mock<IMemoryReader>();

        // Party slot ids at 0x00048DA4 / 0x00048DA8 / 0x00048DAC; Kumamon (id 1) block at 0x00049878
        memoryReaderMock.Setup(m => m.ReadByte(0x00048DA4)).Returns((byte)1);
        memoryReaderMock.Setup(m => m.ReadByte(0x00048DA8)).Returns(EmptySlotId);
        memoryReaderMock.Setup(m => m.ReadByte(0x00048DAC)).Returns(EmptySlotId);

        // Offsets from DigimonStatusAddresses.json
        var fakeMemoryBlock = new byte[1500];
        WriteInt32(fakeMemoryBlock, 0x18, 1500);
        WriteInt16(fakeMemoryBlock, 0x1C, 12);
        WriteInt16(fakeMemoryBlock, 0x20, 450);
        WriteInt16(fakeMemoryBlock, 0x22, 500);
        WriteInt16(fakeMemoryBlock, 0x28, 42);
        WriteInt16(fakeMemoryBlock, 0x48, 5);
        WriteInt16(fakeMemoryBlock, 0x4A, 10);
        WriteInt16(fakeMemoryBlock, 0x50, 5);
        WriteInt16(fakeMemoryBlock, 0x52, 3);

        memoryReaderMock.Setup(m => m.ReadBytes(0x00049878, 1500))
            .Returns(fakeMemoryBlock);
        memoryReaderMock.Setup(m => m.ReadInt16(0x00049878 - 4))
            .Returns(5);
        memoryReaderMock.Setup(m => m.ReadInt16(0x00042B76))
            .Returns((short)0);

        var partyLoader = CreatePartyLoader(addressesRepository, memoryReaderMock.Object);

        var partyResource = partyLoader.Load();

        Assert.NotNull(partyResource);
        Assert.Equal(3, partyResource.SlotsResource.Count);

        var slot0 = partyResource.SlotsResource[0];
        Assert.Equal(1, slot0.Index);
        Assert.Equal(1, slot0.DigimonId);
        Assert.NotNull(slot0.DigimonResource);

        var kumamon = slot0.DigimonResource;
        Assert.Equal(5, kumamon.ActiveDigievolutionId);
        Assert.Equal(1500, kumamon.Experience);
        Assert.Equal(12, kumamon.Level);
        Assert.Equal(450, kumamon.HP.Current);
        Assert.Equal(500, kumamon.HP.Max);
        Assert.Equal(42, kumamon.Attributes.Strength);

        Assert.Equal(3, kumamon.Digievolutions.Count);

        var evolutionSlot1 = kumamon.Digievolutions[0];
        Assert.Equal(5, evolutionSlot1.DigievolutionId);

        var evolutionSlot2 = kumamon.Digievolutions[1];
        Assert.Equal(10, evolutionSlot2.DigievolutionId);

        var storedDigievolution = Assert.Single(kumamon.StoredDigievolutions);
        Assert.Equal(5, storedDigievolution.DigievolutionId);
        Assert.Equal(3, storedDigievolution.Level);

        var slot1 = partyResource.SlotsResource[1];
        Assert.Equal(2, slot1.Index);
        Assert.Equal(EmptySlotId, slot1.DigimonId);
        Assert.Null(slot1.DigimonResource);

        var slot2 = partyResource.SlotsResource[2];
        Assert.Equal(3, slot2.Index);
        Assert.Equal(EmptySlotId, slot2.DigimonId);
        Assert.Null(slot2.DigimonResource);

        memoryReaderMock.Verify(m => m.ReadBytes(It.Is<long>(address => address != 0x00049878), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Load_ShouldThrowMemoryReadException_WhenSlotCannotBeRead()
    {
        var addressesRepository = CreateAddressesRepository();
        var memoryReaderMock = new Mock<IMemoryReader>();

        memoryReaderMock.Setup(m => m.ReadByte(0x00048DA4))
            .Throws(new Backend.Memory.MemoryReadException(0x00048DA4, "Memory session is not connected."));
        memoryReaderMock.Setup(m => m.ReadByte(0x00048DA8)).Returns((byte)2);
        memoryReaderMock.Setup(m => m.ReadByte(0x00048DAC)).Returns(EmptySlotId);

        var partyLoader = CreatePartyLoader(addressesRepository, memoryReaderMock.Object);

        Assert.Throws<Backend.Memory.MemoryReadException>(() => partyLoader.Load());
    }

    [Fact]
    public void Load_ShouldLoadOnlyOccupiedSlots()
    {
        var addressesRepository = CreateAddressesRepository();
        var memoryReaderMock = new Mock<IMemoryReader>();

        memoryReaderMock.Setup(m => m.ReadByte(0x00048DA4)).Returns(EmptySlotId);
        memoryReaderMock.Setup(m => m.ReadByte(0x00048DA8)).Returns((byte)2);
        memoryReaderMock.Setup(m => m.ReadByte(0x00048DAC)).Returns(EmptySlotId);

        // Monmon (id 2) block at 0x00049C54
        var fakeMemoryBlock = new byte[1500];
        WriteInt16(fakeMemoryBlock, 0x1C, 8);

        memoryReaderMock.Setup(m => m.ReadBytes(0x00049C54, 1500))
            .Returns(fakeMemoryBlock);

        memoryReaderMock.Setup(m => m.ReadInt16(0x00049C54 - 4))
            .Returns(2);
        memoryReaderMock.Setup(m => m.ReadInt16(0x00042B78))
            .Returns((short)0);

        var partyLoader = CreatePartyLoader(addressesRepository, memoryReaderMock.Object);

        var partyResource = partyLoader.Load();

        Assert.NotNull(partyResource);
        Assert.Equal(3, partyResource.SlotsResource.Count);

        var slot0 = partyResource.SlotsResource[0];
        Assert.Equal(EmptySlotId, slot0.DigimonId);
        Assert.Null(slot0.DigimonResource);

        var slot1 = partyResource.SlotsResource[1];
        Assert.Equal(2, slot1.DigimonId);
        Assert.NotNull(slot1.DigimonResource);
        Assert.Equal(8, slot1.DigimonResource.Level);

        var slot2 = partyResource.SlotsResource[2];
        Assert.Equal(EmptySlotId, slot2.DigimonId);
        Assert.Null(slot2.DigimonResource);
    }

    [Fact]
    public void Load_ShouldNotLoadDigimonResource_WhenSlotContainsUnknownDigimonId()
    {
        var addressesRepository = CreateAddressesRepository();
        var memoryReaderMock = new Mock<IMemoryReader>();

        memoryReaderMock.Setup(m => m.ReadByte(0x00048DA4)).Returns((byte)99);
        memoryReaderMock.Setup(m => m.ReadByte(0x00048DA8)).Returns(EmptySlotId);
        memoryReaderMock.Setup(m => m.ReadByte(0x00048DAC)).Returns(EmptySlotId);

        var partyLoader = CreatePartyLoader(addressesRepository, memoryReaderMock.Object);

        var partyResource = partyLoader.Load();

        Assert.NotNull(partyResource);
        Assert.Equal(99, partyResource.SlotsResource[0].DigimonId);
        Assert.Null(partyResource.SlotsResource[0].DigimonResource);
        memoryReaderMock.Verify(m => m.ReadBytes(It.IsAny<long>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Load_ShouldNotLoadDigimonResource_WhenLaterSlotContainsUnknownDigimonId()
    {
        var addressesRepository = CreateAddressesRepository();
        var memoryReaderMock = new Mock<IMemoryReader>();

        memoryReaderMock.Setup(m => m.ReadByte(0x00048DA4)).Returns((byte)1);
        memoryReaderMock.Setup(m => m.ReadByte(0x00048DA8)).Returns((byte)99);
        memoryReaderMock.Setup(m => m.ReadByte(0x00048DAC)).Returns(EmptySlotId);

        var fakeMemoryBlock = new byte[1500];
        BitConverter.GetBytes((short)12).CopyTo(fakeMemoryBlock, 28);

        memoryReaderMock.Setup(m => m.ReadBytes(0x00049878, 1500))
            .Returns(fakeMemoryBlock);
        memoryReaderMock.Setup(m => m.ReadInt16(0x00049878 - 4))
            .Returns(5);
        memoryReaderMock.Setup(m => m.ReadInt16(0x00042B76))
            .Returns((short)0);

        var partyLoader = CreatePartyLoader(addressesRepository, memoryReaderMock.Object);

        var partyResource = partyLoader.Load();

        Assert.NotNull(partyResource);
        Assert.Equal(1, partyResource.SlotsResource[0].DigimonId);
        Assert.NotNull(partyResource.SlotsResource[0].DigimonResource);
        Assert.Equal(99, partyResource.SlotsResource[1].DigimonId);
        Assert.Null(partyResource.SlotsResource[1].DigimonResource);
        memoryReaderMock.Verify(m => m.ReadBytes(0x00049878, 1500), Times.Once);
    }

    [Fact]
    public void Load_ShouldLoadKotemon_WhenSlotContainsDigimonIdZero()
    {
        const long KotemonMemoryBlockAddress = 0x0004949C;
        const long KotemonBlastAddress = 0x00042B74;

        var addressesRepository = CreateAddressesRepository();
        var memoryReaderMock = new Mock<IMemoryReader>();

        memoryReaderMock.Setup(m => m.ReadByte(0x00048DA4)).Returns((byte)0);
        memoryReaderMock.Setup(m => m.ReadByte(0x00048DA8)).Returns(EmptySlotId);
        memoryReaderMock.Setup(m => m.ReadByte(0x00048DAC)).Returns(EmptySlotId);

        var fakeMemoryBlock = new byte[1500];
        WriteInt16(fakeMemoryBlock, 0x1C, 7);

        memoryReaderMock.Setup(m => m.ReadBytes(KotemonMemoryBlockAddress, 1500)).Returns(fakeMemoryBlock);
        memoryReaderMock.Setup(m => m.ReadInt16(KotemonBlastAddress)).Returns((short)420);

        var partyLoader = CreatePartyLoader(addressesRepository, memoryReaderMock.Object);

        var partyResource = partyLoader.Load();

        var kotemonSlot = partyResource.SlotsResource[0];
        Assert.Equal(0, kotemonSlot.DigimonId);
        Assert.NotNull(kotemonSlot.DigimonResource);
        Assert.Equal(7, kotemonSlot.DigimonResource.Level);
        Assert.Equal(420, kotemonSlot.DigimonResource.Blast);
    }

    [Fact]
    public void Load_ShouldReturnAllEmptySlots_WhenAllSlotsAreEmpty()
    {
        var addressesRepository = CreateAddressesRepository();
        var memoryReaderMock = new Mock<IMemoryReader>();

        memoryReaderMock.Setup(m => m.ReadByte(0x00048DA4)).Returns(EmptySlotId);
        memoryReaderMock.Setup(m => m.ReadByte(0x00048DA8)).Returns(EmptySlotId);
        memoryReaderMock.Setup(m => m.ReadByte(0x00048DAC)).Returns(EmptySlotId);

        var partyLoader = CreatePartyLoader(addressesRepository, memoryReaderMock.Object);

        var partyResource = partyLoader.Load();

        Assert.NotNull(partyResource);
        Assert.Equal(3, partyResource.SlotsResource.Count);
        Assert.All(partyResource.SlotsResource, slot =>
        {
            Assert.Equal(EmptySlotId, slot.DigimonId);
            Assert.Null(slot.DigimonResource);
        });

        memoryReaderMock.Verify(m => m.ReadBytes(It.IsAny<long>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Load_ShouldKeepPersistentVitalsAndPopulateInBattle()
    {
        var addressesRepository = CreateAddressesRepository();
        var memoryReaderMock = new Mock<IMemoryReader>();

        memoryReaderMock.Setup(m => m.ReadByte(0x00048DA4)).Returns((byte)1);
        memoryReaderMock.Setup(m => m.ReadByte(0x00048DA8)).Returns(EmptySlotId);
        memoryReaderMock.Setup(m => m.ReadByte(0x00048DAC)).Returns(EmptySlotId);

        var fakeMemoryBlock = new byte[1500];
        BitConverter.GetBytes((short)450).CopyTo(fakeMemoryBlock, 0x20);
        BitConverter.GetBytes((short)500).CopyTo(fakeMemoryBlock, 0x22);
        BitConverter.GetBytes((short)200).CopyTo(fakeMemoryBlock, 0x24);
        BitConverter.GetBytes((short)300).CopyTo(fakeMemoryBlock, 0x26);

        memoryReaderMock.Setup(m => m.ReadBytes(0x00049878, 1500)).Returns(fakeMemoryBlock);
        memoryReaderMock.Setup(m => m.ReadInt16(0x00049878 - 4)).Returns(5);
        memoryReaderMock.Setup(m => m.ReadInt16(0x00042B76)).Returns((short)0);

        memoryReaderMock.Setup(m => m.ReadInt16(0x000A4470 + 0x06)).Returns((short)1850);
        memoryReaderMock.Setup(m => m.ReadInt16(0x000A4470 + 0x08)).Returns((short)1400);
        memoryReaderMock.Setup(m => m.ReadInt16(0x000A4470 + 0x0A)).Returns((short)1140);
        memoryReaderMock.Setup(m => m.ReadInt16(0x000A4470 + 0x0C)).Returns((short)900);
        memoryReaderMock.Setup(m => m.ReadInt16(0x000A4470 + 0x10)).Returns((short)252);
        memoryReaderMock.Setup(m => m.ReadInt16(0x000A4470 + 0x12)).Returns((short)185);
        memoryReaderMock.Setup(m => m.ReadInt16(0x000A4470 + 0x14)).Returns((short)84);
        memoryReaderMock.Setup(m => m.ReadByte(0x000A4470 + 0x1C)).Returns((byte)0x04);

        var partyLoader = CreatePartyLoader(addressesRepository, memoryReaderMock.Object);
        var partyResource = partyLoader.Load();

        Assert.NotNull(partyResource.SlotsResource[0].DigimonResource);
        var digimon = partyResource.SlotsResource[0].DigimonResource!;
        Assert.Equal(450, digimon.HP.Current);
        Assert.Equal(500, digimon.HP.Max);
        Assert.Equal(200, digimon.MP.Current);
        Assert.Equal(300, digimon.MP.Max);
        Assert.Equal(1400, digimon.InBattle.HP.Current);
        Assert.Equal(1850, digimon.InBattle.HP.Max);
        Assert.Equal(900, digimon.InBattle.MP.Current);
        Assert.Equal(1140, digimon.InBattle.MP.Max);
        Assert.Equal(0x04, digimon.InBattle.Condition);
        Assert.Equal(252, digimon.InBattle.Strength);
        Assert.Equal(185, digimon.InBattle.Defense);
        Assert.Equal(84, digimon.InBattle.Speed);
    }

    private static PartyLoader CreatePartyLoader(
        Backend.Memory.Repositories.IAddressesRepository addressesRepository,
        IMemoryReader memoryReader)
    {
        var digievolutionSlotReader = new DigievolutionSlotReader();
        var storedDigievolutionReader = new StoredDigievolutionReader();
        var digimonReader = new DigimonReader(memoryReader, digievolutionSlotReader, storedDigievolutionReader, new InBattleReader(memoryReader));
        var digimonSlotReader = new DigimonSlotReader(memoryReader);
        var partyReader = new PartyReader(digimonSlotReader);
        var digimonLoader = new DigimonLoader(addressesRepository, digimonReader);
        return new PartyLoader(
            addressesRepository,
            partyReader,
            digimonLoader);
    }
}
