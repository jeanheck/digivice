using Backend.Domain.Models;

namespace Backend.Events.Interfaces;

public interface IEventDispatcherService
{
    void DispatchConnectionStatus(bool isConnected);
    void ProcessGameState(State newState);
    void DispatchInitialStateToClient(string connectionId);
}
