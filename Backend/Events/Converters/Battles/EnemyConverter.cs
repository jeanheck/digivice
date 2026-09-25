using Backend.Domain.Models.Battles;
using Backend.Events.Converters.Parties.Digimons;
using Backend.Events.DTO.Battles;

namespace Backend.Events.Converters.Battles;

public static class EnemyConverter
{
    public static EnemyDTO ToDTO(Enemy enemy) => new()
    {
        Id = enemy.Id,
        GroupId = enemy.GroupId,
        Condition = enemy.Condition,
        Strength = enemy.Strength,
        Defense = enemy.Defense,
        Speed = enemy.Speed,
        HP = VitalConverter.ToDTO(enemy.HP)
    };
}
