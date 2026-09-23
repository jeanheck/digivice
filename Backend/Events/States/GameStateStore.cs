using Backend.Domain.Models;
using Backend.Events.Models;

namespace Backend.Events.States;

public class GameStateStore : IGameStateStore
{
    public State? CurrentState { get; private set; }
    public HealthStatus Status { get; set; } = HealthStatus.Loading;
    public string? LastErrorCode { get; set; }
    public string? LastErrorDetail { get; set; }

    public void UpdateState(State state) => CurrentState = state;

    public void ClearState()
    {
        CurrentState = null;
    }
}
