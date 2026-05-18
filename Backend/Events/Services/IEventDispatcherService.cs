using Backend.Events.Models;

namespace Backend.Events.Services;

public interface IEventDispatcherService
{
    void DispatchConnectionStatus(bool isConnected);
    void DispatchEvents(IEnumerable<BaseEvent> events);
    void DispatchInitialStateToClient(string connectionId);
}
