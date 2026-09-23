using Backend.Domain.Models;

namespace Backend.Events.States;

public interface IGameStateStore
{
    State? CurrentState { get; }
    bool? IsHealthy { get; set; }
    string? LastErrorCode { get; set; }
    string? LastErrorDetail { get; set; }
    void UpdateState(State state);
    void ClearState();
}
