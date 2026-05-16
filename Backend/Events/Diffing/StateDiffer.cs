using Backend.Events.Models;
using Backend.Events.Models.State;
using Backend.Domain.Models;

namespace Backend.Events.Diffing;

public static class StateDiffer
{
    public static IEnumerable<BaseEvent> Diff(State? previousState, State newState)
    {
        var events = new List<BaseEvent>();

        if (previousState == null)
        {
            events.Add(new InitialStateEvent(newState));
            return events;
        }

        return events;
    }
}