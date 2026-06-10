namespace Tests.Unit.Infrastructure.Duckstation;

using Backend.Infrastructure.Duckstation;
using Backend.Infrastructure.Memory;
using Backend.Infrastructure.Processes;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

public class DuckstationConnectorTests
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

        var connector = new DuckstationConnector(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object);

        var result = connector.TryConnect();

        Assert.True(result);
        Assert.True(connector.IsConnected);
        Assert.Same(memoryAccessorMock.Object, connector.Accessor);
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

        var connector = new DuckstationConnector(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object);

        var result = connector.TryConnect();

        Assert.False(result);
        Assert.False(connector.IsConnected);
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

        var connector = new DuckstationConnector(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object);
        connector.TryConnect();

        Assert.True(connector.IsConnectionAlive());
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

        var connector = new DuckstationConnector(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object);
        connector.TryConnect();

        Assert.False(connector.IsConnectionAlive());
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

        var connector = new DuckstationConnector(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object);
        connector.TryConnect();

        Assert.False(connector.IsConnectionAlive());
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

        var connector = new DuckstationConnector(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object);
        connector.TryConnect();

        connector.Disconnect();

        Assert.False(connector.IsConnected);
        Assert.Null(connector.Accessor);
        memoryAccessorMock.Verify(accessor => accessor.Dispose(), Times.Once);
    }

    [Fact]
    public void Disconnect_ShouldBeIdempotent_WhenAlreadyDisconnected()
    {
        var processServiceMock = new Mock<IProcessService>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        var configurationMock = new Mock<IConfiguration>();

        var connector = new DuckstationConnector(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object);

        connector.Disconnect();
        connector.Disconnect();

        Assert.False(connector.IsConnected);
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

        var connector = new DuckstationConnector(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object);

        var result = connector.TryConnect();

        Assert.False(result);
        Assert.False(connector.IsConnected);
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

        var connector = new DuckstationConnector(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object);

        var result = connector.TryConnect();

        Assert.False(result);
        Assert.False(connector.IsConnected);
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

        var connector = new DuckstationConnector(
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object);

        var result = connector.TryConnect();

        Assert.False(result);
        Assert.False(connector.IsConnected);
    }
}

