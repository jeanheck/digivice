using Backend.Domain.Models;
using Backend.Events.Converters;
using Backend.Events.Diffing.Extensions;
using Backend.Events.Models;

namespace Backend.Events.Factory;

public static class StateEventFactory
{
    public static IEnumerable<Event> Create(State? previousState, State newState)
    {
        if (newState.HasNoChanges(previousState))
        {
            return [];
        }

        if (previousState == null)
        {
            return [new Event(EventType.InitialState, StateConverter.ToDTO(newState))];
        }

        List<Event> events = [];

        events.AddRange(PlayerEventFactory.Create(previousState, newState));
        events.AddRange(ImportantItemsEventFactory.Create(previousState, newState));
        events.AddRange(PartyEventFactory.Create(previousState, newState));
        events.AddRange(DigimonBattleEventFactory.Create(previousState, newState));
        events.AddRange(CardBattleEventFactory.Create(previousState, newState));
        events.AddRange(AuctionsEventFactory.Create(previousState, newState));
        events.AddRange(NpcsEventFactory.Create(previousState, newState));
        events.AddRange(JournalEventFactory.Create(previousState, newState));

        return events;
    }
}
