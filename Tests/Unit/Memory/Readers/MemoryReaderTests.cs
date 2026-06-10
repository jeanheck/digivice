namespace Tests.Memory.Readers;

using System;
using Backend.Infrastructure.Duckstation;
using Backend.Infrastructure.Memory;
using Backend.Memory;
using Backend.Memory.Readers;
using Moq;
using Xunit;

public class MemoryReaderTests
{
    private static (Mock<IDuckstationConnector> ConnectorMock, Mock<IMemoryAccessor> AccessorMock, MemoryReader Reader) CreateConnectedReader()
    {
        var connectorMock = new Mock<IDuckstationConnector>();
        var accessorMock = new Mock<IMemoryAccessor>();
        connectorMock.Setup(connector => connector.IsConnected).Returns(true);
        connectorMock.Setup(connector => connector.Accessor).Returns(accessorMock.Object);

        return (connectorMock, accessorMock, new MemoryReader(connectorMock.Object));
    }

    [Fact]
    public void ReadInt32_ShouldThrowMemoryReadException_WhenDisconnected()
    {
        var connectorMock = new Mock<IDuckstationConnector>();
        connectorMock.Setup(connector => connector.IsConnected).Returns(false);
        connectorMock.Setup(connector => connector.Accessor).Returns((IMemoryAccessor?)null);

        var reader = new MemoryReader(connectorMock.Object);

        var exception = Assert.Throws<MemoryReadException>(() => reader.ReadInt32(0x800100));

        Assert.Equal(0x800100, exception.Address);
    }

    [Fact]
    public void ReadByteSafe_ShouldApplyBitMask_WhenMaskIsProvided()
    {
        var (_, accessorMock, reader) = CreateConnectedReader();
        accessorMock.Setup(accessor => accessor.ReadArray(0x800100, It.IsAny<byte[]>(), 0, 1))
            .Callback<long, byte[], int, int>((address, buffer, index, count) => { buffer[0] = 90; });

        var result = reader.ReadByteSafe(0x800100, 15);

        Assert.Equal(10, result);
    }

    [Fact]
    public void ReadByteSafe_ShouldReturnZero_WhenAddressIsZero()
    {
        var connectorMock = new Mock<IDuckstationConnector>();
        var reader = new MemoryReader(connectorMock.Object);

        var result = reader.ReadByteSafe(0);

        Assert.Equal(0, result);
    }

    [Fact]
    public void ReadInt32_ShouldThrowMemoryReadException_WhenAccessorThrows()
    {
        var (_, accessorMock, reader) = CreateConnectedReader();
        accessorMock.Setup(accessor => accessor.ReadInt32(It.IsAny<long>())).Throws(new Exception("Physical mapped I/O error"));

        var exception = Assert.Throws<MemoryReadException>(() => reader.ReadInt32(0x800100));

        Assert.Equal(0x800100, exception.Address);
        Assert.NotNull(exception.InnerException);
    }

    [Fact]
    public void ReadInt32_ShouldReturnInt_WhenConnectedAndAccessorSucceeds()
    {
        var (_, accessorMock, reader) = CreateConnectedReader();
        accessorMock.Setup(accessor => accessor.ReadInt32(0x800100)).Returns(9999);

        var result = reader.ReadInt32(0x800100);

        Assert.Equal(9999, result);
    }

    [Fact]
    public void ReadInt16_ShouldReturnShort_WhenConnectedAndAccessorSucceeds()
    {
        var (_, accessorMock, reader) = CreateConnectedReader();
        accessorMock.Setup(accessor => accessor.ReadInt16(0x800100)).Returns((short)45);

        var result = reader.ReadInt16(0x800100);

        Assert.Equal((short)45, result);
    }

    [Fact]
    public void ReadInt16_ShouldThrowMemoryReadException_WhenAccessorThrows()
    {
        var (_, accessorMock, reader) = CreateConnectedReader();
        accessorMock.Setup(accessor => accessor.ReadInt16(It.IsAny<long>())).Throws(new Exception("I/O error"));

        Assert.Throws<MemoryReadException>(() => reader.ReadInt16(0x800100));
    }

    [Fact]
    public void ReadInt16_ShouldThrowMemoryReadException_WhenDisconnected()
    {
        var connectorMock = new Mock<IDuckstationConnector>();
        connectorMock.Setup(connector => connector.IsConnected).Returns(false);
        connectorMock.Setup(connector => connector.Accessor).Returns((IMemoryAccessor?)null);

        var reader = new MemoryReader(connectorMock.Object);

        Assert.Throws<MemoryReadException>(() => reader.ReadInt16(0x800100));
    }

    [Fact]
    public void ReadBytes_ShouldReturnBytes_WhenConnectedAndAccessorSucceeds()
    {
        var (_, accessorMock, reader) = CreateConnectedReader();
        var expectedBytes = new byte[] { 1, 2, 3 };
        accessorMock.Setup(accessor => accessor.ReadArray(0x800100, It.IsAny<byte[]>(), 0, 3))
            .Callback<long, byte[], int, int>((address, buffer, index, count) => { Array.Copy(expectedBytes, buffer, 3); });

        var result = reader.ReadBytes(0x800100, 3);

        Assert.Equal(expectedBytes, result);
    }

    [Fact]
    public void ReadBytes_ShouldThrowMemoryReadException_WhenAccessorThrows()
    {
        var (_, accessorMock, reader) = CreateConnectedReader();
        accessorMock.Setup(accessor => accessor.ReadArray(It.IsAny<long>(), It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
            .Throws(new Exception("I/O error"));

        Assert.Throws<MemoryReadException>(() => reader.ReadBytes(0x800100, 3));
    }

    [Fact]
    public void ReadBytes_ShouldThrowMemoryReadException_WhenDisconnected()
    {
        var connectorMock = new Mock<IDuckstationConnector>();
        connectorMock.Setup(connector => connector.IsConnected).Returns(false);
        connectorMock.Setup(connector => connector.Accessor).Returns((IMemoryAccessor?)null);

        var reader = new MemoryReader(connectorMock.Object);

        Assert.Throws<MemoryReadException>(() => reader.ReadBytes(0x800100, 3));
    }

    [Fact]
    public void ReadByteSafe_ShouldReturnRawByte_WhenMaskIsNull()
    {
        var (_, accessorMock, reader) = CreateConnectedReader();
        accessorMock.Setup(accessor => accessor.ReadArray(0x800100, It.IsAny<byte[]>(), 0, 1))
            .Callback<long, byte[], int, int>((address, buffer, index, count) => { buffer[0] = 77; });

        var result = reader.ReadByteSafe(0x800100, null);

        Assert.Equal(77, result);
    }

    [Fact]
    public void ReadByteSafe_ShouldThrowMemoryReadException_WhenReadBytesFails()
    {
        var (_, accessorMock, reader) = CreateConnectedReader();
        accessorMock.Setup(accessor => accessor.ReadArray(It.IsAny<long>(), It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
            .Throws(new Exception("I/O error"));

        Assert.Throws<MemoryReadException>(() => reader.ReadByteSafe(0x800100, null));
    }
}
