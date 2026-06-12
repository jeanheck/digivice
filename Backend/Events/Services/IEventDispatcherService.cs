using Backend.Events.Models;

namespace Backend.Events.Services;

public interface IEventDispatcherService
{
    void DispatchEmulatorConnectionStatus(
        bool isConnectedWithEmulator,
        string? errorCode = null,
        string? errorDetail = null);
    void DispatchEvents(IEnumerable<Event> events);
    void DispatchInitialStateToClient(string connectionId);
}
