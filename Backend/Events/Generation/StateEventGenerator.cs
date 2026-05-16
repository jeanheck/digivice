using Backend.Events.Models;
using Backend.Domain.Models;
using Backend.Events.Diffing;

public class StateEventGenerator(
    StateDiffer stateDiffer,
    PlayerDiffer playerDiffer,
    JournalDiffer journalDiffer,
    PartyDiffer partyDiffer)
{
    public IEnumerable<BaseEvent> Generate(State? previousState, State newState)
    {
        var events = new List<BaseEvent>();

        events.AddRange(stateDiffer.Diff(previousState, newState));
        events.AddRange(playerDiffer.Diff(previousState?.Player, newState.Player));
        events.AddRange(partyDiffer.Diff(previousState?.Party, newState.Party));
        events.AddRange(journalDiffer.Diff(previousState?.Journal, newState.Journal));

        return events;
    }
}