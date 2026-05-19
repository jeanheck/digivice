using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.DTO.Parties.Digimons;

namespace Backend.Events.Converters.Parties.Digimons;

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
