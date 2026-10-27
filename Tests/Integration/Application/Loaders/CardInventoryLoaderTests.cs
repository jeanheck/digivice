namespace Tests.Integration.Application.Loaders;

using Backend.Application.Loaders;
using Backend.Memory;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Interfaces;
using Moq;

public class CardInventoryLoaderTests : LoaderIntegrationTestBase
{
    [Fact]
    public void Load_ShouldIntegrateCardInventoryAddressesAndReader()
    {
        var addressesRepository = CreateAddressesRepository();

        byte[] fakeBytes = new byte[314];
        fakeBytes[9] = 1;

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(0x000490D7, 314)).Returns(fakeBytes);

        var reader = new CardInventoryReader(memoryReaderMock.Object);
        var loader = new CardInventoryLoader(addressesRepository, reader);

        var resource = loader.Load();

        Assert.NotNull(resource);
        Assert.Equal(fakeBytes, resource.Quantities);
    }

    [Fact]
    public void Load_ShouldThrowMemoryReadException_WhenMemoryReaderFails()
    {
        var addressesRepository = CreateAddressesRepository();

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(0x000490D7, 314))
            .Throws(new MemoryReadException(0x000490D7, "Memory session is not connected."));

        var reader = new CardInventoryReader(memoryReaderMock.Object);
        var loader = new CardInventoryLoader(addressesRepository, reader);

        Assert.Throws<MemoryReadException>(() => loader.Load());
    }
}
