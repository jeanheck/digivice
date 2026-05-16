using Backend.Domain.Models;
using Backend.Events.Models;
using Backend.Events.Models.Party;

namespace Backend.Events.Diffing;

public static class PartyDiffer
{
    public static IEnumerable<BaseEvent> Diff(Party? oldParty, Party? newParty)
    {
        var events = new List<BaseEvent>();

        if (newParty == null) return events;

        if (oldParty == null)
        {
            var activeDigimons = newParty.Slots.Where(d => d != null).Select(d => d!).ToList();
            events.Add(new PartySlotsChangedEvent(activeDigimons));
            return events;
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
                        events.AddRange(DigimonDiffer.Diff(i, oldSlots[i], newSlots[i]!));
                    }
                }
            }
        }

        return events;
    }
}
