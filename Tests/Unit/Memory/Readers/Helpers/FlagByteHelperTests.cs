namespace Tests.Unit.Memory.Readers.Helpers;

using Backend.Infrastructure.Duckstation;
using Backend.Infrastructure.Memory;
using Backend.Memory;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Helpers;
using Moq;
using Xunit;

public class FlagByteHelperTests
{
    private static (Mock<IMemoryReader> MemoryReaderMock, IMemoryReader MemoryReader) CreateMemoryReaderMock()
    {
        var memoryReaderMock = new Mock<IMemoryReader>();
        return (memoryReaderMock, memoryReaderMock.Object);
    }

    [Fact]
    public void Read_ShouldReturnZero_WhenAddressIsZero()
    {
        var (_, memoryReader) = CreateMemoryReaderMock();

        var result = FlagByteHelper.Read(memoryReader, 0);

        Assert.Equal(0, result);
    }

    [Fact]
    public void Read_ShouldReturnRawByte_WhenMaskIsNull()
    {
        var (memoryReaderMock, memoryReader) = CreateMemoryReaderMock();
        memoryReaderMock.Setup(reader => reader.ReadBytes(0x800100, 1)).Returns([(byte)77]);

        var result = FlagByteHelper.Read(memoryReader, 0x800100, null);

        Assert.Equal(77, result);
    }

    [Fact]
    public void Read_ShouldApplyBitMask_WhenMaskIsProvided()
    {
        var (memoryReaderMock, memoryReader) = CreateMemoryReaderMock();
        memoryReaderMock.Setup(reader => reader.ReadBytes(0x800100, 1)).Returns([(byte)90]);

        var result = FlagByteHelper.Read(memoryReader, 0x800100, 15);

        Assert.Equal(10, result);
    }

    [Fact]
    public void Read_ShouldThrowMemoryReadException_WhenReadBytesFails()
    {
        var (memoryReaderMock, memoryReader) = CreateMemoryReaderMock();
        memoryReaderMock.Setup(reader => reader.ReadBytes(0x800100, 1))
            .Throws(new MemoryReadException(0x800100, "Memory session is not connected."));

        Assert.Throws<MemoryReadException>(() => FlagByteHelper.Read(memoryReader, 0x800100, null));
    }

    [Fact]
    public void Read_ShouldIntegrateWithMemoryReader_WhenConnected()
    {
        var duckstationSession = new DuckstationSession();
        var accessorMock = new Mock<IMemoryAccessor>();
        duckstationSession.SetAccessor(accessorMock.Object);
        accessorMock.Setup(accessor => accessor.ReadArray(0x800100, It.IsAny<byte[]>(), 0, 1))
            .Callback<long, byte[], int, int>((address, buffer, index, count) => { buffer[0] = 90; });

        var memoryReader = new MemoryReader(duckstationSession);

        var result = FlagByteHelper.Read(memoryReader, 0x800100, 15);

        Assert.Equal(10, result);
    }
}
