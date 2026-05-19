using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO.Party.Digimon;

namespace Backend.Events.Diffing.Parties.Digimons;

public static class AttributesDiffer
{
    public static AttributesDTO? Diff(Attributes? previousAttributes, Attributes newAttributes)
    {
        if (newAttributes.HasNoChanges(previousAttributes))
        {
            return null;
        }

        if (previousAttributes == null)
        {
            return AttributesConverter.ToDTO(newAttributes);
        }

        var dto = new AttributesDTO();

        if (newAttributes.Strength != previousAttributes.Strength)
        {
            dto = dto with { Strength = newAttributes.Strength };
        }
        if (newAttributes.Defense != previousAttributes.Defense)
        {
            dto = dto with { Defense = newAttributes.Defense };
        }
        if (newAttributes.Spirit != previousAttributes.Spirit)
        {
            dto = dto with { Spirit = newAttributes.Spirit };
        }
        if (newAttributes.Wisdom != previousAttributes.Wisdom)
        {
            dto = dto with { Wisdom = newAttributes.Wisdom };
        }
        if (newAttributes.Speed != previousAttributes.Speed)
        {
            dto = dto with { Speed = newAttributes.Speed };
        }
        if (newAttributes.Charisma != previousAttributes.Charisma)
        {
            dto = dto with { Charisma = newAttributes.Charisma };
        }

        return dto;
    }
}
