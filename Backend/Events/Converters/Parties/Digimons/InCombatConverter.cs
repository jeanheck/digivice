using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.DTO.Parties.Digimons;

namespace Backend.Events.Converters.Parties.Digimons;

public static class InCombatConverter
{
    public static InCombatDTO ToDTO(InCombat digimonInCombat) => new()
    {
        Condition = digimonInCombat.Condition,
        Strength = digimonInCombat.Strength,
        Defense = digimonInCombat.Defense,
        Speed = digimonInCombat.Speed,
        HP = VitalConverter.ToDTO(digimonInCombat.HP),
        MP = VitalConverter.ToDTO(digimonInCombat.MP)
    };
}
