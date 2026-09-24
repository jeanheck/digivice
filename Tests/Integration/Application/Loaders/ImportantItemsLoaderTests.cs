namespace Tests.Integration.Application.Loaders;

using Backend.Application.Loaders;
using Backend.Memory;
using Backend.Memory.Readers;
using Moq;
using Backend.Memory.Readers.Interfaces;

public class ImportantItemsLoaderTests : LoaderIntegrationTestBase
{
    [Fact]
    public void Load_ShouldIntegrateImportantItemsAddressesAndReader()
    {
        var addressesRepository = CreateAddressesRepository();

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadByte(0x00048DB4)).Returns((byte)0x01);
        memoryReaderMock.Setup(m => m.ReadByte(0x00048DB5)).Returns((byte)0x00);
        memoryReaderMock.Setup(m => m.ReadByte(0x00048DC2)).Returns((byte)0x01);
        memoryReaderMock.Setup(m => m.ReadByte(0x00048DC4)).Returns((byte)0x00);

        var importantItemsReader = new ImportantItemsReader(memoryReaderMock.Object);
        var importantItemsLoader = new ImportantItemsLoader(addressesRepository, importantItemsReader);

        var resource = importantItemsLoader.Load();

        Assert.NotNull(resource);
        Assert.Equal((byte)0x01, resource.TreeBoots);
        Assert.Equal((byte)0x00, resource.FishingPole);
        Assert.Equal((byte)0x01, resource.AsukaTrophy);
        Assert.Equal((byte)0x00, resource.SunTrophy);
    }

    [Fact]
    public void Load_ShouldThrowMemoryReadException_WhenMemoryReaderCannotRead()
    {
        var addressesRepository = CreateAddressesRepository();

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadByte(0x00048DB4))
            .Throws(new MemoryReadException(0x00048DB4, "Memory session is not connected."));

        var importantItemsReader = new ImportantItemsReader(memoryReaderMock.Object);
        var importantItemsLoader = new ImportantItemsLoader(addressesRepository, importantItemsReader);

        Assert.Throws<MemoryReadException>(() => importantItemsLoader.Load());
    }
}
