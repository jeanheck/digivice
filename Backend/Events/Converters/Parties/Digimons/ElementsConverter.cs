using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.DTO.Parties.Digimons;

namespace Backend.Events.Converters.Parties.Digimons;

public static class ElementsConverter
{
    public static ElementsDTO ToDTO(Elements elements) => new()
    {
        Fire = elements.Fire,
        Water = elements.Water,
        Ice = elements.Ice,
        Wind = elements.Wind,
        Thunder = elements.Thunder,
        Machine = elements.Machine,
        Dark = elements.Dark
    };
}
