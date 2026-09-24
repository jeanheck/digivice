namespace Tests.Integration.Application;

using Backend.Application;
using Backend.Application.Providers.Interfaces;
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
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

public class GameLoopServiceTests
{
    private static readonly TimeSpan WaitTimeout = TimeSpan.FromSeconds(5);

    private readonly Mock<IDuckstationConnector> duckstationConnectorMock;
    private readonly Mock<IPlayerProvider> playerProviderMock;
    private readonly Mock<IEventDispatcherService> eventDispatcherServiceMock;
    private readonly GameStateStore gameStateStore;
    private readonly StateComposer stateComposer;
    private readonly IConfiguration configuration;

    public GameLoopServiceTests()
    {
        duckstationConnectorMock = new Mock<IDuckstationConnector>();
        playerProviderMock = new Mock<IPlayerProvider>();
        eventDispatcherServiceMock = new Mock<IEventDispatcherService>();
        gameStateStore = new GameStateStore();

        var importantItemsProviderMock = new Mock<IImportantItemsProvider>();
        var partyProviderMock = new Mock<IPartyProvider>();
        var digimonBattleProviderMock = new Mock<IDigimonBattleProvider>();
        var cardBattleProviderMock = new Mock<ICardBattleProvider>();
        var auctionsProviderMock = new Mock<IAuctionsProvider>();
        var npcsProviderMock = new Mock<INpcsProvider>();
        var journalProviderMock = new Mock<IJournalProvider>();

        playerProviderMock.Setup(provider => provider.Get()).Returns(new Player { Bits = 123, MapId = "0001" });
        importantItemsProviderMock.Setup(provider => provider.Get()).Returns(new ImportantItems());
        partyProviderMock.Setup(provider => provider.Get()).Returns(new Party { Slots = [] });
        digimonBattleProviderMock.Setup(provider => provider.Get()).Returns(new DigimonBattle());
        cardBattleProviderMock.Setup(provider => provider.Get()).Returns(new CardBattle());
        auctionsProviderMock.Setup(provider => provider.Get()).Returns(new Auctions());
        npcsProviderMock.Setup(provider => provider.Get()).Returns(new Npcs());
        journalProviderMock.Setup(provider => provider.Get())
            .Returns(new Journal { MainQuest = new Quest { Id = "MainQuest" }, SideQuests = [] });

        stateComposer = new StateComposer(
            playerProviderMock.Object,
            importantItemsProviderMock.Object,
            partyProviderMock.Object,
            digimonBattleProviderMock.Object,
            cardBattleProviderMock.Object,
            auctionsProviderMock.Object,
            npcsProviderMock.Object,
            journalProviderMock.Object);

        var inMemorySettings = new Dictionary<string, string?>
        {
            { "GameLoop:PollingIntervalMs", "1" },
            { "Features:Debugging", "false" }
        };
        configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCycleConnectionOfflineThenOnline()
    {
        duckstationConnectorMock.SetupSequence(connector => connector.EnsureConnection())
            .Returns(ConnectionAttemptResult.Failure(EmulatorConnectionErrorCodes.ProcessNotFound))
            .Returns(ConnectionAttemptResult.Failure(EmulatorConnectionErrorCodes.ProcessNotFound))
            .Returns(ConnectionAttemptResult.Success())
            .Returns(ConnectionAttemptResult.Success());

        await RunServiceUntilAsync(() => WasDispatched(events => ContainsHealthEvent(events, HealthStatus.Healthy)));

        Assert.True(WasDispatched(events =>
            ContainsHealthEvent(events, HealthStatus.Error, EmulatorConnectionErrorCodes.ProcessNotFound, null)));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReadStateAndDispatchEvents_WhenConnected()
    {
        duckstationConnectorMock.Setup(connector => connector.EnsureConnection())
            .Returns(ConnectionAttemptResult.Success());

        await RunServiceUntilAsync(() =>
            gameStateStore.CurrentState != null
            && WasDispatched(events => events.Any(dispatchedEvent => dispatchedEvent.Type.Equals(EventType.InitialState))));

        playerProviderMock.Verify(provider => provider.Get(), Times.AtLeastOnce);
        eventDispatcherServiceMock.Verify(
            dispatcher => dispatcher.DispatchEvents(It.Is<IEnumerable<Event>>(events =>
                events.Any(dispatchedEvent => dispatchedEvent.Type.Equals(EventType.InitialState)))),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldClearSessionAndPreviousState_WhenComposeThrows()
    {
        gameStateStore.UpdateState(new State { Player = new Player { Bits = 123, MapId = "0001" } });
        duckstationConnectorMock.Setup(connector => connector.EnsureConnection())
            .Returns(ConnectionAttemptResult.Success());
        playerProviderMock.Setup(provider => provider.Get())
            .Throws(new InvalidOperationException("Address not found for Digimon ID 208"));

        var sessionClearedWhileRunning = false;
        await RunServiceUntilAsync(() =>
        {
            sessionClearedWhileRunning = WasSessionCleared();
            return sessionClearedWhileRunning
                && WasDispatched(events => ContainsHealthEvent(
                    events,
                    HealthStatus.Error,
                    EmulatorConnectionErrorCodes.StateComposeFailed,
                    "Address not found for Digimon ID 208"));
        });

        Assert.True(sessionClearedWhileRunning);
        Assert.Null(gameStateStore.CurrentState);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldClearSessionAndDispatchMemoryReadFailed_WhenComposeThrowsMemoryReadException()
    {
        duckstationConnectorMock.Setup(connector => connector.EnsureConnection())
            .Returns(ConnectionAttemptResult.Success());
        playerProviderMock.Setup(provider => provider.Get())
            .Throws(new MemoryReadException(0x1000, "Failed to read player data"));

        var sessionClearedWhileRunning = false;
        await RunServiceUntilAsync(() =>
        {
            sessionClearedWhileRunning = WasSessionCleared();
            return sessionClearedWhileRunning
                && WasDispatched(events => ContainsHealthEvent(
                    events,
                    HealthStatus.Error,
                    EmulatorConnectionErrorCodes.MemoryReadFailed,
                    "Failed to read player data"));
        });

        Assert.True(sessionClearedWhileRunning);
        Assert.Null(gameStateStore.CurrentState);
    }

    private GameLoopService CreateGameLoopService()
    {
        return new GameLoopService(
            duckstationConnectorMock.Object,
            stateComposer,
            eventDispatcherServiceMock.Object,
            gameStateStore,
            new DebugConsoleRenderer(),
            configuration,
            NullLogger<GameLoopService>.Instance);
    }

    private async Task RunServiceUntilAsync(Func<bool> condition)
    {
        var service = CreateGameLoopService();
        using var cancellationTokenSource = new CancellationTokenSource();
        var serviceTask = service.StartAsync(cancellationTokenSource.Token);

        var conditionMet = await WaitUntilAsync(condition);
        await cancellationTokenSource.CancelAsync();

        try
        {
            await serviceTask;
        }
        catch (OperationCanceledException)
        {
        }

        Assert.True(conditionMet, "Condition was not met before timeout");
    }

    private static async Task<bool> WaitUntilAsync(Func<bool> condition)
    {
        var deadline = DateTime.UtcNow + WaitTimeout;
        while (DateTime.UtcNow < deadline)
        {
            if (condition())
            {
                return true;
            }

            await Task.Delay(5);
        }

        return condition();
    }

    private bool WasSessionCleared()
    {
        return duckstationConnectorMock.Invocations
            .Any(invocation => invocation.Method.Name == nameof(IDuckstationConnector.ClearSession));
    }

    private bool WasDispatched(Func<IEnumerable<Event>, bool> predicate)
    {
        return eventDispatcherServiceMock.Invocations
            .Where(invocation => invocation.Method.Name == nameof(IEventDispatcherService.DispatchEvents))
            .Any(invocation => predicate((IEnumerable<Event>)invocation.Arguments[0]));
    }

    private static bool ContainsHealthEvent(
        IEnumerable<Event> events,
        HealthStatus status,
        string? errorCode = null,
        string? errorDetail = null)
    {
        return events.Any(dispatchedEvent =>
        {
            if (!dispatchedEvent.Type.Equals(EventType.HealthChanged))
            {
                return false;
            }

            var healthDTO = (HealthDTO)dispatchedEvent.Payload;
            return healthDTO.Status == status
                && healthDTO.ErrorCode == errorCode
                && healthDTO.ErrorDetail == errorDetail;
        });
    }
}
