using Backend.Domain.Models;

namespace Backend.Events.States;

public class GameStateStore : IGameStateStore
{
    public State? CurrentState { get; private set; }
    public bool? IsEmulatorConnected { get; set; }

    public void UpdateState(State state) => CurrentState = state;
    
    public void ClearState()
    {
        CurrentState = null;
        IsEmulatorConnected = false;
    }
}
