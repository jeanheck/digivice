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
        if (enemyDelta == null)
        {
            return new BattleDTO();
        }

        return new BattleDTO { Enemy = enemyDelta };
    }
}
