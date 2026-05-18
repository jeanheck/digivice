using Backend.Domain.Models;
using Backend.Events.DTO;
using Backend.Events.Diffing.Extensions;

namespace Backend.Events.Diffing;

public static class PartyDiffer
{
    public static PartyDTO Diff(Party? previousParty, Party newParty)
    {
        if (newParty.HasNoChanges(previousParty))
        {
            return new PartyDTO();
        }

        /*
        if (previousParty == null)
        {
            var activeDigimons = newParty.Slots
                .Where(s => s.Digimon != null)
                .Select(s => s.Digimon!)
                .ToList();
            events.Add(new PartySlotsChangedEvent(activeDigimons));
            return events;
        }

        var newSlots = newParty.Slots;
        var oldSlots = previousParty.Slots;

        bool partyRosterChanged = false;
        if (newSlots.Count != oldSlots.Count)
        {
            partyRosterChanged = true;
        }
        else
        {
            for (int i = 0; i < newSlots.Count; i++)
            {
                var newDigi = newSlots[i].Digimon;
                var oldDigi = oldSlots[i].Digimon;

                if ((newDigi == null && oldDigi != null) || (newDigi != null && oldDigi == null))
                {
                    partyRosterChanged = true;
                    break;
                }
            }
        }

        if (partyRosterChanged)
        {
            var activeDigimons = newSlots
                .Where(s => s.Digimon != null)
                .Select(s => s.Digimon!)
                .ToList();
            events.Add(new PartySlotsChangedEvent(activeDigimons));
        }
        else
        {
            for (int i = 0; i < newSlots.Count; i++)
            {
                var newDigi = newSlots[i].Digimon;
                var oldDigi = oldSlots[i].Digimon;
                if (newDigi != null && oldDigi != null)
                {
                    if (!newDigi.Equals(oldDigi))
                    {
                        events.AddRange(DigimonDiffer.Diff(i, oldDigi, newDigi));
                    }
                }
            }
        }
        */

        return new PartyDTO();
    }
}
