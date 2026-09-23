namespace Tests.Unit.Events.States;

using Xunit;
using Backend.Events.States;
using Backend.Domain.Models;
using Backend.Events.Models;

public class GameStateStoreTests
{
    [Fact]
    public void UpdateState_ShouldUpdateCurrentState()
    {
        GameStateStore gameStateStore = new GameStateStore();
        State expectedState = new State();

        gameStateStore.UpdateState(expectedState);

        Assert.NotNull(gameStateStore.CurrentState);
        Assert.Same(expectedState, gameStateStore.CurrentState);
    }

    [Fact]
    public void ClearState_ShouldResetCurrentState_WithoutChangingStatus()
    {
        GameStateStore gameStateStore = new GameStateStore();
        State dummyState = new State();
        gameStateStore.UpdateState(dummyState);
        gameStateStore.Status = HealthStatus.Healthy;

        gameStateStore.ClearState();

        Assert.Null(gameStateStore.CurrentState);
        Assert.Equal(HealthStatus.Healthy, gameStateStore.Status);
    }

    [Fact]
    public void DefaultStatus_ShouldBeLoading()
    {
        GameStateStore gameStateStore = new GameStateStore();

        Assert.Equal(HealthStatus.Loading, gameStateStore.Status);
    }
}
