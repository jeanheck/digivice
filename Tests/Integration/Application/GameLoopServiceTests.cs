namespace Tests.Integration.Application;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using Backend.Application;
using Backend.Application.Providers;
using Backend.Diagnostics;
using Backend.Domain.Models;
using Backend.Domain.Models.Journals;
using Backend.Events.Services;
using Backend.Events.States;
using Backend.Memory.Readers;

public class GameLoopServiceTests
{
    private readonly Mock<IMemoryReader> _memoryReaderMock;
    private readonly Mock<IPlayerProvider> _playerProviderMock;
    private readonly Mock<IPartyProvider> _partyProviderMock;
    private readonly Mock<IJournalProvider> _journalProviderMock;
    private readonly Mock<IEventDispatcherService> _eventDispatcherServiceMock;
    private readonly GameStateStore _gameStateStore;
    private readonly DebugConsoleRenderer _debugConsoleRenderer;
    private readonly StateComposer _stateComposer;
    private readonly IConfiguration _configuration;

    public GameLoopServiceTests()
    {
        _memoryReaderMock = new Mock<IMemoryReader>();
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
        // Setup memory reader connection sequence
        _memoryReaderMock.SetupSequence(r => r.IsConnected)
            .Returns(false) // First loop check: not connected, will try to connect
            .Returns(false) // Second loop check: still not connected
            .Returns(true)  // Third loop check: connected
            .Returns(true); // Remaining checks

        _memoryReaderMock.SetupSequence(r => r.TryConnect())
            .Returns(false) // First connection attempt fails
            .Returns(true);  // Second connection attempt succeeds

        var service = new GameLoopService(
            _memoryReaderMock.Object,
            _stateComposer,
            _eventDispatcherServiceMock.Object,
            _gameStateStore,
            _debugConsoleRenderer,
            _configuration);

        using var cts = new CancellationTokenSource();
        var serviceTask = service.StartAsync(cts.Token);

        // Allow some loops to run (1ms interval)
        await Task.Delay(100);
        await cts.CancelAsync();
        
        try
        {
            await serviceTask;
        }
        catch (OperationCanceledException)
        {
            // Expected
        }

        // Verify dispatch connection status events
        _eventDispatcherServiceMock.Verify(d => d.DispatchEmulatorConnectionStatus(false), Times.AtLeastOnce);
        _eventDispatcherServiceMock.Verify(d => d.DispatchEmulatorConnectionStatus(true), Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReadStateAndDispatchEvents_WhenConnected()
    {
        _memoryReaderMock.Setup(r => r.IsConnected).Returns(true);

        var service = new GameLoopService(
            _memoryReaderMock.Object,
            _stateComposer,
            _eventDispatcherServiceMock.Object,
            _gameStateStore,
            _debugConsoleRenderer,
            _configuration);

        using var cts = new CancellationTokenSource();
        var serviceTask = service.StartAsync(cts.Token);

        // Allow loops to run
        await Task.Delay(100);
        await cts.CancelAsync();
        
        try
        {
            await serviceTask;
        }
        catch (OperationCanceledException)
        {
            // Expected
        }

        // Verify that state composer was called, state updated, and events were dispatched
        _playerProviderMock.Verify(p => p.Get(), Times.AtLeastOnce);
        Assert.NotNull(_gameStateStore.CurrentState);
        _eventDispatcherServiceMock.Verify(d => d.DispatchEvents(It.IsAny<IEnumerable<Backend.Events.Models.Event>>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldHandleAbruptExceptionsAndDisconnect()
    {
        // Setup initial connection, but throw exception on player provider (fails StateComposer.Compose)
        _memoryReaderMock.SetupSequence(r => r.IsConnected)
            .Returns(true)
            .Returns(false); // Fails the connection loss check in catch block

        _playerProviderMock.Setup(p => p.Get()).Throws(new Exception("RAM read error"));

        var service = new GameLoopService(
            _memoryReaderMock.Object,
            _stateComposer,
            _eventDispatcherServiceMock.Object,
            _gameStateStore,
            _debugConsoleRenderer,
            _configuration);

        using var cts = new CancellationTokenSource();
        var serviceTask = service.StartAsync(cts.Token);

        // Allow loop to run and execute catch block
        await Task.Delay(50);
        await cts.CancelAsync();
        
        try
        {
            await serviceTask;
        }
        catch (OperationCanceledException)
        {
            // Expected
        }

        // Verify we dispatched a connection failure event on catch block when not connected
        _eventDispatcherServiceMock.Verify(d => d.DispatchEmulatorConnectionStatus(false), Times.AtLeastOnce);
    }
}
