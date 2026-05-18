using Backend.Domain.Models;
using Backend.Events.Converters;
using Backend.Events.Diffing.Extensions;
using Backend.Events.Models;

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
            return [new BaseEvent(EventType.InitialState, StateConverter.ToDTO(newState))];
        }

        var events = new List<BaseEvent>();

        events.AddRange(PlayerEventFactory.Create(previousState, newState));
        events.AddRange(PartyEventFactory.Create(previousState, newState));
        events.AddRange(JournalEventFactory.Create(previousState, newState));

        return events;
    }
}
