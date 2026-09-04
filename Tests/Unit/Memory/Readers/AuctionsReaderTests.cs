namespace Tests.Memory.Readers;

using Backend.Memory.Addresses;
using Backend.Memory.Readers;
using Moq;
using Xunit;

public class AuctionsReaderTests
{
    [Fact]
    public void Read_ShouldMapAllAuctionFlagsFromSharedByte()
    {
        var addresses = CreateAddresses();
        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B38A, 1)).Returns([(byte)0x05]);

        var reader = new AuctionsReader(memoryReaderMock.Object);
        var result = reader.Read(addresses);

        Assert.Equal((byte)0x01, result.DivineBarrier);
        Assert.Equal((byte)0x00, result.HazardShield);
        Assert.Equal((byte)0x04, result.SniperShield);
        Assert.Equal((byte)0x00, result.DramonShield);
        Assert.Equal((byte)0x00, result.YinYangWand);
    }

    [Fact]
    public void Read_ShouldReturnZeroFlags_WhenMemoryByteIsZero()
    {
        var addresses = CreateAddresses();
        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(memoryReader => memoryReader.ReadBytes(0x0004B38A, 1)).Returns([(byte)0x00]);

        var reader = new AuctionsReader(memoryReaderMock.Object);
        var result = reader.Read(addresses);

        Assert.Equal((byte)0x00, result.DivineBarrier);
        Assert.Equal((byte)0x00, result.HazardShield);
        Assert.Equal((byte)0x00, result.SniperShield);
        Assert.Equal((byte)0x00, result.DramonShield);
        Assert.Equal((byte)0x00, result.YinYangWand);
    }

    private static AuctionsAddresses CreateAddresses()
    {
        return new AuctionsAddresses
        {
            DivineBarrier = new AuctionAddresses { Address = 0x0004B38A, BitMask = 0x01 },
            HazardShield = new AuctionAddresses { Address = 0x0004B38A, BitMask = 0x02 },
            SniperShield = new AuctionAddresses { Address = 0x0004B38A, BitMask = 0x04 },
            DramonShield = new AuctionAddresses { Address = 0x0004B38A, BitMask = 0x08 },
            YinYangWand = new AuctionAddresses { Address = 0x0004B38A, BitMask = 0x10 },
        };
    }
}
