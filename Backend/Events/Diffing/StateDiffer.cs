using Backend.Events.Models;
using Backend.Events.Models.State;
using Backend.Domain.Models;
using Backend.Events.Diffing.Extensions;

namespace Backend.Events.Diffing;

public static class StateDiffer
{
    public static IEnumerable<BaseEvent> Diff(State? previousState, State newState)
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

        events.AddRange(PlayerDiffer.Diff(previousState.Player, newState.Player));
        events.AddRange(PartyDiffer.Diff(previousState.Party, newState.Party));
        events.AddRange(JournalDiffer.Diff(previousState.Journal, newState.Journal));

        return events;
    }
}