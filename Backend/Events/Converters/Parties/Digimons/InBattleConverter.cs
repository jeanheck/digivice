using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.DTO.Parties.Digimons;

namespace Backend.Events.Converters.Parties.Digimons;

public static class InBattleConverter
{
    public static InBattleDTO ToDTO(InBattle digimonInBattle) => new()
    {
        Condition = digimonInBattle.Condition,
        Strength = digimonInBattle.Strength,
        Defense = digimonInBattle.Defense,
        Speed = digimonInBattle.Speed,
        HP = VitalConverter.ToDTO(digimonInBattle.HP),
        MP = VitalConverter.ToDTO(digimonInBattle.MP)
    };
}
