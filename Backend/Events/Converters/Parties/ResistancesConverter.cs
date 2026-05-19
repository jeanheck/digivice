using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.DTO.Party.Digimon;

namespace Backend.Events.Converters.Parties;

public static class ResistancesConverter
{
    public static ResistancesDTO ToDTO(Resistances resistances) => new()
    {
        Fire = resistances.Fire,
        Water = resistances.Water,
        Ice = resistances.Ice,
        Wind = resistances.Wind,
        Thunder = resistances.Thunder,
        Machine = resistances.Machine,
        Dark = resistances.Dark
    };
}
