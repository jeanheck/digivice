using Backend.Domain.Models;
using Backend.Events.Converters.Battles;
using Backend.Events.DTO;

namespace Backend.Events.Converters;

public static class BattleConverter
{
    public static BattleDTO ToDTO(Battle battle) => new()
    {
        Field = battle.Field,
        Enemy = EnemyConverter.ToDTO(battle.Enemy)
    };
}
