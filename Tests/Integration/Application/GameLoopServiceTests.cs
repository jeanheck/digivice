namespace Tests.Integration.Application;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Backend.Application;
using Backend.Application.Providers;
using Backend.Diagnostics;
using Backend.Domain.Models;
using Backend.Domain.Models.Journals;
using Backend.Events.DTO;
using Backend.Events.Models;
using Backend.Events.Services;
using Backend.Events.States;
using Backend.Infrastructure.Duckstation;
using Backend.Memory;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

public class GameLoopServiceTests
{
    private readonly Mock<IDuckstationConnector> _duckstationConnectorMock;
    private readonly Mock<IPlayerProvider> _playerProviderMock;
    private readonly Mock<IPartyProvider> _partyProviderMock;
    private readonly Mock<IJournalProvider> _journalProviderMock;
    private readonly Mock<IEventDispatcherService> _eventDispatcherServiceMock;
    private readonly GameStateStore _gameStateStore;
    private readonly DebugConsoleRenderer _debugConsoleRenderer;
    private readonly StateComposer _stateComposer;
    private readonly IConfiguration _configuration;

    private GameLoopService CreateGameLoopService(StateComposer? stateComposer = null)
    {
        return new GameLoopService(
            _duckstationConnectorMock.Object,
            stateComposer ?? _stateComposer,
            _eventDispatcherServiceMock.Object,
            _gameStateStore,
            _debugConsoleRenderer,
            _configuration);
    }

