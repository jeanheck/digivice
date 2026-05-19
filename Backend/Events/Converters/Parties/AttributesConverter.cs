using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.DTO.Party.Digimon;

namespace Backend.Events.Converters.Parties;

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
