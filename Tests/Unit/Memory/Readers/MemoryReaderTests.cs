namespace Tests.Memory.Readers;

using System;
using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using Backend.Memory.Readers;
using Backend.Infrastructure.Processes;
using Backend.Infrastructure.Memory;

public class MemoryReaderTests
{
    [Fact]
    public void TryConnect_ShouldReturnTrue_WhenProcessAndAccessorExist()
    {
        // Arrange
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(p => p.GetProcessIdByName("duckstation")).Returns(1234);

        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(m => m.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);

        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(s => s.Value).Returns("duckstation");
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);

        var reader = new MemoryReader(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object
        );

        // Act
        var result = reader.TryConnect();

        // Assert
        Assert.True(result);
        Assert.True(reader.IsConnected);
    }

    [Fact]
    public void TryConnect_ShouldReturnFalse_WhenProcessNotFound()
    {
        // Arrange
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(p => p.GetProcessIdByName("duckstation")).Returns((int?)null);

        var memoryProviderMock = new Mock<IMemoryProvider>();

        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(s => s.Value).Returns("duckstation");
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);

        var reader = new MemoryReader(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object
        );

        // Act
        var result = reader.TryConnect();

        // Assert
        Assert.False(result);
        Assert.False(reader.IsConnected);
    }

    [Fact]
    public void Disconnect_ShouldSetIsConnectedFalse_AndDisposeAccessor()
    {
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(p => p.GetProcessIdByName("duckstation")).Returns(1234);

        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(m => m.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);

        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(s => s.Value).Returns("duckstation");
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);

        var reader = new MemoryReader(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object
        );
        reader.TryConnect();

        reader.Disconnect();

        Assert.False(reader.IsConnected);
        Assert.Null(reader.ReadInt32(0x800100));
        memoryAccessorMock.Verify(a => a.Dispose(), Times.Once);
    }

    [Fact]
    public void Disconnect_ShouldBeIdempotent_WhenAlreadyDisconnected()
    {
        var processServiceMock = new Mock<IProcessService>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        var configurationMock = new Mock<IConfiguration>();

        var reader = new MemoryReader(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object
        );

        reader.Disconnect();
        reader.Disconnect();

        Assert.False(reader.IsConnected);
    }

    [Fact]
    public void ReadInt32_ShouldReturnNull_WhenDisconnected()
    {
        // Arrange
        var processServiceMock = new Mock<IProcessService>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        var configurationMock = new Mock<IConfiguration>();

        var reader = new MemoryReader(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object
        );

        // Act
        var result = reader.ReadInt32(0x800100);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ReadByteSafe_ShouldApplyBitMask_WhenMaskIsProvided()
    {
        // Arrange
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(p => p.GetProcessIdByName("duckstation")).Returns(1234);

        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        // Simular a leitura de array retornando byte 0b0101_1010 (90 em decimal)
        memoryAccessorMock.Setup(m => m.ReadArray(0x800100, It.IsAny<byte[]>(), 0, 1))
            .Callback<long, byte[], int, int>((addr, buf, idx, cnt) => { buf[0] = 90; });

        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(m => m.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);

        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(s => s.Value).Returns("duckstation");
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);

        var reader = new MemoryReader(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object
        );
        reader.TryConnect();

        // Act (90 & 0b0000_1111 => 90 & 15 => 10)
        var result = reader.ReadByteSafe(0x800100, 15);

        // Assert
        Assert.Equal(10, result);
    }

    [Fact]
    public void ReadByteSafe_ShouldReturnZero_WhenAddressIsZero()
    {
        // Arrange
        var processServiceMock = new Mock<IProcessService>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        var configurationMock = new Mock<IConfiguration>();

        var reader = new MemoryReader(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object
        );

        // Act
        var result = reader.ReadByteSafe(0);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void ReadInt32_ShouldDisconnectAndReturnNull_WhenAccessorThrows()
    {
        // Arrange
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(p => p.GetProcessIdByName("duckstation")).Returns(1234);

        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        memoryAccessorMock.Setup(m => m.ReadInt32(It.IsAny<long>())).Throws(new Exception("Physical mapped I/O error"));

        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(m => m.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);

        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(s => s.Value).Returns("duckstation");
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);

        var reader = new MemoryReader(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object
        );
        reader.TryConnect();

        // Act
        var result = reader.ReadInt32(0x800100);

        // Assert
        Assert.Null(result);
        Assert.False(reader.IsConnected); // Conexão deve ser perdida
    }

    [Fact]
    public void TryConnect_ShouldReturnFalse_WhenConfigValueIsEmpty()
    {
        // Arrange
        var processServiceMock = new Mock<IProcessService>();
        var memoryProviderMock = new Mock<IMemoryProvider>();

        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(s => s.Value).Returns(string.Empty); // Nome do emulador vazio
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);

        var reader = new MemoryReader(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object
        );

        // Act
        var result = reader.TryConnect();

        // Assert
        Assert.False(result);
        Assert.False(reader.IsConnected);
    }

    [Fact]
    public void TryConnect_ShouldReturnFalse_WhenAccessorIsNull()
    {
        // Arrange
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(p => p.GetProcessIdByName("duckstation")).Returns(1234);

        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(m => m.OpenExisting("duckstation_1234")).Returns((IMemoryAccessor?)null); // Retorna nulo

        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(s => s.Value).Returns("duckstation");
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);

        var reader = new MemoryReader(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object
        );

        // Act
        var result = reader.TryConnect();

        // Assert
        Assert.False(result);
        Assert.False(reader.IsConnected);
    }

    [Fact]
    public void TryConnect_ShouldReturnFalse_WhenOpenExistingThrowsException()
    {
        // Arrange
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(p => p.GetProcessIdByName("duckstation")).Returns(1234);

        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(m => m.OpenExisting("duckstation_1234")).Throws(new Exception("Open mapping error"));

        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(s => s.Value).Returns("duckstation");
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);

        var reader = new MemoryReader(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object
        );

        // Act
        var result = reader.TryConnect();

        // Assert
        Assert.False(result);
        Assert.False(reader.IsConnected);
    }

    [Fact]
    public void ReadInt32_ShouldReturnInt_WhenConnectedAndAccessorSucceeds()
    {
        // Arrange
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(p => p.GetProcessIdByName("duckstation")).Returns(1234);

        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        memoryAccessorMock.Setup(m => m.ReadInt32(0x800100)).Returns(9999);

        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(m => m.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);

        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(s => s.Value).Returns("duckstation");
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);

        var reader = new MemoryReader(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object
        );
        reader.TryConnect();

        // Act
        var result = reader.ReadInt32(0x800100);

        // Assert
        Assert.Equal(9999, result);
    }

    [Fact]
    public void ReadInt16_ShouldReturnShort_WhenConnectedAndAccessorSucceeds()
    {
        // Arrange
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(p => p.GetProcessIdByName("duckstation")).Returns(1234);

        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        memoryAccessorMock.Setup(m => m.ReadInt16(0x800100)).Returns((short)45);

        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(m => m.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);

        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(s => s.Value).Returns("duckstation");
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);

        var reader = new MemoryReader(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object
        );
        reader.TryConnect();

        // Act
        var result = reader.ReadInt16(0x800100);

        // Assert
        Assert.Equal((short)45, result);
    }

    [Fact]
    public void ReadInt16_ShouldDisconnectAndReturnNull_WhenAccessorThrows()
    {
        // Arrange
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(p => p.GetProcessIdByName("duckstation")).Returns(1234);

        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        memoryAccessorMock.Setup(m => m.ReadInt16(It.IsAny<long>())).Throws(new Exception("I/O error"));

        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(m => m.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);

        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(s => s.Value).Returns("duckstation");
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);

        var reader = new MemoryReader(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object
        );
        reader.TryConnect();

        // Act
        var result = reader.ReadInt16(0x800100);

        // Assert
        Assert.Null(result);
        Assert.False(reader.IsConnected);
    }

    [Fact]
    public void ReadInt16_ShouldReturnNull_WhenDisconnected()
    {
        // Arrange
        var processServiceMock = new Mock<IProcessService>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        var configurationMock = new Mock<IConfiguration>();

        var reader = new MemoryReader(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object
        );

        // Act
        var result = reader.ReadInt16(0x800100);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ReadBytes_ShouldReturnBytes_WhenConnectedAndAccessorSucceeds()
    {
        // Arrange
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(p => p.GetProcessIdByName("duckstation")).Returns(1234);

        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        var expectedBytes = new byte[] { 1, 2, 3 };
        memoryAccessorMock.Setup(m => m.ReadArray(0x800100, It.IsAny<byte[]>(), 0, 3))
            .Callback<long, byte[], int, int>((addr, buf, idx, cnt) => { Array.Copy(expectedBytes, buf, 3); });

        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(m => m.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);

        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(s => s.Value).Returns("duckstation");
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);

        var reader = new MemoryReader(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object
        );
        reader.TryConnect();

        // Act
        var result = reader.ReadBytes(0x800100, 3);

        // Assert
        Assert.Equal(expectedBytes, result);
    }

    [Fact]
    public void ReadBytes_ShouldDisconnectAndReturnNull_WhenAccessorThrows()
    {
        // Arrange
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(p => p.GetProcessIdByName("duckstation")).Returns(1234);

        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        memoryAccessorMock.Setup(m => m.ReadArray(It.IsAny<long>(), It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
            .Throws(new Exception("I/O error"));

        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(m => m.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);

        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(s => s.Value).Returns("duckstation");
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);

        var reader = new MemoryReader(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object
        );
        reader.TryConnect();

        // Act
        var result = reader.ReadBytes(0x800100, 3);

        // Assert
        Assert.Null(result);
        Assert.False(reader.IsConnected);
    }

    [Fact]
    public void ReadBytes_ShouldReturnNull_WhenDisconnected()
    {
        // Arrange
        var processServiceMock = new Mock<IProcessService>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        var configurationMock = new Mock<IConfiguration>();

        var reader = new MemoryReader(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object
        );

        // Act
        var result = reader.ReadBytes(0x800100, 3);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ReadByteSafe_ShouldReturnRawByte_WhenMaskIsNull()
    {
        // Arrange
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(p => p.GetProcessIdByName("duckstation")).Returns(1234);

        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        memoryAccessorMock.Setup(m => m.ReadArray(0x800100, It.IsAny<byte[]>(), 0, 1))
            .Callback<long, byte[], int, int>((addr, buf, idx, cnt) => { buf[0] = 77; });

        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(m => m.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);

        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(s => s.Value).Returns("duckstation");
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);

        var reader = new MemoryReader(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object
        );
        reader.TryConnect();

        // Act
        var result = reader.ReadByteSafe(0x800100, null);

        // Assert
        Assert.Equal(77, result);
    }

    [Fact]
    public void ReadByteSafe_ShouldReturnZero_WhenReadBytesReturnsNullOrEmpty()
    {
        // Arrange
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(p => p.GetProcessIdByName("duckstation")).Returns(1234);

        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        // Simular que a leitura retorna um array vazio (ou nulo) por falha interna
        memoryAccessorMock.Setup(m => m.ReadArray(0x800100, It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()));

        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(m => m.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);

        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(s => s.Value).Returns("duckstation");
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);

        var reader = new MemoryReader(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object
        );
        reader.TryConnect();

        // Act
        var result = reader.ReadByteSafe(0x800100, null);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void ReadByteSafe_ShouldReturnZero_WhenReadBytesThrowsException()
    {
        // Arrange
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(p => p.GetProcessIdByName("duckstation")).Returns(1234);

        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        memoryAccessorMock.Setup(m => m.ReadArray(It.IsAny<long>(), It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
            .Throws(new Exception("I/O error"));

        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(m => m.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);

        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(s => s.Value).Returns("duckstation");
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(c => c.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);

        var reader = new MemoryReader(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object
        );
        reader.TryConnect();

        // Act
        var result = reader.ReadByteSafe(0x800100, null);

        // Assert
        Assert.Equal(0, result);
    }
}
