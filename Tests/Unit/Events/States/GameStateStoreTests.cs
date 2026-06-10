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

    [Fact]
    public void RegisterClientConnection_ShouldIncrementConnectedClientCount()
    {
        GameStateStore gameStateStore = new GameStateStore();

        gameStateStore.RegisterClientConnection();
        gameStateStore.RegisterClientConnection();

        Assert.Equal(2, gameStateStore.ConnectedClientCount);
        Assert.True(gameStateStore.HasConnectedClients);
    }

    [Fact]
    public void UnregisterClientConnection_ShouldDecrementConnectedClientCount()
    {
        GameStateStore gameStateStore = new GameStateStore();
        gameStateStore.RegisterClientConnection();
        gameStateStore.RegisterClientConnection();

        gameStateStore.UnregisterClientConnection();

        Assert.Equal(1, gameStateStore.ConnectedClientCount);
        Assert.True(gameStateStore.HasConnectedClients);
    }

    [Fact]
    public void UnregisterClientConnection_ShouldNotGoBelowZero()
    {
        GameStateStore gameStateStore = new GameStateStore();

        gameStateStore.UnregisterClientConnection();

        Assert.Equal(0, gameStateStore.ConnectedClientCount);
        Assert.False(gameStateStore.HasConnectedClients);
    }

    [Fact]
    public void ClearState_ShouldNotResetConnectedClientCount()
    {
        GameStateStore gameStateStore = new GameStateStore();
        gameStateStore.RegisterClientConnection();
        gameStateStore.UpdateState(new State());
        gameStateStore.IsConnectedWithEmulator = true;

        gameStateStore.ClearState();

        Assert.Equal(1, gameStateStore.ConnectedClientCount);
    }
}
