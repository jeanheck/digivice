using Backend.Domain.Models;
using Backend.Events.Models;

namespace Backend.Events.States;

public interface IGameStateStore
{
    State? CurrentState { get; }
    HealthStatus Status { get; set; }
    string? LastErrorCode { get; set; }
    string? LastErrorDetail { get; set; }
    void UpdateState(State state);
    void ClearState();
}
