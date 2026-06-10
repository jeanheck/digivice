namespace Tests.Unit.Application;

using Backend.Application;
using Backend.Diagnostics;
using Backend.Events.Services;
using Backend.Memory.Readers;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

public class DuckstationConnectorTests
{
    private readonly Mock<IMemoryReader> MemoryReaderMock;
    private readonly Mock<IEventDispatcherService> EventDispatcherServiceMock;
    private readonly DebugConsoleRenderer DebugConsoleRenderer;
    private readonly IConfiguration Configuration;

    public DuckstationConnectorTests()
    {
        MemoryReaderMock = new Mock<IMemoryReader>();
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

    private DuckstationConnector CreateConnector()
    {
        return new DuckstationConnector(
            MemoryReaderMock.Object,
            EventDispatcherServiceMock.Object,
            DebugConsoleRenderer,
            Configuration);
    }

    [Fact]
    public void EnsureSession_ShouldReturnReady_WhenConnectedAndAlive()
    {
        MemoryReaderMock.Setup(reader => reader.IsConnected).Returns(true);
        MemoryReaderMock.Setup(reader => reader.IsConnectionAlive()).Returns(true);

        var connector = CreateConnector();

        var status = connector.getConnectionStatus();

        Assert.Equal(DuckstationConnectionStatus.Ready, status);
        EventDispatcherServiceMock.Verify(
            dispatcher => dispatcher.DispatchEmulatorConnectionStatus(It.IsAny<bool>()),
            Times.Never);
    }

    [Fact]
    public void EnsureSession_ShouldReturnSessionLost_WhenConnectedButNotAlive()
    {
        MemoryReaderMock.Setup(reader => reader.IsConnected).Returns(true);
        MemoryReaderMock.Setup(reader => reader.IsConnectionAlive()).Returns(false);

        var connector = CreateConnector();

        var status = connector.getConnectionStatus();

        Assert.Equal(DuckstationConnectionStatus.ConnectionLost, status);
        MemoryReaderMock.Verify(reader => reader.Disconnect(), Times.Once);
        EventDispatcherServiceMock.Verify(dispatcher => dispatcher.DispatchEmulatorConnectionStatus(false), Times.Once);
    }

    [Fact]
    public void EnsureSession_ShouldReturnWaitingForEmulator_WhenTryConnectFails()
    {
        MemoryReaderMock.Setup(reader => reader.IsConnected).Returns(false);
        MemoryReaderMock.Setup(reader => reader.TryConnect()).Returns(false);

        var connector = CreateConnector();

        var status = connector.getConnectionStatus();

        Assert.Equal(DuckstationConnectionStatus.WaitingForEmulator, status);
        MemoryReaderMock.Verify(reader => reader.Disconnect(), Times.Never);
        EventDispatcherServiceMock.Verify(dispatcher => dispatcher.DispatchEmulatorConnectionStatus(false), Times.Once);
    }

    [Fact]
    public void EnsureSession_ShouldReturnReady_WhenTryConnectSucceeds()
    {
        MemoryReaderMock.Setup(reader => reader.IsConnected).Returns(false);
        MemoryReaderMock.SetupSequence(reader => reader.TryConnect())
            .Returns(true)
            .Returns(true);
        MemoryReaderMock.Setup(reader => reader.IsConnectionAlive()).Returns(true);

        var connector = CreateConnector();

        var status = connector.getConnectionStatus();

        Assert.Equal(DuckstationConnectionStatus.Ready, status);
        EventDispatcherServiceMock.Verify(dispatcher => dispatcher.DispatchEmulatorConnectionStatus(true), Times.Once);
    }

    [Fact]
    public void HandleProcessingFailure_ShouldDisconnectAndDispatchFalse()
    {
        var connector = CreateConnector();

        connector.HandleProcessingFailure(new Exception("RAM read error"));

        MemoryReaderMock.Verify(reader => reader.Disconnect(), Times.Once);
        EventDispatcherServiceMock.Verify(dispatcher => dispatcher.DispatchEmulatorConnectionStatus(false), Times.Once);
    }

    [Fact]
    public void HandleSilentReadFailure_ShouldDisconnectAndDispatchFalse_WhenReaderIsDisconnected()
    {
        MemoryReaderMock.Setup(reader => reader.IsConnected).Returns(false);

        var connector = CreateConnector();

        connector.HandleSilentReadFailure();

        MemoryReaderMock.Verify(reader => reader.Disconnect(), Times.Once);
        EventDispatcherServiceMock.Verify(dispatcher => dispatcher.DispatchEmulatorConnectionStatus(false), Times.Once);
    }

    [Fact]
    public void HandleSilentReadFailure_ShouldDoNothing_WhenReaderIsStillConnected()
    {
        MemoryReaderMock.Setup(reader => reader.IsConnected).Returns(true);

        var connector = CreateConnector();

        connector.HandleSilentReadFailure();

        MemoryReaderMock.Verify(reader => reader.Disconnect(), Times.Never);
        EventDispatcherServiceMock.Verify(
            dispatcher => dispatcher.DispatchEmulatorConnectionStatus(It.IsAny<bool>()),
            Times.Never);
    }
}
