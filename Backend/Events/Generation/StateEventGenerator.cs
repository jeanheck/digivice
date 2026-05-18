using Backend.Events.Models;
using Backend.Events.Models.State;
using Backend.Domain.Models;
using Backend.Events.Diffing;

namespace Backend.Events.Generation;

public static class StateEventGenerator
{
    public static IEnumerable<BaseEvent> Generate(State? previousState, State newState)
    {
        if (previousState == null)
        {
            return [new InitialStateEvent(newState)];
        }

        var events = new List<BaseEvent>();

        events.AddRange(PlayerDiffer.Diff(previousState.Player, newState.Player));
        events.AddRange(PartyDiffer.Diff(previousState.Party, newState.Party));
        events.AddRange(JournalDiffer.Diff(previousState.Journal, newState.Journal));

        return events;
    }
}