namespace Tests.Memory.Readers;

using Backend.Memory;
using Backend.Memory.Addresses.Parties;
using Backend.Memory.Readers;
using Moq;
using Xunit;
using Backend.Memory.Readers.Interfaces;

public class DigimonSlotReaderTests
{
    [Fact]
    public void Read_ShouldReturnSlotWithRawDigimonId()
    {
        var addresses = new SlotAddresses { Index = 2, Address = 0x5000 };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadByte(0x5000)).Returns((byte)14);

        var reader = new DigimonSlotReader(memoryReaderMock.Object);

        var result = reader.Read(addresses);

        Assert.NotNull(result);
        Assert.Equal(2, result.Index);
        Assert.Equal(14, result.DigimonId);
    }

    [Fact]
    public void Read_ShouldKeepEmptySlotSentinelAsRawValue()
    {
        var addresses = new SlotAddresses { Index = 3, Address = 0x6000 };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadByte(0x6000)).Returns((byte)0xFF);

        var reader = new DigimonSlotReader(memoryReaderMock.Object);

        var result = reader.Read(addresses);

        Assert.Equal(3, result.Index);
        Assert.Equal(0xFF, result.DigimonId);
        Assert.Null(result.DigimonResource);
    }

    [Fact]
    public void Read_ShouldThrowMemoryReadException_WhenMemoryReaderFails()
    {
        var addresses = new SlotAddresses { Index = 5, Address = 0x6000 };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadByte(0x6000))
            .Throws(new MemoryReadException(0x6000, "Memory session is not connected."));

        var reader = new DigimonSlotReader(memoryReaderMock.Object);

        Assert.Throws<MemoryReadException>(() => reader.Read(addresses));
    }
}
