namespace Tests.Unit.Events.States;

using Xunit;
using Backend.Events.States;
using Backend.Domain.Models;

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
    public void ClearState_ShouldResetCurrentStateAndSetEmulatorConnectedToFalse()
    {
        GameStateStore gameStateStore = new GameStateStore();
        State dummyState = new State();
        gameStateStore.UpdateState(dummyState);
        gameStateStore.IsConnectedWithEmulator = true;

        gameStateStore.ClearState();

        Assert.Null(gameStateStore.CurrentState);
        Assert.False(gameStateStore.IsConnectedWithEmulator);
    }
}
