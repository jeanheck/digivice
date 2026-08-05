using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.DTO.Parties.Digimons;

namespace Backend.Events.Converters.Parties.Digimons;

public static class DigimonInCombatConverter
{
    public static DigimonInCombatDTO ToDTO(DigimonInCombat digimonInCombat) => new()
    {
        Condition = digimonInCombat.Condition,
        HP = VitalConverter.ToDTO(digimonInCombat.HP),
        MP = VitalConverter.ToDTO(digimonInCombat.MP)
    };
}
