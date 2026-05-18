using Backend.Domain.Models;
using Backend.Events.Models;

namespace Backend.Events.Factory;

public static class GameEventFactory
{
    public static IEnumerable<BaseEvent> Create(State? previousState, State newState)
    {
        return StateEventFactory.Create(previousState, newState);
    }
}
