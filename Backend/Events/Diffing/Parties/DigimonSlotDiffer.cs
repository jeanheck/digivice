using Backend.Domain.Models.Parties;
using Backend.Events.Converters.Parties;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO.Parties;

namespace Backend.Events.Diffing.Parties;

public static class DigimonSlotDiffer
{
    public static DigimonSlotDTO? Diff(DigimonSlot? previousDigimonSlot, DigimonSlot newDigimonSlot)
    {
        if (newDigimonSlot.HasNoChanges(previousDigimonSlot))
        {
            return null;
        }

        if (previousDigimonSlot == null)
        {
            return DigimonSlotConverter.ToDTO(newDigimonSlot);
        }

        if (previousDigimonSlot.DigimonId != newDigimonSlot.DigimonId)
        {
            if (newDigimonSlot.Digimon == null)
            {
                return new DigimonSlotDTO
                {
                    Index = newDigimonSlot.Index,
                    DigimonId = null,
                    Digimon = null
                };
            }

            return DigimonSlotConverter.ToDTO(newDigimonSlot);
        }

        var digimonDelta = DigimonDiffer.Diff(previousDigimonSlot.Digimon, newDigimonSlot.Digimon);
        if (digimonDelta == null)
        {
            return null;
        }

        return new DigimonSlotDTO
        {
            Index = newDigimonSlot.Index,
            DigimonId = newDigimonSlot.DigimonId,
            Digimon = digimonDelta
        };
    }
}
