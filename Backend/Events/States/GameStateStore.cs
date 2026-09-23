using Backend.Domain.Models;

namespace Backend.Events.States;

public class GameStateStore : IGameStateStore
{
    public State? CurrentState { get; private set; }
    public bool? IsHealthy { get; set; }
    public string? LastErrorCode { get; set; }
    public string? LastErrorDetail { get; set; }

    public void UpdateState(State state) => CurrentState = state;

    public void ClearState()
    {
        CurrentState = null;
        IsHealthy = false;
    }
}
