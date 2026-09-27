using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties.Digimons;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO.Parties.Digimons;

namespace Backend.Events.Diffing.Parties.Digimons;

public static class ElementsDiffer
{
    public static ElementsDTO? Diff(Elements? previousElements, Elements newElements)
    {
        if (newElements.HasNoChanges(previousElements))
        {
            return null;
        }

        if (previousElements == null)
        {
            return ElementsConverter.ToDTO(newElements);
        }

        var dto = new ElementsDTO();

        if (newElements.Fire != previousElements.Fire)
        {
            dto = dto with { Fire = newElements.Fire };
        }
        if (newElements.Water != previousElements.Water)
        {
            dto = dto with { Water = newElements.Water };
        }
        if (newElements.Ice != previousElements.Ice)
        {
            dto = dto with { Ice = newElements.Ice };
        }
        if (newElements.Wind != previousElements.Wind)
        {
            dto = dto with { Wind = newElements.Wind };
        }
        if (newElements.Thunder != previousElements.Thunder)
        {
            dto = dto with { Thunder = newElements.Thunder };
        }
        if (newElements.Machine != previousElements.Machine)
        {
            dto = dto with { Machine = newElements.Machine };
        }
        if (newElements.Dark != previousElements.Dark)
        {
            dto = dto with { Dark = newElements.Dark };
        }

        return dto;
    }
}
