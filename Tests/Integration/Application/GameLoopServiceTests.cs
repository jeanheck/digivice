namespace Tests.Integration.Application;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Backend.Application;
using Backend.Application.Providers;
using Backend.Diagnostics;
using Backend.Domain.Models;
using Backend.Domain.Models.Journals;
using Backend.Events.Services;
using Backend.Events.States;
using Backend.Infrastructure.Duckstation;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

public class GameLoopServiceTests
{
    private readonly Mock<IDuckstationConnection> _duckstationConnectionMock;
    private readonly Mock<IPlayerProvider> _playerProviderMock;
    private readonly Mock<IPartyProvider> _partyProviderMock;
    private readonly Mock<IJournalProvider> _journalProviderMock;
    private readonly Mock<IEventDispatcherService> _eventDispatcherServiceMock;
    private readonly GameStateStore _gameStateStore;
    private readonly DebugConsoleRenderer _debugConsoleRenderer;
    private readonly StateComposer _stateComposer;
    private readonly IConfiguration _configuration;

    private DuckstationConnector CreateDuckstationConnector()
    {
        return new DuckstationConnector(
            _duckstationConnectionMock.Object,
            _eventDispatcherServiceMock.Object,
            _debugConsoleRenderer,
            _configuration);
    }

    private GameLoopService CreateGameLoopService(StateComposer? stateComposer = null)
    {
        return new GameLoopService(
            CreateDuckstationConnector(),
            stateComposer ?? _stateComposer,
            _eventDispatcherServiceMock.Object,
            _gameStateStore,
            _debugConsoleRenderer,
            _configuration);
    }

    private void SetupConnectedDuckstationConnection()
    {
        _duckstationConnectionMock.Setup(connection => connection.IsConnectionAlive()).Returns(true);
    }

    public GameLoopServiceTests()
    {
        _duckstationConnectionMock = new Mock<IDuckstationConnection>();
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
        _duckstationConnectionMock.SetupSequence(connector => connector.IsConnected)
            .Returns(false)
            .Returns(false)
            .Returns(true)
            .Returns(true);

        _duckstationConnectionMock.SetupSequence(connector => connector.TryConnect())
            .Returns(false)
            .Returns(true);

        _duckstationConnectionMock.Setup(connector => connector.IsConnectionAlive()).Returns(true);

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

        _eventDispatcherServiceMock.Verify(d => d.DispatchEmulatorConnectionStatus(false), Times.AtLeastOnce);
        _eventDispatcherServiceMock.Verify(d => d.DispatchEmulatorConnectionStatus(true), Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldDisconnect_WhenConnectionIsNoLongerAlive()
    {
        _duckstationConnectionMock.Setup(connector => connector.IsConnected).Returns(true);
        _duckstationConnectionMock.Setup(connector => connector.IsConnectionAlive()).Returns(false);

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

        _duckstationConnectionMock.Verify(connector => connector.Disconnect(), Times.AtLeastOnce);
        _eventDispatcherServiceMock.Verify(d => d.DispatchEmulatorConnectionStatus(false), Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReadStateAndDispatchEvents_WhenConnected()
    {
        _duckstationConnectionMock.Setup(connector => connector.IsConnected).Returns(true);
        SetupConnectedDuckstationConnection();

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
        _eventDispatcherServiceMock.Verify(d => d.DispatchEvents(It.IsAny<IEnumerable<Backend.Events.Models.Event>>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldHandleAbruptExceptionsAndDisconnect()
    {
        _duckstationConnectionMock.Setup(connector => connector.IsConnected).Returns(true);
        SetupConnectedDuckstationConnection();
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

        _duckstationConnectionMock.Verify(connector => connector.Disconnect(), Times.AtLeastOnce);
        _eventDispatcherServiceMock.Verify(d => d.DispatchEmulatorConnectionStatus(false), Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldDisconnectAndDispatchFalse_WhenComposeThrowsWhileReaderReportsConnected()
    {
        _duckstationConnectionMock.Setup(connector => connector.IsConnected).Returns(true);
        SetupConnectedDuckstationConnection();
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

        _duckstationConnectionMock.Verify(connector => connector.Disconnect(), Times.AtLeastOnce);
        _eventDispatcherServiceMock.Verify(d => d.DispatchEmulatorConnectionStatus(false), Times.AtLeastOnce);
        Assert.Null(_gameStateStore.CurrentState);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldDispatchDisconnect_WhenComposeThrows()
    {
        _gameStateStore.UpdateState(new State
        {
            Player = new Player { Name = "Agumon", Bits = 123, MapId = "0001" }
        });
        _duckstationConnectionMock.Setup(connector => connector.IsConnected).Returns(true);
        SetupConnectedDuckstationConnection();

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
            d => d.DispatchEmulatorConnectionStatus(false),
            Times.AtLeastOnce);
    }
}
