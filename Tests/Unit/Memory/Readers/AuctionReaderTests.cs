namespace Tests.Memory.Readers;

using Backend.Memory.Addresses;
using Backend.Memory.Readers;
using Moq;
using Xunit;

public class AuctionReaderTests
{
    [Fact]
    public void Read_ShouldExtractParticipationPerAuction_FromSharedByte()
    {
        var addresses = new AuctionAddresses
        {
            Address = 0x0004B38A,
            Auctions = new Dictionary<string, AuctionEntryAddresses>
            {
                ["divineBarrier"] = new() { BitMask = 0x01 },
                ["hazardShield"] = new() { BitMask = 0x02 },
                ["sniperShield"] = new() { BitMask = 0x04 },
                ["dramonShield"] = new() { BitMask = 0x08 },
            },
        };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B38A, 1)).Returns([(byte)0x05]);

        var reader = new AuctionReader(memoryReaderMock.Object);

        var result = reader.Read(addresses);

        Assert.Equal(4, result.Auctions.Count);
        Assert.Equal(0x01, GetValue(result, "divineBarrier"));
        Assert.Equal(0x00, GetValue(result, "hazardShield"));
        Assert.Equal(0x04, GetValue(result, "sniperShield"));
        Assert.Equal(0x00, GetValue(result, "dramonShield"));
        memoryReaderMock.Verify(memoryReader => memoryReader.ReadBytes(0x0004B38A, 1), Times.Once);
    }

    [Fact]
    public void Read_ShouldReturnZeroParticipation_WhenMemoryReaderReturnsZero()
    {
        var addresses = new AuctionAddresses
        {
            Address = 0x0004B38A,
            Auctions = new Dictionary<string, AuctionEntryAddresses>
            {
                ["divineBarrier"] = new() { BitMask = 0x01 },
            },
        };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B38A, 1)).Returns([(byte)0x00]);

        var reader = new AuctionReader(memoryReaderMock.Object);

        var result = reader.Read(addresses);

        Assert.Equal(0x00, Assert.Single(result.Auctions).Value);
    }

    private static byte GetValue(Backend.Memory.Resources.AuctionsResource resource, string auctionId)
    {
        return resource.Auctions.Single(auction => auction.Id == auctionId).Value;
    }
}
