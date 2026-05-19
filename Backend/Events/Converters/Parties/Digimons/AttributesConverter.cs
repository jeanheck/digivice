using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.DTO.Parties.Digimons;

namespace Backend.Events.Converters.Parties.Digimons;

public static class AttributesConverter
{
    public static AttributesDTO ToDTO(Attributes attributes) => new()
    {
        Strength = attributes.Strength,
        Defense = attributes.Defense,
        Spirit = attributes.Spirit,
        Wisdom = attributes.Wisdom,
        Speed = attributes.Speed,
        Charisma = attributes.Charisma
    };
}
