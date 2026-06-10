namespace Tests.Unit.Events.Hubs;

using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Xunit;
using Backend.Events.Hubs;
using Backend.Events.Services;
using Backend.Events.States;

public class GameHubTests
{
    [Fact]
    public async Task OnConnectedAsync_ShouldRegisterClientAndDispatchInitialState()
    {
        string expectedConnectionId = "test-connection-id-12345";
        Mock<IEventDispatcherService> eventDispatcherMock = new Mock<IEventDispatcherService>();
        Mock<IGameStateStore> gameStateStoreMock = new Mock<IGameStateStore>();
        Mock<HubCallerContext> hubCallerContextMock = new Mock<HubCallerContext>();

        hubCallerContextMock.Setup(context => context.ConnectionId).Returns(expectedConnectionId);

        GameHub gameHub = new GameHub(eventDispatcherMock.Object, gameStateStoreMock.Object)
        {
            Context = hubCallerContextMock.Object
        };

        await gameHub.OnConnectedAsync();

        gameStateStoreMock.Verify(store => store.RegisterClientConnection(), Times.Once);
        eventDispatcherMock.Verify(dispatcher => dispatcher.DispatchInitialStateToClient(expectedConnectionId), Times.Once);
    }

    [Fact]
    public async Task OnDisconnectedAsync_ShouldUnregisterClient()
    {
        Mock<IEventDispatcherService> eventDispatcherMock = new Mock<IEventDispatcherService>();
        Mock<IGameStateStore> gameStateStoreMock = new Mock<IGameStateStore>();

        GameHub gameHub = new GameHub(eventDispatcherMock.Object, gameStateStoreMock.Object);

        await gameHub.OnDisconnectedAsync(null);

        gameStateStoreMock.Verify(store => store.UnregisterClientConnection(), Times.Once);
    }
}
