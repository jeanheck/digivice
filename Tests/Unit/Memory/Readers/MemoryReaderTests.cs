namespace Tests.Memory.Readers;

using System;
using Backend.Infrastructure.Duckstation;
using Backend.Infrastructure.Memory;
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
    public void ReadInt32_ShouldReturnNull_WhenDisconnected()
    {
        var connectorMock = new Mock<IDuckstationConnector>();
        connectorMock.Setup(connector => connector.IsConnected).Returns(false);
        connectorMock.Setup(connector => connector.Accessor).Returns((IMemoryAccessor?)null);

        var reader = new MemoryReader(connectorMock.Object);

        var result = reader.ReadInt32(0x800100);

        Assert.Null(result);
    }

    [Fact]
    public void ReadByteSafe_ShouldApplyBitMask_WhenMaskIsProvided()
    {
        var (connectorMock, accessorMock, reader) = CreateConnectedReader();
        accessorMock.Setup(accessor => accessor.ReadArray(0x800100, It.IsAny<byte[]>(), 0, 1))
            .Callback<long, byte[], int, int>((address, buffer, index, count) => { buffer[0] = 90; });

        var result = reader.ReadByteSafe(0x800100, 15);

        Assert.Equal(10, result);
        connectorMock.Verify(connector => connector.InvalidateConnection(), Times.Never);
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
    public void ReadInt32_ShouldInvalidateConnectionAndReturnNull_WhenAccessorThrows()
    {
        var (connectorMock, accessorMock, reader) = CreateConnectedReader();
        accessorMock.Setup(accessor => accessor.ReadInt32(It.IsAny<long>())).Throws(new Exception("Physical mapped I/O error"));

        var result = reader.ReadInt32(0x800100);

        Assert.Null(result);
        connectorMock.Verify(connector => connector.InvalidateConnection(), Times.Once);
    }

    [Fact]
    public void ReadInt32_ShouldReturnInt_WhenConnectedAndAccessorSucceeds()
    {
        var (connectorMock, accessorMock, reader) = CreateConnectedReader();
        accessorMock.Setup(accessor => accessor.ReadInt32(0x800100)).Returns(9999);

        var result = reader.ReadInt32(0x800100);

        Assert.Equal(9999, result);
        connectorMock.Verify(connector => connector.InvalidateConnection(), Times.Never);
    }

    [Fact]
    public void ReadInt16_ShouldReturnShort_WhenConnectedAndAccessorSucceeds()
    {
        var (connectorMock, accessorMock, reader) = CreateConnectedReader();
        accessorMock.Setup(accessor => accessor.ReadInt16(0x800100)).Returns((short)45);

        var result = reader.ReadInt16(0x800100);

        Assert.Equal((short)45, result);
        connectorMock.Verify(connector => connector.InvalidateConnection(), Times.Never);
    }

    [Fact]
    public void ReadInt16_ShouldInvalidateConnectionAndReturnNull_WhenAccessorThrows()
    {
        var (connectorMock, accessorMock, reader) = CreateConnectedReader();
        accessorMock.Setup(accessor => accessor.ReadInt16(It.IsAny<long>())).Throws(new Exception("I/O error"));

        var result = reader.ReadInt16(0x800100);

        Assert.Null(result);
        connectorMock.Verify(connector => connector.InvalidateConnection(), Times.Once);
    }

    [Fact]
    public void ReadInt16_ShouldReturnNull_WhenDisconnected()
    {
        var connectorMock = new Mock<IDuckstationConnector>();
        connectorMock.Setup(connector => connector.IsConnected).Returns(false);
        connectorMock.Setup(connector => connector.Accessor).Returns((IMemoryAccessor?)null);

        var reader = new MemoryReader(connectorMock.Object);

        var result = reader.ReadInt16(0x800100);

        Assert.Null(result);
    }

    [Fact]
    public void ReadBytes_ShouldReturnBytes_WhenConnectedAndAccessorSucceeds()
    {
        var (connectorMock, accessorMock, reader) = CreateConnectedReader();
        var expectedBytes = new byte[] { 1, 2, 3 };
        accessorMock.Setup(accessor => accessor.ReadArray(0x800100, It.IsAny<byte[]>(), 0, 3))
            .Callback<long, byte[], int, int>((address, buffer, index, count) => { Array.Copy(expectedBytes, buffer, 3); });

        var result = reader.ReadBytes(0x800100, 3);

        Assert.Equal(expectedBytes, result);
        connectorMock.Verify(connector => connector.InvalidateConnection(), Times.Never);
    }

    [Fact]
    public void ReadBytes_ShouldInvalidateConnectionAndReturnNull_WhenAccessorThrows()
    {
        var (connectorMock, accessorMock, reader) = CreateConnectedReader();
        accessorMock.Setup(accessor => accessor.ReadArray(It.IsAny<long>(), It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
            .Throws(new Exception("I/O error"));

        var result = reader.ReadBytes(0x800100, 3);

        Assert.Null(result);
        connectorMock.Verify(connector => connector.InvalidateConnection(), Times.Once);
    }

    [Fact]
    public void ReadBytes_ShouldReturnNull_WhenDisconnected()
    {
        var connectorMock = new Mock<IDuckstationConnector>();
        connectorMock.Setup(connector => connector.IsConnected).Returns(false);
        connectorMock.Setup(connector => connector.Accessor).Returns((IMemoryAccessor?)null);

        var reader = new MemoryReader(connectorMock.Object);

        var result = reader.ReadBytes(0x800100, 3);

        Assert.Null(result);
    }

    [Fact]
    public void ReadByteSafe_ShouldReturnRawByte_WhenMaskIsNull()
    {
        var (connectorMock, accessorMock, reader) = CreateConnectedReader();
        accessorMock.Setup(accessor => accessor.ReadArray(0x800100, It.IsAny<byte[]>(), 0, 1))
            .Callback<long, byte[], int, int>((address, buffer, index, count) => { buffer[0] = 77; });

        var result = reader.ReadByteSafe(0x800100, null);

        Assert.Equal(77, result);
        connectorMock.Verify(connector => connector.InvalidateConnection(), Times.Never);
    }

    [Fact]
    public void ReadByteSafe_ShouldReturnZero_WhenReadBytesReturnsNullOrEmpty()
    {
        var (connectorMock, accessorMock, reader) = CreateConnectedReader();
        accessorMock.Setup(accessor => accessor.ReadArray(0x800100, It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()));

        var result = reader.ReadByteSafe(0x800100, null);

        Assert.Equal(0, result);
    }

    [Fact]
    public void ReadByteSafe_ShouldReturnZero_WhenReadBytesThrowsException()
    {
        var (connectorMock, accessorMock, reader) = CreateConnectedReader();
        accessorMock.Setup(accessor => accessor.ReadArray(It.IsAny<long>(), It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
            .Throws(new Exception("I/O error"));

        var result = reader.ReadByteSafe(0x800100, null);

        Assert.Equal(0, result);
        connectorMock.Verify(connector => connector.InvalidateConnection(), Times.Once);
    }
}
