namespace Tests.Integration.Application.Loaders;

using Backend.Application.Loaders;
using Backend.Memory.Readers;
using Moq;
using Xunit;

public class AuctionsLoaderTests : LoaderIntegrationTestBase
{
    [Fact]
    public void Load_ShouldIntegrateAuctionsAddressesAndReader()
    {
        var addressesRepository = CreateAddressesRepository();
        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B38A, 1)).Returns([(byte)0x03]);

        var auctionsReader = new AuctionsReader(memoryReaderMock.Object);
        var auctionsLoader = new AuctionsLoader(addressesRepository, auctionsReader);

        var resource = auctionsLoader.Load();

        Assert.Equal((byte)0x01, resource.DivineBarrier);
        Assert.Equal((byte)0x02, resource.HazardShield);
        Assert.Equal((byte)0x00, resource.SniperShield);
        Assert.Equal((byte)0x00, resource.DramonShield);
        Assert.Equal((byte)0x00, resource.YinYangWand);
    }
}
