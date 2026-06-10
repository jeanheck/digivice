using Backend.Domain.Models;

namespace Backend.Events.States;

public class GameStateStore : IGameStateStore
{
    public State? CurrentState { get; private set; }
    public bool? IsConnectedWithEmulator { get; set; }
    public int ConnectedClientCount { get; private set; }
    public bool HasConnectedClients => ConnectedClientCount > 0;

    public void UpdateState(State state) => CurrentState = state;

    public void ClearState()
    {
        CurrentState = null;
        IsConnectedWithEmulator = false;
    }

    public void RegisterClientConnection()
    {
        ConnectedClientCount++;
    }

    public void UnregisterClientConnection()
    {
        if (ConnectedClientCount > 0)
        {
            ConnectedClientCount--;
        }
    }
}
