using Backend.Domain.Models;

namespace Backend.Events.States;

public interface IGameStateStore
{
    State? CurrentState { get; }
    bool? IsConnectedWithEmulator { get; set; }
    int ConnectedClientCount { get; }
    bool HasConnectedClients => ConnectedClientCount > 0;
    void UpdateState(State state);
    void ClearState();
    void RegisterClientConnection();
    void UnregisterClientConnection();
}
