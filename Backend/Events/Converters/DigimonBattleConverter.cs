using Backend.Domain.Models;
using Backend.Events.Converters.Battles;
using Backend.Events.DTO;

namespace Backend.Events.Converters;

public static class DigimonBattleConverter
{
    public static DigimonBattleDTO ToDTO(DigimonBattle digimonBattle) => new()
    {
        Field = digimonBattle.Field,
        Enemy = digimonBattle.Enemy != null ? EnemyConverter.ToDTO(digimonBattle.Enemy) : null
    };
}
