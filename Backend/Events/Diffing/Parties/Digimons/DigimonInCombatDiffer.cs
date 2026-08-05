using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties.Digimons;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO.Parties.Digimons;

namespace Backend.Events.Diffing.Parties.Digimons;

public static class DigimonInCombatDiffer
{
    public static DigimonInCombatDTO? Diff(DigimonInCombat? previousDigimonInCombat, DigimonInCombat newDigimonInCombat)
    {
        if (newDigimonInCombat.HasNoChanges(previousDigimonInCombat))
        {
            return null;
        }

        if (previousDigimonInCombat == null)
        {
            return DigimonInCombatConverter.ToDTO(newDigimonInCombat);
        }

        bool conditionChanged = previousDigimonInCombat.Condition != newDigimonInCombat.Condition;
        var hpDelta = VitalDiffer.Diff(previousDigimonInCombat.HP, newDigimonInCombat.HP);
        var mpDelta = VitalDiffer.Diff(previousDigimonInCombat.MP, newDigimonInCombat.MP);

        if (!conditionChanged && hpDelta == null && mpDelta == null)
        {
            return null;
        }

        var dto = new DigimonInCombatDTO();
        if (conditionChanged)
        {
            dto = dto with { Condition = newDigimonInCombat.Condition };
        }
        if (hpDelta != null)
        {
            dto = dto with { HP = hpDelta };
        }
        if (mpDelta != null)
        {
            dto = dto with { MP = mpDelta };
        }

        return dto;
    }
}
