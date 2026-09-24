using Backend.Domain.Models;
using Backend.Events.Converters;
using Backend.Events.Diffing.Battles;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO;
using Backend.Events.DTO.Battles;
using Backend.Events.DTO.Shared;

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

        var enemyDelta = DiffEnemy(previousDigimonBattle, newDigimonBattle);
        bool fieldChanged = previousDigimonBattle.Field != newDigimonBattle.Field;

        if (!enemyDelta.HasValue && !fieldChanged)
        {
            return new DigimonBattleDTO();
        }

        var dto = new DigimonBattleDTO();
        if (enemyDelta.HasValue)
        {
            dto = dto with { Enemy = enemyDelta };
        }
        if (fieldChanged)
        {
            dto = dto with { Field = newDigimonBattle.Field };
        }

        return dto;
    }

    private static Optional<EnemyDTO?> DiffEnemy(DigimonBattle previousDigimonBattle, DigimonBattle newDigimonBattle)
    {
        if (newDigimonBattle.Enemy == null)
        {
            if (previousDigimonBattle.Enemy == null)
            {
                return Optional<EnemyDTO?>.Empty;
            }

            return new Optional<EnemyDTO?>(null);
        }

        var enemyDelta = EnemyDiffer.Diff(previousDigimonBattle.Enemy, newDigimonBattle.Enemy);
        if (enemyDelta == null)
        {
            return Optional<EnemyDTO?>.Empty;
        }

        return enemyDelta;
    }
}
