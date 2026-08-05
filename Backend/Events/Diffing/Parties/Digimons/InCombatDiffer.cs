using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties.Digimons;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO.Parties.Digimons;

namespace Backend.Events.Diffing.Parties.Digimons;

public static class InCombatDiffer
{
    public static InCombatDTO? Diff(InCombat? previousInCombat, InCombat newInCombat)
    {
        if (newInCombat.HasNoChanges(previousInCombat))
        {
            return null;
        }

        if (previousInCombat == null)
        {
            return InCombatConverter.ToDTO(newInCombat);
        }

        bool conditionChanged = previousInCombat.Condition != newInCombat.Condition;
        var hpDelta = VitalDiffer.Diff(previousInCombat.HP, newInCombat.HP);
        var mpDelta = VitalDiffer.Diff(previousInCombat.MP, newInCombat.MP);

        if (!conditionChanged && hpDelta == null && mpDelta == null)
        {
            return null;
        }

        var dto = new InCombatDTO();
        if (conditionChanged)
        {
            dto = dto with { Condition = newInCombat.Condition };
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
