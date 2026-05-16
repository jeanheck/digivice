using Backend.Domain.Models;

namespace Backend.Events.Services;

public interface IEventDispatcherService
{
    void DispatchConnectionStatus(bool isConnected);
    void ProcessGameState(State newState);
    void DispatchInitialStateToClient(string connectionId);
}
