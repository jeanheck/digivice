using Backend.Events.Models;
using Backend.Events.Models.Journal;
using Backend.Events.Models.Party;
using Backend.Events.Models.State;
using Backend.Domain.Models;

namespace Backend.Events.Diffing;

public class StateDiffer(PlayerDiffer playerDiffer, DigimonDiffer digimonDiffer) : IDiffer<State>
{
    public IEnumerable<BaseEvent> Diff(State? previousState, State newState)
    {
        var events = new List<BaseEvent>();

        if (previousState == null)
        {
            events.Add(new InitialStateEvent(newState));
            return events;
        }

        events.AddRange(playerDiffer.Diff(previousState.Player, newState.Player));

        // 2. Party Comparison
        DetectPartyChanges(previousState.Party, newState.Party, events);

        // 3. Journal Comparison
        if (HasChanged(previousState.Journal, newState.Journal))
        {
            events.Add(new JournalChangedEvent(newState.Journal));
        }

        return events;
    }

    private void DetectPartyChanges(Party? oldParty, Party? newParty, List<BaseEvent> events)
    {
        if (newParty == null) return;

        if (oldParty == null)
        {
            var activeDigimons = newParty.Slots.Where(d => d != null).Select(d => d!).ToList();
            events.Add(new PartySlotsChangedEvent(activeDigimons));
            return;
        }

        var newSlots = newParty.Slots;
        var oldSlots = oldParty.Slots;

        bool partyRosterChanged = false;
        if (newSlots.Count != oldSlots.Count)
        {
            partyRosterChanged = true;
        }
        else
        {
            for (int i = 0; i < newSlots.Count; i++)
            {
                var newDigi = newSlots[i];
                var oldDigi = oldSlots[i];

                if ((newDigi == null && oldDigi != null) || (newDigi != null && oldDigi == null))
                {
                    partyRosterChanged = true;
                    break;
                }
            }
        }

        if (partyRosterChanged)
        {
            var activeDigimons = newSlots.Where(d => d != null).Select(d => d!).ToList();
            events.Add(new PartySlotsChangedEvent(activeDigimons));
        }
        else
        {
            for (int i = 0; i < newSlots.Count; i++)
            {
                if (newSlots[i] != null && oldSlots[i] != null)
                {
                    if (!newSlots[i]!.Equals(oldSlots[i]))
                    {
                        events.AddRange(digimonDiffer.Diff(i, oldSlots[i], newSlots[i]!));
                    }
                }
            }
        }
    }

    private static bool HasChanged<T>(T? oldVal, T? newVal) where T : class
    {
        if (oldVal == null && newVal == null) return false;
        if (oldVal == null || newVal == null) return true;
        return !oldVal.Equals(newVal);
    }
}
