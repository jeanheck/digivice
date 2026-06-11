namespace Tests.Unit.Infrastructure.Duckstation;

using Backend.Infrastructure.Duckstation;
using Backend.Infrastructure.Memory;
using Backend.Infrastructure.Processes;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

public class DuckstationConnectorTests
{
    private static DuckstationConnector CreateConnector(
        DuckstationSession duckstationSession,
        Mock<IProcessService> processServiceMock,
        Mock<IMemoryProvider> memoryProviderMock,
        Mock<IConfiguration> configurationMock)
    {
        return new DuckstationConnector(
            duckstationSession,
            processServiceMock.Object,
            memoryProviderMock.Object,
            configurationMock.Object);
    }

    private static Mock<IConfiguration> CreateConfigurationMock(string emulatorProcessName)
    {
        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(section => section.Value).Returns(emulatorProcessName);
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(configuration => configuration.GetSection("EmulatorProcessName")).Returns(configSectionMock.Object);
        return configurationMock;
    }

    [Fact]
    public void EnsureConnection_ShouldReturnTrue_WhenProcessAndAccessorExist()
    {
        var duckstationSession = new DuckstationSession();
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(processService => processService.GetProcessIdByName("duckstation")).Returns(1234);

        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(memoryProvider => memoryProvider.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);

        var connector = CreateConnector(
            duckstationSession,
            processServiceMock,
            memoryProviderMock,
            CreateConfigurationMock("duckstation"));

        var result = connector.EnsureConnection();

        Assert.True(result);
        Assert.Same(memoryAccessorMock.Object, duckstationSession.Accessor);
    }

    [Fact]
    public void EnsureConnection_ShouldReturnFalse_WhenProcessNotFound()
    {
        var duckstationSession = new DuckstationSession();
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(processService => processService.GetProcessIdByName("duckstation")).Returns((int?)null);

        var memoryProviderMock = new Mock<IMemoryProvider>();

        var connector = CreateConnector(
            duckstationSession,
            processServiceMock,
            memoryProviderMock,
            CreateConfigurationMock("duckstation"));

        var result = connector.EnsureConnection();

        Assert.False(result);
        Assert.Null(duckstationSession.Accessor);
    }

    [Fact]
    public void EnsureConnection_ShouldReturnTrue_WhenConnectedProcessStillExists()
    {
        var duckstationSession = new DuckstationSession();
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(processService => processService.GetProcessIdByName("duckstation")).Returns(1234);

        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(memoryProvider => memoryProvider.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);

        var connector = CreateConnector(
            duckstationSession,
            processServiceMock,
            memoryProviderMock,
            CreateConfigurationMock("duckstation"));

        Assert.True(connector.EnsureConnection());
        Assert.True(connector.EnsureConnection());
    }

    [Fact]
    public void EnsureConnection_ShouldReturnFalse_WhenEmulatorProcessIsGone()
    {
        var duckstationSession = new DuckstationSession();
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.SetupSequence(processService => processService.GetProcessIdByName("duckstation"))
            .Returns(1234)
            .Returns((int?)null);

        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(memoryProvider => memoryProvider.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);

        var connector = CreateConnector(
            duckstationSession,
            processServiceMock,
            memoryProviderMock,
            CreateConfigurationMock("duckstation"));

        Assert.True(connector.EnsureConnection());

        var result = connector.EnsureConnection();

        Assert.False(result);
        Assert.Null(duckstationSession.Accessor);
    }

    [Fact]
    public void EnsureConnection_ShouldReturnFalse_WhenEmulatorProcessIdChangesAndMappingIsMissing()
    {
        var duckstationSession = new DuckstationSession();
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.SetupSequence(processService => processService.GetProcessIdByName("duckstation"))
            .Returns(1234)
            .Returns(5678);

        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(memoryProvider => memoryProvider.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);

        var connector = CreateConnector(
            duckstationSession,
            processServiceMock,
            memoryProviderMock,
            CreateConfigurationMock("duckstation"));

        Assert.True(connector.EnsureConnection());

        var result = connector.EnsureConnection();

        Assert.False(result);
        Assert.Null(duckstationSession.Accessor);
    }

