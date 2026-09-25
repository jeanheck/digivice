using Backend.Events.Converters.Battles;
using Backend.Events.Diffing.Extensions;
using Backend.Events.Diffing.Parties.Digimons;
using Backend.Events.DTO.Battles;
using Backend.Domain.Models.Battles;

namespace Backend.Events.Diffing.Battles;

public static class EnemyDiffer
{
    public static EnemyDTO? Diff(Enemy? previousEnemy, Enemy newEnemy)
    {
        if (newEnemy.HasNoChanges(previousEnemy))
        {
            return null;
        }

        if (previousEnemy == null)
        {
            return EnemyConverter.ToDTO(newEnemy);
        }

        bool idChanged = previousEnemy.Id != newEnemy.Id;
        bool groupIdChanged = previousEnemy.GroupId != newEnemy.GroupId;
        bool conditionChanged = previousEnemy.Condition != newEnemy.Condition;
        bool strengthChanged = previousEnemy.Strength != newEnemy.Strength;
        bool defenseChanged = previousEnemy.Defense != newEnemy.Defense;
        bool speedChanged = previousEnemy.Speed != newEnemy.Speed;
        var hpDelta = VitalDiffer.Diff(previousEnemy.HP, newEnemy.HP);

        if (!idChanged && !groupIdChanged && !conditionChanged && !strengthChanged && !defenseChanged && !speedChanged
            && hpDelta == null)
        {
            return null;
        }

        var dto = new EnemyDTO();
        if (idChanged)
        {
            dto = dto with { Id = newEnemy.Id };
        }
        if (groupIdChanged)
        {
            dto = dto with { GroupId = newEnemy.GroupId };
        }
        if (conditionChanged)
        {
            dto = dto with { Condition = newEnemy.Condition };
        }
        if (strengthChanged)
        {
            dto = dto with { Strength = newEnemy.Strength };
        }
        if (defenseChanged)
        {
            dto = dto with { Defense = newEnemy.Defense };
        }
        if (speedChanged)
        {
            dto = dto with { Speed = newEnemy.Speed };
        }
        if (hpDelta != null)
        {
            dto = dto with { HP = hpDelta };
        }

        return dto;
    }
}
