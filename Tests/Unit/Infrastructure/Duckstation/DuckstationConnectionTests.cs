namespace Tests.Unit.Infrastructure.Duckstation;

using Backend.Infrastructure.Duckstation;
using Backend.Infrastructure.Memory;
using Backend.Infrastructure.Processes;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

public class DuckstationConnectionTests
{
    [Fact]
    public void TryConnect_ShouldReturnTrue_WhenProcessAndAccessorExist()
    {
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(processService => processService.GetProcessIdByName("duckstation")).Returns(1234);

        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(memoryProvider => memoryProvider.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);

        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(section => section.Value).Returns("duckstation");
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(configuration => configuration.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);

        var connection = new DuckstationConnection(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object);

        var result = connection.TryConnect();

        Assert.True(result);
        Assert.True(connection.IsConnected);
        Assert.Same(memoryAccessorMock.Object, connection.Accessor);
    }

    [Fact]
    public void TryConnect_ShouldReturnFalse_WhenProcessNotFound()
    {
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(processService => processService.GetProcessIdByName("duckstation")).Returns((int?)null);

        var memoryProviderMock = new Mock<IMemoryProvider>();

        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(section => section.Value).Returns("duckstation");
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(configuration => configuration.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);

        var connection = new DuckstationConnection(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object);

        var result = connection.TryConnect();

        Assert.False(result);
        Assert.False(connection.IsConnected);
    }

    [Fact]
    public void IsConnectionAlive_ShouldReturnTrue_WhenConnectedProcessStillExists()
    {
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(processService => processService.GetProcessIdByName("duckstation")).Returns(1234);

        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(memoryProvider => memoryProvider.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);

        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(section => section.Value).Returns("duckstation");
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(configuration => configuration.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);

        var connection = new DuckstationConnection(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object);
        connection.TryConnect();

        Assert.True(connection.IsConnectionAlive());
    }

    [Fact]
    public void IsConnectionAlive_ShouldReturnFalse_WhenEmulatorProcessIsGone()
    {
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.SetupSequence(processService => processService.GetProcessIdByName("duckstation"))
            .Returns(1234)
            .Returns((int?)null);

        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(memoryProvider => memoryProvider.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);

        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(section => section.Value).Returns("duckstation");
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(configuration => configuration.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);

        var connection = new DuckstationConnection(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object);
        connection.TryConnect();

        Assert.False(connection.IsConnectionAlive());
    }

    [Fact]
    public void IsConnectionAlive_ShouldReturnFalse_WhenEmulatorProcessIdChanges()
    {
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.SetupSequence(processService => processService.GetProcessIdByName("duckstation"))
            .Returns(1234)
            .Returns(5678);

        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(memoryProvider => memoryProvider.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);

        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(section => section.Value).Returns("duckstation");
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(configuration => configuration.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);

        var connection = new DuckstationConnection(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object);
        connection.TryConnect();

        Assert.False(connection.IsConnectionAlive());
    }

    [Fact]
    public void Disconnect_ShouldSetIsConnectedFalse_AndDisposeAccessor()
    {
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(processService => processService.GetProcessIdByName("duckstation")).Returns(1234);

        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(memoryProvider => memoryProvider.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);

        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(section => section.Value).Returns("duckstation");
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(configuration => configuration.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);

        var connection = new DuckstationConnection(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object);
        connection.TryConnect();

        connection.Disconnect();

        Assert.False(connection.IsConnected);
        Assert.Null(connection.Accessor);
        memoryAccessorMock.Verify(accessor => accessor.Dispose(), Times.Once);
    }

    [Fact]
    public void Disconnect_ShouldBeIdempotent_WhenAlreadyDisconnected()
    {
        var processServiceMock = new Mock<IProcessService>();
        var memoryProviderMock = new Mock<IMemoryProvider>();

        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(section => section.Value).Returns("duckstation");
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(configuration => configuration.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);

        var connection = new DuckstationConnection(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object);

        connection.Disconnect();
        connection.Disconnect();

        Assert.False(connection.IsConnected);
    }

    [Fact]
    public void TryConnect_ShouldReturnFalse_WhenConfigValueIsEmpty()
    {
        var processServiceMock = new Mock<IProcessService>();
        var memoryProviderMock = new Mock<IMemoryProvider>();

        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(section => section.Value).Returns(string.Empty);
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(configuration => configuration.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);

        var connection = new DuckstationConnection(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object);

        var result = connection.TryConnect();

        Assert.False(result);
        Assert.False(connection.IsConnected);
    }

    [Fact]
    public void TryConnect_ShouldReturnFalse_WhenAccessorIsNull()
    {
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(processService => processService.GetProcessIdByName("duckstation")).Returns(1234);

        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(memoryProvider => memoryProvider.OpenExisting("duckstation_1234")).Returns((IMemoryAccessor?)null);

        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(section => section.Value).Returns("duckstation");
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(configuration => configuration.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);

        var connection = new DuckstationConnection(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object);

        var result = connection.TryConnect();

        Assert.False(result);
        Assert.False(connection.IsConnected);
    }

    [Fact]
    public void TryConnect_ShouldReturnFalse_WhenOpenExistingThrowsException()
    {
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(processService => processService.GetProcessIdByName("duckstation")).Returns(1234);

        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(memoryProvider => memoryProvider.OpenExisting("duckstation_1234")).Throws(new Exception("Open mapping error"));

        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(section => section.Value).Returns("duckstation");
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(configuration => configuration.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);

        var connection = new DuckstationConnection(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object);

        var result = connection.TryConnect();

        Assert.False(result);
        Assert.False(connection.IsConnected);
    }
}

