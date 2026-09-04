using Backend.Domain.Models;
using Backend.Events.Converters;
using Backend.Events.Diffing.Battles;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO;

namespace Backend.Events.Diffing;

public static class DigimonBattleDiffer
{
    public static DigimonBattleDTO Diff(DigimonBattle? previousDigimonBattle, DigimonBattle newDigimonBattle)
    {
        if (newDigimonBattle.HasNoChanges(previousDigimonBattle))
        {
            return new DigimonBattleDTO();
        }

        if (previousDigimonBattle == null)
        {
            return DigimonBattleConverter.ToDTO(newDigimonBattle);
        }

        var enemyDelta = EnemyDiffer.Diff(previousDigimonBattle.Enemy, newDigimonBattle.Enemy);
        bool fieldChanged = previousDigimonBattle.Field != newDigimonBattle.Field;

        if (enemyDelta == null && !fieldChanged)
        {
            return new DigimonBattleDTO();
        }

        var dto = new DigimonBattleDTO();
        if (enemyDelta != null)
        {
            dto = dto with { Enemy = enemyDelta };
        }
        if (fieldChanged)
        {
            dto = dto with { Field = newDigimonBattle.Field };
        }

        return dto;
    }
}
