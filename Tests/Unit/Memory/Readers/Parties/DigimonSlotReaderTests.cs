namespace Tests.Memory.Readers.Parties;

using Backend.Memory;
using Backend.Memory.Addresses.Parties;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Parties;
using Moq;
using Xunit;

public class DigimonSlotReaderTests
{
    [Fact]
    public void Read_ShouldReturnSlotWithDigimonId_WhenBytesAreRead()
    {
        var addresses = new SlotAddresses { Index = 2, Address = 0x5000 };
        var bytes = new byte[] { 14, 0, 0, 0 };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(0x5000, 64)).Returns(bytes);

        var reader = new DigimonSlotReader(memoryReaderMock.Object);

        var result = reader.Read(addresses, 64);

        Assert.NotNull(result);
        Assert.Equal(2, result.Index);
        Assert.Equal(14, result.DigimonId);
    }

    [Fact]
    public void Read_ShouldReturnSlotWithNullDigimonId_WhenBytesAreEmpty()
    {
        var addresses = new SlotAddresses { Index = 5, Address = 0x6000 };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(0x6000, 64)).Returns([]);

        var reader = new DigimonSlotReader(memoryReaderMock.Object);

        var result = reader.Read(addresses, 64);

        Assert.NotNull(result);
        Assert.Equal(5, result.Index);
        Assert.Null(result.DigimonId);
    }

    [Fact]
    public void Read_ShouldThrowMemoryReadException_WhenMemoryReaderFails()
    {
        var addresses = new SlotAddresses { Index = 5, Address = 0x6000 };

        var memoryReaderMock = new Mock<IMemoryReader>();
        memoryReaderMock.Setup(m => m.ReadBytes(0x6000, 64))
            .Throws(new MemoryReadException(0x6000, "Memory session is not connected."));

        var reader = new DigimonSlotReader(memoryReaderMock.Object);

        Assert.Throws<MemoryReadException>(() => reader.Read(addresses, 64));
    }
}
