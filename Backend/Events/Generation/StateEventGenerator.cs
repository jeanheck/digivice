using Backend.Events.Models;
using Backend.Domain.Models;
using Backend.Events.Diffing;

namespace Backend.Events.Generation;

public class StateEventGenerator()
{
    public IEnumerable<BaseEvent> Generate(State? previousState, State newState)
    {
        var events = new List<BaseEvent>();

        events.AddRange(StateDiffer.Diff(previousState, newState));
        events.AddRange(PlayerDiffer.Diff(previousState?.Player, newState.Player));
        events.AddRange(PartyDiffer.Diff(previousState?.Party, newState.Party));
        events.AddRange(JournalDiffer.Diff(previousState?.Journal, newState.Journal));

        return events;
    }
}