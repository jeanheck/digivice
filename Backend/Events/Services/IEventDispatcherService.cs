using Backend.Events.Models;

namespace Backend.Events.Services;

public interface IEventDispatcherService
{
    void DispatchEmulatorConnectionStatus(bool isConnectedWithEmulator);
    void DispatchEvents(IEnumerable<Event> events);
    void DispatchInitialStateToClient(string connectionId);
}
