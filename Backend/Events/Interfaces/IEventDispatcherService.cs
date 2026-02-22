using Backend.Models;

namespace Backend.Events.Interfaces;

public interface IEventDispatcherService
{
    void DispatchConnectionStatus(bool isConnected);
    void ProcessGameState(Player newState);
}
