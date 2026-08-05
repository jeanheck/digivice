using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.DTO.Parties.Digimons;

namespace Backend.Events.Converters.Parties.Digimons;

public static class InCombatConverter
{
    public static InCombatDTO ToDTO(InCombat digimonInCombat) => new()
    {
        Condition = digimonInCombat.Condition,
        HP = VitalConverter.ToDTO(digimonInCombat.HP),
        MP = VitalConverter.ToDTO(digimonInCombat.MP)
    };
}
