using Backend.Domain.Models;
using Backend.Events.Converters;
using Backend.Events.Diffing.Battles;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO;

namespace Backend.Events.Diffing;

public static class BattleDiffer
{
    public static BattleDTO Diff(Battle? previousBattle, Battle newBattle)
    {
        if (newBattle.HasNoChanges(previousBattle))
        {
            return new BattleDTO();
        }

        if (previousBattle == null)
        {
            return BattleConverter.ToDTO(newBattle);
        }

        var enemyDelta = EnemyDiffer.Diff(previousBattle.Enemy, newBattle.Enemy);
        bool fieldChanged = previousBattle.Field != newBattle.Field;

        if (enemyDelta == null && !fieldChanged)
        {
            return new BattleDTO();
        }

        var dto = new BattleDTO();
        if (enemyDelta != null)
        {
            dto = dto with { Enemy = enemyDelta };
        }
        if (fieldChanged)
        {
            dto = dto with { Field = newBattle.Field };
        }

        return dto;
    }
}
