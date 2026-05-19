using Backend.Domain.Models;
using Backend.Events.Converters;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO;
using Backend.Events.DTO.Party;
using Backend.Events.Diffing.Parties;

namespace Backend.Events.Diffing;

public static class PartyDiffer
{
    public static PartyDTO Diff(Party? previousParty, Party newParty)
    {
        if (newParty.HasNoChanges(previousParty))
        {
            return new PartyDTO();
        }

        if (previousParty == null)
        {
            return PartyConverter.ToDTO(newParty);
        }

        var dto = new PartyDTO();

        var digimonSlotsDelta = new List<DigimonSlotDTO>();
        foreach (var newSlot in newParty.Slots)
        {
            var previousSlot = previousParty.Slots.FirstOrDefault(s => s.Index == newSlot.Index);
            var digimonSlotDelta = DigimonSlotDiffer.Diff(previousSlot, newSlot);
            if (digimonSlotDelta != null)
            {
                digimonSlotsDelta.Add(digimonSlotDelta);
            }
        }

        if (digimonSlotsDelta.Count > 0)
        {
            dto = dto with { Slots = digimonSlotsDelta };
        }

        return dto;
    }
}