    public GameLoopServiceTests()
    {
        _duckstationConnectorMock = new Mock<IDuckstationConnector>();
        _playerProviderMock = new Mock<IPlayerProvider>();
        _partyProviderMock = new Mock<IPartyProvider>();
        _journalProviderMock = new Mock<IJournalProvider>();
        _eventDispatcherServiceMock = new Mock<IEventDispatcherService>();
        _gameStateStore = new GameStateStore();
        _debugConsoleRenderer = new DebugConsoleRenderer();

        var player = new Player { Name = "Agumon", Bits = 123, MapId = "0001" };
        var party = new Party { Slots = [] };
        var journal = new Journal { MainQuest = new Quest { Id = "MainQuest" }, SideQuests = [] };
        _playerProviderMock.Setup(p => p.Get()).Returns(player);
        _partyProviderMock.Setup(p => p.Get()).Returns(party);
        _journalProviderMock.Setup(p => p.Get()).Returns(journal);

        _stateComposer = new StateComposer(
            _playerProviderMock.Object,
            _partyProviderMock.Object,
            _journalProviderMock.Object);

        var inMemorySettings = new Dictionary<string, string?> {
            {"GameLoop:PollingIntervalMs", "1"},
            {"Features:Debugging", "false"}
        };
        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCycleConnectionOfflineThenOnline()
    {
        _duckstationConnectorMock.SetupSequence(connector => connector.EnsureConnection())
            .Returns(ConnectionAttemptResult.Failure(EmulatorConnectionErrorCodes.ProcessNotFound))
            .Returns(ConnectionAttemptResult.Failure(EmulatorConnectionErrorCodes.ProcessNotFound))
            .Returns(ConnectionAttemptResult.Success())
            .Returns(ConnectionAttemptResult.Success());

        var service = CreateGameLoopService();

        using var cts = new CancellationTokenSource();
        var serviceTask = service.StartAsync(cts.Token);

        await Task.Delay(100);
        await cts.CancelAsync();

        try
        {
            await serviceTask;
        }
        catch (OperationCanceledException)
        {
        }

        _eventDispatcherServiceMock.Verify(
            d => d.DispatchEvents(It.Is<IEnumerable<Event>>(events =>
                ContainsConnectionEvent(events, isConnected: false, EmulatorConnectionErrorCodes.ProcessNotFound, null))),
            Times.AtLeastOnce);
        _eventDispatcherServiceMock.Verify(
            d => d.DispatchEvents(It.Is<IEnumerable<Event>>(events =>
                ContainsConnectionEvent(events, isConnected: true))),
            Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldDisconnect_WhenConnectionIsNoLongerAlive()
    {
        _duckstationConnectorMock.Setup(connector => connector.EnsureConnection())
            .Returns(ConnectionAttemptResult.Failure(EmulatorConnectionErrorCodes.ProcessNotFound));

        var service = CreateGameLoopService();

        using var cts = new CancellationTokenSource();
        var serviceTask = service.StartAsync(cts.Token);

        await Task.Delay(50);
        await cts.CancelAsync();

        try
        {
            await serviceTask;
        }
        catch (OperationCanceledException)
        {
        }

        _eventDispatcherServiceMock.Verify(
            d => d.DispatchEvents(It.Is<IEnumerable<Event>>(events =>
                ContainsConnectionEvent(events, isConnected: false, EmulatorConnectionErrorCodes.ProcessNotFound, null))),
            Times.AtLeastOnce);
    }

    private static bool ContainsConnectionEvent(
        IEnumerable<Event> events,
        bool isConnected,
        string? errorCode = null,
        string? errorDetail = null)
    {
        return events.Any(ev =>
        {
            if (!ev.Type.Equals(EventType.EmulatorConnectionStatusChanged))
            {
                return false;
            }

            var dto = (ConnectionDTO)ev.Payload;
            return dto.IsConnected == isConnected
                && dto.ErrorCode == errorCode
                && dto.ErrorDetail == errorDetail;
        });
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReadStateAndDispatchEvents_WhenConnected()
    {
        _duckstationConnectorMock.Setup(connector => connector.EnsureConnection())
            .Returns(ConnectionAttemptResult.Success());

        var service = CreateGameLoopService();

        using var cts = new CancellationTokenSource();
        var serviceTask = service.StartAsync(cts.Token);

        await Task.Delay(100);
        await cts.CancelAsync();

        try
        {
            await serviceTask;
        }
        catch (OperationCanceledException)
        {
        }

        _playerProviderMock.Verify(p => p.Get(), Times.AtLeastOnce);
        Assert.NotNull(_gameStateStore.CurrentState);
        _eventDispatcherServiceMock.Verify(d => d.DispatchEvents(It.IsAny<IEnumerable<Event>>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldHandleAbruptExceptionsAndDisconnect()
    {
        _duckstationConnectorMock.Setup(connector => connector.EnsureConnection())
            .Returns(ConnectionAttemptResult.Success());
        _playerProviderMock.Setup(p => p.Get()).Throws(new Exception("RAM read error"));

        var service = CreateGameLoopService();

        using var cts = new CancellationTokenSource();
        var serviceTask = service.StartAsync(cts.Token);

        await Task.Delay(50);
        await cts.CancelAsync();

        try
        {
            await serviceTask;
        }
        catch (OperationCanceledException)
        {
        }

        _duckstationConnectorMock.Verify(connector => connector.ClearSession(), Times.AtLeastOnce);
        _eventDispatcherServiceMock.Verify(
            d => d.DispatchEvents(It.Is<IEnumerable<Event>>(events =>
                ContainsConnectionEvent(
                    events,
                    isConnected: false,
                    EmulatorConnectionErrorCodes.StateComposeFailed,
                    "RAM read error"))),
            Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldDisconnectAndDispatchFalse_WhenComposeThrowsWhileReaderReportsConnected()
    {
        _duckstationConnectorMock.Setup(connector => connector.EnsureConnection())
            .Returns(ConnectionAttemptResult.Success());
        _playerProviderMock.Setup(p => p.Get()).Throws(new InvalidOperationException("Address not found for Digimon ID 208"));

        var service = CreateGameLoopService();

        using var cts = new CancellationTokenSource();
        var serviceTask = service.StartAsync(cts.Token);

        await Task.Delay(50);
        await cts.CancelAsync();

        try
        {
            await serviceTask;
        }
        catch (OperationCanceledException)
        {
        }

        _duckstationConnectorMock.Verify(connector => connector.ClearSession(), Times.AtLeastOnce);
        _eventDispatcherServiceMock.Verify(
            d => d.DispatchEvents(It.Is<IEnumerable<Event>>(events =>
                events.Any(ev =>
                    ev.Type.Equals(EventType.EmulatorConnectionStatusChanged)
                    && !((ConnectionDTO)ev.Payload).IsConnected
                    && ((ConnectionDTO)ev.Payload).ErrorCode == EmulatorConnectionErrorCodes.StateComposeFailed))),
            Times.AtLeastOnce);
        Assert.Null(_gameStateStore.CurrentState);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldDispatchMemoryReadFailed_WhenComposeThrowsMemoryReadException()
    {
        _duckstationConnectorMock.Setup(connector => connector.EnsureConnection())
            .Returns(ConnectionAttemptResult.Success());
        _playerProviderMock.Setup(p => p.Get()).Throws(new MemoryReadException(0x1000, "Failed to read player data"));

        var service = CreateGameLoopService();

        using var cts = new CancellationTokenSource();
        var serviceTask = service.StartAsync(cts.Token);

        await Task.Delay(50);
        await cts.CancelAsync();

        try
        {
            await serviceTask;
        }
        catch (OperationCanceledException)
        {
        }

        _eventDispatcherServiceMock.Verify(
            d => d.DispatchEvents(It.Is<IEnumerable<Event>>(events =>
                ContainsConnectionEvent(
                    events,
                    isConnected: false,
                    EmulatorConnectionErrorCodes.MemoryReadFailed,
                    "Failed to read player data"))),
            Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldDispatchDisconnect_WhenComposeThrows()
    {
        _gameStateStore.UpdateState(new State
        {
            Player = new Player { Name = "Agumon", Bits = 123, MapId = "0001" }
        });
        _duckstationConnectorMock.Setup(connector => connector.EnsureConnection())
            .Returns(ConnectionAttemptResult.Success());

        var throwingPlayerProviderMock = new Mock<IPlayerProvider>();
        throwingPlayerProviderMock.Setup(p => p.Get()).Throws(new Exception("RAM read error"));
        var stateComposer = new StateComposer(
            throwingPlayerProviderMock.Object,
            _partyProviderMock.Object,
            _journalProviderMock.Object);

        var service = CreateGameLoopService(stateComposer);

        using var cts = new CancellationTokenSource();
        var serviceTask = service.StartAsync(cts.Token);

        await Task.Delay(50);
        await cts.CancelAsync();

        try
        {
            await serviceTask;
        }
        catch (OperationCanceledException)
        {
        }

        _eventDispatcherServiceMock.Verify(
            d => d.DispatchEvents(It.Is<IEnumerable<Event>>(events =>
                events.Any(ev =>
                    ev.Type.Equals(EventType.EmulatorConnectionStatusChanged)
                    && !((ConnectionDTO)ev.Payload).IsConnected
                    && ((ConnectionDTO)ev.Payload).ErrorCode == EmulatorConnectionErrorCodes.StateComposeFailed))),
            Times.AtLeastOnce);
    }
}
