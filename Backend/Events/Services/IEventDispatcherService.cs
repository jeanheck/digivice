using Backend.Events.Models;

namespace Backend.Events.Services;

public interface IEventDispatcherService
{
    void DispatchEvents(IEnumerable<Event> events);
    void DispatchInitialStateToClient(string connectionId);
}
