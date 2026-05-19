namespace Tests.Integration.Application.Loaders;

using Backend.Application.Loaders;
using Backend.Memory.Readers;
using Moq;
using Xunit;

public class PlayerLoaderTests : LoaderIntegrationTestBase
{
    [Fact]
    public void Load_ShouldIntegratePlayerAddressesAndReader()
    {
        var addressesRepository = CreateAddressesRepository();
        var nameBytes = new byte[] { 65, 103, 117, 109, 111, 110, 0, 0, 0, 0 };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadInt32(0x00048DA0)).Returns(15000);
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048D88, 10)).Returns(nameBytes);
        memoryReaderMock.Setup(m => m.ReadInt16(0x0004B3F8)).Returns((short)4);

        var playerReader = new PlayerReader(memoryReaderMock.Object);
        var playerLoader = new PlayerLoader(addressesRepository, playerReader);

        var playerResource = playerLoader.Load();

        Assert.NotNull(playerResource);
        Assert.Equal(15000, playerResource.Bits);
        Assert.Equal(nameBytes, playerResource.NameInBytes);
        Assert.Equal((short)4, playerResource.MapId);
    }

    [Fact]
    public void Load_ShouldReturnNullFields_WhenMemoryReaderCannotReadPlayerData()
    {
        var addressesRepository = CreateAddressesRepository();

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadInt32(0x00048DA0)).Returns((int?)null);
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048D88, 10)).Returns((byte[]?)null);
        memoryReaderMock.Setup(m => m.ReadInt16(0x0004B3F8)).Returns((short?)null);

        var playerReader = new PlayerReader(memoryReaderMock.Object);
        var playerLoader = new PlayerLoader(addressesRepository, playerReader);

        var playerResource = playerLoader.Load();

        Assert.NotNull(playerResource);
        Assert.Null(playerResource.Bits);
        Assert.Null(playerResource.NameInBytes);
        Assert.Null(playerResource.MapId);
    }
}
