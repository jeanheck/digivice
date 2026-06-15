namespace Tests.Events.Services;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Backend.Events.Services;
using Backend.Events.Hubs;
using Backend.Events.States;
using Backend.Events.Models;
using Backend.Events.DTO;
using Backend.Domain.Models;

public class EventDispatcherServiceTests
{
    [Fact]
    public void DispatchInitialStateToClient_ShouldIncludeLastError_WhenEmulatorIsOffline()
    {
        var clientProxyMock = new Mock<ISingleClientProxy>();
        clientProxyMock.Setup(c => c.SendCoreAsync(
            It.IsAny<string>(),
            It.IsAny<object?[]>(),
            It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var hubClientsMock = new Mock<IHubClients>();
        hubClientsMock.Setup(c => c.Client("client1")).Returns(clientProxyMock.Object);

        var hubContextMock = new Mock<IHubContext<GameHub>>();
        hubContextMock.Setup(h => h.Clients).Returns(hubClientsMock.Object);

        var loggerMock = new Mock<ILogger<EventDispatcherService>>();
        var gameStateStore = new GameStateStore
        {
            IsConnectedWithEmulator = false,
            LastEmulatorConnectionErrorCode = "mapping_not_found"
        };

        var service = new EventDispatcherService(
            hubContextMock.Object,
            loggerMock.Object,
            gameStateStore);

        service.DispatchInitialStateToClient("client1");

        clientProxyMock.Verify(
            c => c.SendCoreAsync(
                "EmulatorConnectionStatusChanged",
                It.Is<object?[]>(args =>
                    args.Length == 1
                    && GetConnectionDto(args[0]!).IsConnected == false
                    && GetConnectionDto(args[0]!).ErrorCode == "mapping_not_found"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static ConnectionDTO GetConnectionDto(object eventObject)
    {
        var ev = (Event)eventObject;
        return (ConnectionDTO)ev.Payload;
    }

    [Fact]
    public void DispatchInitialStateToClient_ShouldSendEmulatorStatusOnly_WhenCurrentStateIsNull()
    {
        var clientProxyMock = new Mock<ISingleClientProxy>();
        clientProxyMock.Setup(c => c.SendCoreAsync(
            It.IsAny<string>(),
            It.IsAny<object?[]>(),
            It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var hubClientsMock = new Mock<IHubClients>();
        hubClientsMock.Setup(c => c.Client("client1")).Returns(clientProxyMock.Object);

        var hubContextMock = new Mock<IHubContext<GameHub>>();
        hubContextMock.Setup(h => h.Clients).Returns(hubClientsMock.Object);

        var loggerMock = new Mock<ILogger<EventDispatcherService>>();

        var gameStateStoreMock = new Mock<IGameStateStore>();
        gameStateStoreMock.Setup(g => g.CurrentState).Returns((State?)null);
        gameStateStoreMock.Setup(g => g.IsConnectedWithEmulator).Returns(false);

        var service = new EventDispatcherService(
            hubContextMock.Object,
            loggerMock.Object,
            gameStateStoreMock.Object
        );

        service.DispatchInitialStateToClient("client1");

        clientProxyMock.Verify(
            c => c.SendCoreAsync(
                "InitialState",
                It.IsAny<object?[]>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Never
        );
        clientProxyMock.Verify(
            c => c.SendCoreAsync(
                "EmulatorConnectionStatusChanged",
                It.Is<object?[]>(args => args.Length == 1 && ((Event)args[0]!).Type.Equals(EventType.EmulatorConnectionStatusChanged)),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }

    [Fact]
    public void DispatchInitialStateToClient_ShouldSendStateDTO_WhenCurrentStateIsNotNull()
    {
        // Arrange
        var clientProxyMock = new Mock<ISingleClientProxy>();
        clientProxyMock.Setup(c => c.SendCoreAsync(
            It.IsAny<string>(),
            It.IsAny<object?[]>(),
            It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var hubClientsMock = new Mock<IHubClients>();
        hubClientsMock.Setup(c => c.Client("client1")).Returns(clientProxyMock.Object);
        hubClientsMock.Setup(c => c.All).Returns(clientProxyMock.Object);

        var hubContextMock = new Mock<IHubContext<GameHub>>();
        hubContextMock.Setup(h => h.Clients).Returns(hubClientsMock.Object);

        var loggerMock = new Mock<ILogger<EventDispatcherService>>();

        var state = new State(); // Novo estado limpo padrão
        var gameStateStoreMock = new Mock<IGameStateStore>();
        gameStateStoreMock.Setup(g => g.CurrentState).Returns(state);

        var service = new EventDispatcherService(
            hubContextMock.Object,
            loggerMock.Object,
            gameStateStoreMock.Object
        );

        // Act
        service.DispatchInitialStateToClient("client1");

        // Assert
        hubClientsMock.Verify(c => c.Client("client1"), Times.Once);
        clientProxyMock.Verify(
            c => c.SendCoreAsync(
                "InitialState",
                It.Is<object?[]>(args => args.Length == 1 && ((Event)args[0]!).Type.Equals(EventType.InitialState)),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }

    [Fact]
    public void DispatchEvents_ShouldSendAllEvents_WhenEnumerableIsNotEmpty()
    {
        // Arrange
        var clientProxyMock = new Mock<ISingleClientProxy>();
        clientProxyMock.Setup(c => c.SendCoreAsync(
            It.IsAny<string>(),
            It.IsAny<object?[]>(),
            It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var hubClientsMock = new Mock<IHubClients>();
        hubClientsMock.Setup(c => c.All).Returns(clientProxyMock.Object);

        var hubContextMock = new Mock<IHubContext<GameHub>>();
        hubContextMock.Setup(h => h.Clients).Returns(hubClientsMock.Object);

        var loggerMock = new Mock<ILogger<EventDispatcherService>>();
        var gameStateStoreMock = new Mock<IGameStateStore>();

        var service = new EventDispatcherService(
            hubContextMock.Object,
            loggerMock.Object,
            gameStateStoreMock.Object
        );

        var events = new List<Event>
        {
            new(EventType.PlayerChanged, new ConnectionDTO(true)),
            new(EventType.PartyChanged, new ConnectionDTO(true))
        };

        // Act
        service.DispatchEvents(events);

        // Assert
        clientProxyMock.Verify(
            c => c.SendCoreAsync(
                "PlayerChanged",
                It.IsAny<object?[]>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        clientProxyMock.Verify(
            c => c.SendCoreAsync(
                "PartyChanged",
                It.IsAny<object?[]>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }

    [Fact]
    public async Task SafeDispatch_ShouldLogException_WhenSendAsyncThrows()
    {
        // Arrange
        var clientProxyMock = new Mock<ISingleClientProxy>();
        var networkException = new Exception("SignalR connection closed catastrophically");

        // Retorna uma task falhada para simular exceção assíncrona
        clientProxyMock.Setup(c => c.SendCoreAsync(
            It.IsAny<string>(),
            It.IsAny<object?[]>(),
            It.IsAny<CancellationToken>()))
            .Returns(Task.FromException(networkException));

        var hubClientsMock = new Mock<IHubClients>();
        hubClientsMock.Setup(c => c.All).Returns(clientProxyMock.Object);

        var hubContextMock = new Mock<IHubContext<GameHub>>();
        hubContextMock.Setup(h => h.Clients).Returns(hubClientsMock.Object);

        var loggerMock = new Mock<ILogger<EventDispatcherService>>();
        var gameStateStoreMock = new Mock<IGameStateStore>();

        var service = new EventDispatcherService(
            hubContextMock.Object,
            loggerMock.Object,
            gameStateStoreMock.Object
        );

        var singleEvent = new Event(EventType.PlayerChanged, new ConnectionDTO(true));

        // Act
        service.DispatchEvents(new[] { singleEvent });

        // Damos um tempo curto de thread para permitir que o ContinueWith assíncrono execute
        await Task.Delay(100);

        // Assert
        loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.Is<Exception>(ex => ex == networkException || ex.InnerException == networkException),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)
            ),
            Times.Once
        );
    }

    [Fact]
    public void DispatchEvents_ShouldDoNothing_WhenEnumerableIsEmpty()
    {
        // Arrange
        var clientProxyMock = new Mock<ISingleClientProxy>();
        var hubClientsMock = new Mock<IHubClients>();
        hubClientsMock.Setup(c => c.All).Returns(clientProxyMock.Object);

        var hubContextMock = new Mock<IHubContext<GameHub>>();
        hubContextMock.Setup(h => h.Clients).Returns(hubClientsMock.Object);

        var loggerMock = new Mock<ILogger<EventDispatcherService>>();
        var gameStateStoreMock = new Mock<IGameStateStore>();

        var service = new EventDispatcherService(
            hubContextMock.Object,
            loggerMock.Object,
            gameStateStoreMock.Object
        );

        // Act
        service.DispatchEvents([]); // Envia uma coleção de eventos vazia

        // Assert
        clientProxyMock.Verify(
            c => c.SendCoreAsync(It.IsAny<string>(), It.IsAny<object?[]>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    [Fact]
    public void DispatchInitialStateToClient_ShouldSendStateDTOWithNulls_WhenCurrentStateHasNullEntities()
    {
        // Arrange
        var clientProxyMock = new Mock<ISingleClientProxy>();
        clientProxyMock.Setup(c => c.SendCoreAsync(
            It.IsAny<string>(),
            It.IsAny<object?[]>(),
            It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var hubClientsMock = new Mock<IHubClients>();
        hubClientsMock.Setup(c => c.Client("client1")).Returns(clientProxyMock.Object);
        hubClientsMock.Setup(c => c.All).Returns(clientProxyMock.Object);

        var hubContextMock = new Mock<IHubContext<GameHub>>();
        hubContextMock.Setup(h => h.Clients).Returns(hubClientsMock.Object);

        var loggerMock = new Mock<ILogger<EventDispatcherService>>();

        var state = new State { Player = null!, Party = null!, Journal = null! }; // Entidades internas nulas
        var gameStateStoreMock = new Mock<IGameStateStore>();
        gameStateStoreMock.Setup(g => g.CurrentState).Returns(state);

        var service = new EventDispatcherService(
            hubContextMock.Object,
            loggerMock.Object,
            gameStateStoreMock.Object
        );

        // Act
        service.DispatchInitialStateToClient("client1");

        // Assert
        hubClientsMock.Verify(c => c.Client("client1"), Times.Once);
        clientProxyMock.Verify(
            c => c.SendCoreAsync(
                "InitialState",
                It.Is<object?[]>(args => args.Length == 1 && ((Event)args[0]!).Type.Equals(EventType.InitialState)),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }
}
