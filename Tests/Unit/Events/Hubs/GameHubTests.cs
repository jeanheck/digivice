namespace Tests.Unit.Events.Hubs;

using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Xunit;
using Backend.Events.Hubs;
using Backend.Events.Services;

public class GameHubTests
{
    [Fact]
    public async Task OnConnectedAsync_ShouldDispatchInitialState()
    {
        string expectedConnectionId = "test-connection-id-12345";
        Mock<IEventDispatcherService> eventDispatcherMock = new Mock<IEventDispatcherService>();
        Mock<HubCallerContext> hubCallerContextMock = new Mock<HubCallerContext>();

        hubCallerContextMock.Setup(context => context.ConnectionId).Returns(expectedConnectionId);

        GameHub gameHub = new GameHub(eventDispatcherMock.Object)
        {
            Context = hubCallerContextMock.Object
        };

        await gameHub.OnConnectedAsync();

        eventDispatcherMock.Verify(dispatcher => dispatcher.DispatchInitialStateToClient(expectedConnectionId), Times.Once);
    }
}
