using Backend.Domain.Models;
using Backend.Events.Models;
using Backend.Events.Models.State;
using Backend.Events.Diffing.Extensions;

namespace Backend.Events.Factory;

public static class StateEventFactory
{
    public static IEnumerable<BaseEvent> Create(State? previousState, State newState)
    {
        if (newState.HasNoChanges(previousState))
        {
            return [];
        }

        if (previousState == null)
        {
            return [new InitialStateEvent(newState)];
        }

        var events = new List<BaseEvent>();

        events.AddRange(PlayerEventFactory.Create(previousState, newState));
        events.AddRange(PartyEventFactory.Create(previousState, newState));
        events.AddRange(JournalEventFactory.Create(previousState, newState));

        return events;
    }
}