    [Fact]
    public void EnsureConnection_ShouldReturnTrue_WhenEmulatorProcessIdChangesAndMappingExists()
    {
        var duckstationSession = new DuckstationSession();
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.SetupSequence(processService => processService.GetProcessIdByName("duckstation"))
            .Returns(1234)
            .Returns(5678)
            .Returns(5678);

        var oldMemoryAccessorMock = new Mock<IMemoryAccessor>();
        var newMemoryAccessorMock = new Mock<IMemoryAccessor>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(memoryProvider => memoryProvider.OpenExisting("duckstation_1234")).Returns(oldMemoryAccessorMock.Object);
        memoryProviderMock.Setup(memoryProvider => memoryProvider.OpenExisting("duckstation_5678")).Returns(newMemoryAccessorMock.Object);

        var connector = CreateConnector(
            duckstationSession,
            processServiceMock,
            memoryProviderMock,
            CreateConfigurationMock("duckstation"));

        Assert.True(connector.EnsureConnection());

        var result = connector.EnsureConnection();

        Assert.True(result);
        Assert.Same(newMemoryAccessorMock.Object, duckstationSession.Accessor);
        oldMemoryAccessorMock.Verify(accessor => accessor.Dispose(), Times.Once);
    }

    [Fact]
    public void ClearSession_ShouldClearSessionAccessor_AndDisposeAccessor()
    {
        var duckstationSession = new DuckstationSession();
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(processService => processService.GetProcessIdByName("duckstation")).Returns(1234);

        var memoryAccessorMock = new Mock<IMemoryAccessor>();
        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(memoryProvider => memoryProvider.OpenExisting("duckstation_1234")).Returns(memoryAccessorMock.Object);

        var connector = CreateConnector(
            duckstationSession,
            processServiceMock,
            memoryProviderMock,
            CreateConfigurationMock("duckstation"));
        connector.EnsureConnection();

        connector.ClearSession();

        Assert.Null(duckstationSession.Accessor);
        memoryAccessorMock.Verify(accessor => accessor.Dispose(), Times.Once);
    }

    [Fact]
    public void ClearSession_ShouldBeIdempotent_WhenAlreadyDisconnected()
    {
        var duckstationSession = new DuckstationSession();
        var processServiceMock = new Mock<IProcessService>();
        var memoryProviderMock = new Mock<IMemoryProvider>();

        var connector = CreateConnector(
            duckstationSession,
            processServiceMock,
            memoryProviderMock,
            CreateConfigurationMock("duckstation"));

        connector.ClearSession();
        connector.ClearSession();

        Assert.Null(duckstationSession.Accessor);
    }

    [Fact]
    public void EnsureConnection_ShouldReturnFalse_WhenConfigValueIsEmpty()
    {
        var duckstationSession = new DuckstationSession();
        var processServiceMock = new Mock<IProcessService>();
        var memoryProviderMock = new Mock<IMemoryProvider>();

        var connector = CreateConnector(
            duckstationSession,
            processServiceMock,
            memoryProviderMock,
            CreateConfigurationMock(string.Empty));

        var result = connector.EnsureConnection();

        Assert.False(result);
        Assert.Null(duckstationSession.Accessor);
    }

    [Fact]
    public void EnsureConnection_ShouldReturnFalse_WhenAccessorIsNull()
    {
        var duckstationSession = new DuckstationSession();
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(processService => processService.GetProcessIdByName("duckstation")).Returns(1234);

        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(memoryProvider => memoryProvider.OpenExisting("duckstation_1234")).Returns((IMemoryAccessor?)null);

        var connector = CreateConnector(
            duckstationSession,
            processServiceMock,
            memoryProviderMock,
            CreateConfigurationMock("duckstation"));

        var result = connector.EnsureConnection();

        Assert.False(result);
        Assert.Null(duckstationSession.Accessor);
    }

    [Fact]
    public void EnsureConnection_ShouldReturnFalse_WhenOpenExistingThrowsException()
    {
        var duckstationSession = new DuckstationSession();
        var processServiceMock = new Mock<IProcessService>();
        processServiceMock.Setup(processService => processService.GetProcessIdByName("duckstation")).Returns(1234);

        var memoryProviderMock = new Mock<IMemoryProvider>();
        memoryProviderMock.Setup(memoryProvider => memoryProvider.OpenExisting("duckstation_1234")).Throws(new Exception("Open mapping error"));

        var connector = CreateConnector(
            duckstationSession,
            processServiceMock,
            memoryProviderMock,
            CreateConfigurationMock("duckstation"));

        var result = connector.EnsureConnection();

        Assert.False(result);
        Assert.Null(duckstationSession.Accessor);
    }
}
