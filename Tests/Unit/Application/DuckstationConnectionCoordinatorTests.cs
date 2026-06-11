namespace Tests.Unit.Application;

using Backend.Application;
using Backend.Diagnostics;
using Backend.Events.Services;
using Backend.Infrastructure.Duckstation;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

public class DuckstationConnectionCoordinatorTests
{
    private readonly Mock<IDuckstationConnector> DuckstationConnectorMock;
    private readonly Mock<IEventDispatcherService> EventDispatcherServiceMock;
    private readonly DebugConsoleRenderer DebugConsoleRenderer;
    private readonly IConfiguration Configuration;

    public DuckstationConnectionCoordinatorTests()
    {
        DuckstationConnectorMock = new Mock<IDuckstationConnector>();
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

    private DuckstationConnectionHandler CreateCoordinator()
    {
        return new DuckstationConnectionHandler(
            DuckstationConnectorMock.Object,
            EventDispatcherServiceMock.Object,
            DebugConsoleRenderer,
            Configuration);
    }

    [Fact]
    public void GetConnectionStatus_ShouldReturnReady_WhenConnectedAndAlive()
    {
        DuckstationConnectorMock.Setup(connector => connector.IsConnected).Returns(true);
        DuckstationConnectorMock.Setup(connector => connector.IsConnectionAlive()).Returns(true);

        var coordinator = CreateCoordinator();

        var status = coordinator.Handle();

        Assert.Equal(DuckstationConnectionStatus.Ready, status);
        EventDispatcherServiceMock.Verify(
            dispatcher => dispatcher.DispatchEmulatorConnectionStatus(It.IsAny<bool>()),
            Times.Never);
    }

    [Fact]
    public void GetConnectionStatus_ShouldReturnConnectionLost_WhenConnectedButNotAlive()
    {
        DuckstationConnectorMock.Setup(connector => connector.IsConnected).Returns(true);
        DuckstationConnectorMock.Setup(connector => connector.IsConnectionAlive()).Returns(false);

        var coordinator = CreateCoordinator();

        var status = coordinator.Handle();

        Assert.Equal(DuckstationConnectionStatus.ConnectionLost, status);
        DuckstationConnectorMock.Verify(connector => connector.Disconnect(), Times.Once);
        EventDispatcherServiceMock.Verify(dispatcher => dispatcher.DispatchEmulatorConnectionStatus(false), Times.Once);
    }

    [Fact]
    public void GetConnectionStatus_ShouldReturnWaitingForEmulator_WhenTryConnectFails()
    {
        DuckstationConnectorMock.Setup(connector => connector.IsConnected).Returns(false);
        DuckstationConnectorMock.Setup(connector => connector.TryConnect()).Returns(false);

        var coordinator = CreateCoordinator();

        var status = coordinator.Handle();

        Assert.Equal(DuckstationConnectionStatus.WaitingForEmulator, status);
        DuckstationConnectorMock.Verify(connector => connector.Disconnect(), Times.Never);
        EventDispatcherServiceMock.Verify(dispatcher => dispatcher.DispatchEmulatorConnectionStatus(false), Times.Once);
    }

    [Fact]
    public void GetConnectionStatus_ShouldReturnReady_WhenTryConnectSucceeds()
    {
        DuckstationConnectorMock.Setup(connector => connector.IsConnected).Returns(false);
        DuckstationConnectorMock.SetupSequence(connector => connector.TryConnect())
            .Returns(true)
            .Returns(true);
        DuckstationConnectorMock.Setup(connector => connector.IsConnectionAlive()).Returns(true);

        var coordinator = CreateCoordinator();

        var status = coordinator.Handle();

        Assert.Equal(DuckstationConnectionStatus.Ready, status);
        EventDispatcherServiceMock.Verify(dispatcher => dispatcher.DispatchEmulatorConnectionStatus(true), Times.Once);
    }

    [Fact]
    public void HandleProcessingFailure_ShouldDisconnectAndDispatchFalse()
    {
        var coordinator = CreateCoordinator();

        coordinator.HandleProcessingFailure(new Exception("RAM read error"));

        DuckstationConnectorMock.Verify(connector => connector.Disconnect(), Times.Once);
        EventDispatcherServiceMock.Verify(dispatcher => dispatcher.DispatchEmulatorConnectionStatus(false), Times.Once);
    }

    [Fact]
    public void HandleMemoryReadFailure_ShouldDisconnectDispatchFalse()
    {
        var coordinator = CreateCoordinator();

        coordinator.HandleMemoryReadFailure();

        DuckstationConnectorMock.Verify(connector => connector.Disconnect(), Times.Once);
        EventDispatcherServiceMock.Verify(dispatcher => dispatcher.DispatchEmulatorConnectionStatus(false), Times.Once);
    }
}
