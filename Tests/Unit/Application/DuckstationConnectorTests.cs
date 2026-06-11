namespace Tests.Unit.Application;

using Backend.Application;
using Backend.Diagnostics;
using Backend.Events.Services;
using Backend.Infrastructure.Duckstation;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

public class DuckstationConnectorTests
{
    private readonly Mock<IDuckstationConnection> DuckstationConnectionMock;
    private readonly Mock<IEventDispatcherService> EventDispatcherServiceMock;
    private readonly DebugConsoleRenderer DebugConsoleRenderer;
    private readonly IConfiguration Configuration;

    public DuckstationConnectorTests()
    {
        DuckstationConnectionMock = new Mock<IDuckstationConnection>();
        EventDispatcherServiceMock = new Mock<IEventDispatcherService>();
        DebugConsoleRenderer = new DebugConsoleRenderer();

        var inMemorySettings = new Dictionary<string, string?>
        {
            { "Features:Debugging", "false" }
        };
        Configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }

    private DuckstationConnector CreateDuckstationConnector()
    {
        return new DuckstationConnector(
            DuckstationConnectionMock.Object,
            EventDispatcherServiceMock.Object,
            DebugConsoleRenderer,
            Configuration);
    }

    [Fact]
    public void EnsureConnection_ShouldReturnTrue_WhenConnectedAndAlive()
    {
        DuckstationConnectionMock.Setup(connection => connection.IsConnected).Returns(true);
        DuckstationConnectionMock.Setup(connection => connection.IsConnectionAlive()).Returns(true);

        var duckstationConnector = CreateDuckstationConnector();

        var isReadyToCompose = duckstationConnector.EnsureConnection();

        Assert.True(isReadyToCompose);
        EventDispatcherServiceMock.Verify(
            dispatcher => dispatcher.DispatchEmulatorConnectionStatus(It.IsAny<bool>()),
            Times.Never);
    }

    [Fact]
    public void EnsureConnection_ShouldReturnFalse_WhenConnectedButNotAlive()
    {
        DuckstationConnectionMock.Setup(connection => connection.IsConnected).Returns(true);
        DuckstationConnectionMock.Setup(connection => connection.IsConnectionAlive()).Returns(false);

        var duckstationConnector = CreateDuckstationConnector();

        var isReadyToCompose = duckstationConnector.EnsureConnection();

        Assert.False(isReadyToCompose);
        DuckstationConnectionMock.Verify(connection => connection.Disconnect(), Times.Once);
        EventDispatcherServiceMock.Verify(dispatcher => dispatcher.DispatchEmulatorConnectionStatus(false), Times.Once);
    }

    [Fact]
    public void EnsureConnection_ShouldReturnFalse_WhenTryConnectFails()
    {
        DuckstationConnectionMock.Setup(connection => connection.IsConnected).Returns(false);
        DuckstationConnectionMock.Setup(connection => connection.TryConnect()).Returns(false);

        var duckstationConnector = CreateDuckstationConnector();

        var isReadyToCompose = duckstationConnector.EnsureConnection();

        Assert.False(isReadyToCompose);
        DuckstationConnectionMock.Verify(connection => connection.Disconnect(), Times.Never);
        EventDispatcherServiceMock.Verify(dispatcher => dispatcher.DispatchEmulatorConnectionStatus(false), Times.Once);
    }

    [Fact]
    public void EnsureConnection_ShouldReturnTrue_WhenTryConnectSucceeds()
    {
        DuckstationConnectionMock.Setup(connection => connection.IsConnected).Returns(false);
        DuckstationConnectionMock.SetupSequence(connection => connection.TryConnect())
            .Returns(true)
            .Returns(true);
        DuckstationConnectionMock.Setup(connection => connection.IsConnectionAlive()).Returns(true);

        var duckstationConnector = CreateDuckstationConnector();

        var isReadyToCompose = duckstationConnector.EnsureConnection();

        Assert.True(isReadyToCompose);
        EventDispatcherServiceMock.Verify(dispatcher => dispatcher.DispatchEmulatorConnectionStatus(true), Times.Once);
    }

    [Fact]
    public void HandleProcessingFailure_ShouldDisconnectAndDispatchFalse()
    {
        var duckstationConnector = CreateDuckstationConnector();

        duckstationConnector.HandleProcessingFailure(new Exception("RAM read error"));

        DuckstationConnectionMock.Verify(connection => connection.Disconnect(), Times.Once);
        EventDispatcherServiceMock.Verify(dispatcher => dispatcher.DispatchEmulatorConnectionStatus(false), Times.Once);
    }

    [Fact]
    public void HandleMemoryReadFailure_ShouldDisconnectDispatchFalse()
    {
        var duckstationConnector = CreateDuckstationConnector();

        duckstationConnector.HandleMemoryReadFailure();

        DuckstationConnectionMock.Verify(connection => connection.Disconnect(), Times.Once);
        EventDispatcherServiceMock.Verify(dispatcher => dispatcher.DispatchEmulatorConnectionStatus(false), Times.Once);
    }
}
