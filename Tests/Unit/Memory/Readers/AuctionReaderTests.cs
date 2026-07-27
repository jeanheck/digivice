namespace Tests.Memory.Readers;

using Backend.Memory.Addresses;
using Backend.Memory.Readers;
using Moq;
using Xunit;

public class AuctionReaderTests
{
    [Fact]
    public void Read_ShouldExtractParticipation_FromEntryAddressAndBitMask()
    {
        var auctionEntry = new KeyValuePair<string, AuctionAddresses>(
            "divineBarrier",
            new AuctionAddresses { Address = 0x0004B38A, BitMask = 0x01 }
        );

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B38A, 1)).Returns([(byte)0x05]);

        var reader = new AuctionReader(memoryReaderMock.Object);

        var result = reader.Read(auctionEntry);

        Assert.Equal("divineBarrier", result.Id);
        Assert.Equal(0x01, result.Value);
        memoryReaderMock.Verify(memoryReader => memoryReader.ReadBytes(0x0004B38A, 1), Times.Once);
    }

    [Fact]
    public void Read_ShouldReturnZeroParticipation_WhenBitMaskIsNotSet()
    {
        var auctionEntry = new KeyValuePair<string, AuctionAddresses>(
            "hazardShield",
            new AuctionAddresses { Address = 0x0004B38A, BitMask = 0x02 }
        );

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B38A, 1)).Returns([(byte)0x05]);

        var reader = new AuctionReader(memoryReaderMock.Object);

        var result = reader.Read(auctionEntry);

        Assert.Equal("hazardShield", result.Id);
        Assert.Equal(0x00, result.Value);
    }

    [Fact]
    public void Read_ShouldReturnZeroParticipation_WhenMemoryReaderReturnsZero()
    {
        var auctionEntry = new KeyValuePair<string, AuctionAddresses>(
            "divineBarrier",
            new AuctionAddresses { Address = 0x0004B38A, BitMask = 0x01 }
        );

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B38A, 1)).Returns([(byte)0x00]);

        var reader = new AuctionReader(memoryReaderMock.Object);

        var result = reader.Read(auctionEntry);

        Assert.Equal("divineBarrier", result.Id);
        Assert.Equal(0x00, result.Value);
    }
}
