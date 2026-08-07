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
        bool strengthChanged = previousInCombat.Strength != newInCombat.Strength;
        bool defenseChanged = previousInCombat.Defense != newInCombat.Defense;
        bool speedChanged = previousInCombat.Speed != newInCombat.Speed;
        var hpDelta = VitalDiffer.Diff(previousInCombat.HP, newInCombat.HP);
        var mpDelta = VitalDiffer.Diff(previousInCombat.MP, newInCombat.MP);

        if (!conditionChanged && !strengthChanged && !defenseChanged && !speedChanged && hpDelta == null && mpDelta == null)
        {
            return null;
        }

        var dto = new InCombatDTO();
        if (conditionChanged)
        {
            dto = dto with { Condition = newInCombat.Condition };
        }
        if (strengthChanged)
        {
            dto = dto with { Strength = newInCombat.Strength };
        }
        if (defenseChanged)
        {
            dto = dto with { Defense = newInCombat.Defense };
        }
        if (speedChanged)
        {
            dto = dto with { Speed = newInCombat.Speed };
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
