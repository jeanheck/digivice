namespace Tests.Integration.Application.Loaders;

using Backend.Application.Loaders;
using Backend.Memory.Readers;
using Moq;
using Xunit;

public class AuctionLoaderTests : LoaderIntegrationTestBase
{
    [Fact]
    public void Load_ShouldIntegrateAuctionAddressesAndReader()
    {
        var addressesRepository = CreateAddressesRepository();

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadByteSafe(0x0004B38A, 0x01)).Returns((byte)0x01);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadByteSafe(0x0004B38A, 0x02)).Returns((byte)0x02);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadByteSafe(0x0004B38A, 0x04)).Returns((byte)0x00);
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadByteSafe(0x0004B38A, 0x08)).Returns((byte)0x00);

        var auctionReader = new AuctionReader(memoryReaderMock.Object);
        var auctionLoader = new AuctionLoader(addressesRepository, auctionReader);

        var auctionsResource = auctionLoader.Load();

        Assert.Equal(4, auctionsResource.Auctions.Count);
        Assert.Contains(auctionsResource.Auctions, auction => auction.Id == "divineBarrier" && auction.Value == 0x01);
        Assert.Contains(auctionsResource.Auctions, auction => auction.Id == "hazardShield" && auction.Value == 0x02);
        Assert.Contains(auctionsResource.Auctions, auction => auction.Id == "sniperShield" && auction.Value == 0x00);
        Assert.Contains(auctionsResource.Auctions, auction => auction.Id == "dramonShield" && auction.Value == 0x00);
    }
}
