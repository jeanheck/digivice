using Backend.Domain.Models;

namespace Backend.Events.States;

public interface IGameStateStore
{
    State? CurrentState { get; }
    bool? IsConnectedWithEmulator { get; set; }
    string? LastEmulatorConnectionErrorCode { get; set; }
    string? LastEmulatorConnectionErrorDetail { get; set; }
    void UpdateState(State state);
    void ClearState();
}
