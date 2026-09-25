using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties.Digimons;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO.Parties.Digimons;

namespace Backend.Events.Diffing.Parties.Digimons;

public static class InBattleDiffer
{
    public static InBattleDTO? Diff(InBattle? previousInBattle, InBattle newInBattle)
    {
        if (newInBattle.HasNoChanges(previousInBattle))
        {
            return null;
        }

        if (previousInBattle == null)
        {
            return InBattleConverter.ToDTO(newInBattle);
        }

        bool conditionChanged = previousInBattle.Condition != newInBattle.Condition;
        bool strengthChanged = previousInBattle.Strength != newInBattle.Strength;
        bool defenseChanged = previousInBattle.Defense != newInBattle.Defense;
        bool speedChanged = previousInBattle.Speed != newInBattle.Speed;
        var hpDelta = VitalDiffer.Diff(previousInBattle.HP, newInBattle.HP);
        var mpDelta = VitalDiffer.Diff(previousInBattle.MP, newInBattle.MP);

        if (!conditionChanged && !strengthChanged && !defenseChanged && !speedChanged && hpDelta == null && mpDelta == null)
        {
            return null;
        }

        var dto = new InBattleDTO();
        if (conditionChanged)
        {
            dto = dto with { Condition = newInBattle.Condition };
        }
        if (strengthChanged)
        {
            dto = dto with { Strength = newInBattle.Strength };
        }
        if (defenseChanged)
        {
            dto = dto with { Defense = newInBattle.Defense };
        }
        if (speedChanged)
        {
            dto = dto with { Speed = newInBattle.Speed };
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
