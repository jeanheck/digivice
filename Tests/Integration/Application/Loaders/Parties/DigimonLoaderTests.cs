namespace Tests.Integration.Application.Loaders.Parties;

using Backend.Application.Loaders.Parties;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Parties.Digimons;
using Moq;
using Tests.Integration.Application.Loaders;
using Xunit;

public class DigimonLoaderTests : LoaderIntegrationTestBase
{
    [Fact]
    public void Load_ShouldIntegrateDigimonAddressesStatusAndReaderPipeline()
    {
        var addressesRepository = CreateAddressesRepository();
        var memoryBlock = new byte[1500];

        WriteInt32(memoryBlock, 0x18, 85000);
        WriteInt16(memoryBlock, 0x1C, 45);

        WriteInt16(memoryBlock, 0x20, 500);
        WriteInt16(memoryBlock, 0x22, 1000);
        WriteInt16(memoryBlock, 0x24, 200);
        WriteInt16(memoryBlock, 0x26, 400);

        WriteInt16(memoryBlock, 0x28, 80);
        WriteInt16(memoryBlock, 0x2A, 75);
        WriteInt16(memoryBlock, 0x2C, 60);
        WriteInt16(memoryBlock, 0x2E, 95);
        WriteInt16(memoryBlock, 0x30, 110);
        WriteInt16(memoryBlock, 0x32, 40);

        WriteInt16(memoryBlock, 0x34, 10);
        WriteInt16(memoryBlock, 0x36, 20);
        WriteInt16(memoryBlock, 0x38, 30);
        WriteInt16(memoryBlock, 0x3A, 40);
        WriteInt16(memoryBlock, 0x3C, 50);
        WriteInt16(memoryBlock, 0x3E, 60);
        WriteInt16(memoryBlock, 0x40, 70);

        WriteInt16(memoryBlock, 0x3C0, 101);
        WriteInt16(memoryBlock, 0x3C2, 102);
        WriteInt16(memoryBlock, 0x3C4, 103);
        WriteInt16(memoryBlock, 0x3C6, 104);
        WriteInt16(memoryBlock, 0x3C8, 105);
        WriteInt16(memoryBlock, 0x3CA, 106);

        WriteInt16(memoryBlock, 0x48, 5);
        WriteInt16(memoryBlock, 0x4A, 10);
        WriteInt16(memoryBlock, 0x4C, 0);
        WriteInt16(memoryBlock, 0x50, 5);
        WriteInt16(memoryBlock, 0x52, 3);

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(0x00049878, 1500)).Returns(memoryBlock);
        memoryReaderMock.Setup(m => m.ReadInt16(0x00049878 - 4)).Returns((short)5);

        var digievolutionSlotReader = new DigievolutionSlotReader();
        var digievolutionReader = new DigievolutionReader();
        var digimonReader = new DigimonReader(memoryReaderMock.Object, digievolutionSlotReader, digievolutionReader);
        var digimonLoader = new DigimonLoader(addressesRepository, digimonReader);

        var digimonResource = digimonLoader.Load(1);

        Assert.NotNull(digimonResource);
        Assert.Equal(5, digimonResource.ActiveDigievolutionId);
        Assert.Equal(85000, digimonResource.Experience);
        Assert.Equal(45, digimonResource.Level);
        Assert.Equal(500, digimonResource.Vitals.CurrentHP);
        Assert.Equal(1000, digimonResource.Vitals.MaxHP);
        Assert.Equal(200, digimonResource.Vitals.CurrentMP);
        Assert.Equal(400, digimonResource.Vitals.MaxMP);
        Assert.Equal(80, digimonResource.Attributes.Strength);
        Assert.Equal(75, digimonResource.Attributes.Defense);
        Assert.Equal(60, digimonResource.Attributes.Spirit);
        Assert.Equal(95, digimonResource.Attributes.Wisdow);
        Assert.Equal(110, digimonResource.Attributes.Speed);
        Assert.Equal(40, digimonResource.Attributes.Charisma);
        Assert.Equal(10, digimonResource.Resistances.Fire);
        Assert.Equal(20, digimonResource.Resistances.Water);
        Assert.Equal(30, digimonResource.Resistances.Ice);
        Assert.Equal(40, digimonResource.Resistances.Wind);
        Assert.Equal(50, digimonResource.Resistances.Thunder);
        Assert.Equal(60, digimonResource.Resistances.Machine);
        Assert.Equal(70, digimonResource.Resistances.Dark);
        Assert.Equal(101, digimonResource.Equipments.Head);
        Assert.Equal(102, digimonResource.Equipments.Body);
        Assert.Equal(103, digimonResource.Equipments.RightHand);
        Assert.Equal(104, digimonResource.Equipments.LeftHand);
        Assert.Equal(105, digimonResource.Equipments.Accessory1);
        Assert.Equal(106, digimonResource.Equipments.Accessory2);
        Assert.Equal(3, digimonResource.Digievolutions.Count);
        Assert.Equal(5, digimonResource.Digievolutions[0].DigievolutionId);
        Assert.Equal(3, digimonResource.Digievolutions[0].DigievolutionResource!.Level);
        Assert.Equal(10, digimonResource.Digievolutions[1].DigievolutionId);
        Assert.Equal(1, digimonResource.Digievolutions[1].DigievolutionResource!.Level);
        Assert.Null(digimonResource.Digievolutions[2].DigievolutionId);
        Assert.Null(digimonResource.Digievolutions[2].DigievolutionResource);
    }

    [Fact]
    public void Load_ShouldIntegrateAnotherValidDigimonAddress()
    {
        var addressesRepository = CreateAddressesRepository();
        var memoryBlock = new byte[1500];
        WriteInt32(memoryBlock, 0x18, 12000);
        WriteInt16(memoryBlock, 0x1C, 18);

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(0x0004AFA0, 1500)).Returns(memoryBlock);
        memoryReaderMock.Setup(m => m.ReadInt16(0x0004AFA0 - 4)).Returns((short)7);

        var digimonLoader = CreateDigimonLoader(addressesRepository, memoryReaderMock.Object);

        var digimonResource = digimonLoader.Load(7);

        Assert.NotNull(digimonResource);
        Assert.Equal(7, digimonResource.ActiveDigievolutionId);
        Assert.Equal(12000, digimonResource.Experience);
        Assert.Equal(18, digimonResource.Level);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(1499)]
    public void Load_ShouldThrowInvalidOperationException_WhenDigimonMemoryBlockCannotBeFullyRead(int? memoryBlockSize)
    {
        var addressesRepository = CreateAddressesRepository();

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(0x00049878, 1500))
            .Returns(memoryBlockSize is null ? null : new byte[memoryBlockSize.Value]);

        var digimonLoader = CreateDigimonLoader(addressesRepository, memoryReaderMock.Object);

        var exception = Assert.Throws<InvalidOperationException>(() => digimonLoader.Load(1));
        Assert.Contains("Failed to read full memory block for Digimon", exception.Message);
    }

    private static DigimonLoader CreateDigimonLoader(
        Backend.Memory.Repositories.IAddressesRepository addressesRepository,
        IMemoryReader memoryReader)
    {
        var digievolutionSlotReader = new DigievolutionSlotReader();
        var digievolutionReader = new DigievolutionReader();
        var digimonReader = new DigimonReader(memoryReader, digievolutionSlotReader, digievolutionReader);
        return new DigimonLoader(addressesRepository, digimonReader);
    }
}
