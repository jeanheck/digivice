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
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B38A, 1)).Returns([(byte)0x03]);

        var auctionReader = new AuctionReader(memoryReaderMock.Object);
        var auctionLoader = new AuctionLoader(addressesRepository, auctionReader);

        var auctionsResource = auctionLoader.LoadAuctions();

        Assert.Equal(5, auctionsResource.Auctions.Count);
        Assert.Contains(auctionsResource.Auctions, auction => auction.Id == "divineBarrier" && auction.Value == 0x01);
        Assert.Contains(auctionsResource.Auctions, auction => auction.Id == "hazardShield" && auction.Value == 0x02);
        Assert.Contains(auctionsResource.Auctions, auction => auction.Id == "sniperShield" && auction.Value == 0x00);
        Assert.Contains(auctionsResource.Auctions, auction => auction.Id == "dramonShield" && auction.Value == 0x00);
        Assert.Contains(auctionsResource.Auctions, auction => auction.Id == "yinYangWand" && auction.Value == 0x00);
    }
}
