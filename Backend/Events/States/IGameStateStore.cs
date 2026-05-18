using Backend.Domain.Models;

namespace Backend.Events.States;

public interface IGameStateStore
{
    State? CurrentState { get; }
    bool? IsEmulatorConnected { get; set; }
    void UpdateState(State state);
    void ClearState();
}
