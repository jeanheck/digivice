namespace Tests.Memory.Readers;

using Backend.Memory.Addresses;
using Backend.Memory.Readers;
using Moq;
using Backend.Memory.Readers.Interfaces;

public class AuctionsReaderTests
{
    [Fact]
    public void Read_ShouldMapAllAuctionFlagsFromSharedByte()
    {
        var addresses = CreateAddresses();
        var memoryReaderMock = new Mock<IMemoryReader>();
        SetupMaskedByte(memoryReaderMock, 0x0004B38A, 0x05);

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
        SetupMaskedByte(memoryReaderMock, 0x0004B38A, 0x00);

        var reader = new AuctionsReader(memoryReaderMock.Object);
        var result = reader.Read(addresses);

        Assert.Equal((byte)0x00, result.DivineBarrier);
        Assert.Equal((byte)0x00, result.HazardShield);
        Assert.Equal((byte)0x00, result.SniperShield);
        Assert.Equal((byte)0x00, result.DramonShield);
        Assert.Equal((byte)0x00, result.YinYangWand);
    }

    private static void SetupMaskedByte(Mock<IMemoryReader> memoryReaderMock, long address, byte rawValue)
    {
        memoryReaderMock
            .Setup(memoryReader => memoryReader.ReadByte(address, It.IsAny<long>()))
            .Returns((long _, long bitMask) => (byte)(rawValue & bitMask));
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
