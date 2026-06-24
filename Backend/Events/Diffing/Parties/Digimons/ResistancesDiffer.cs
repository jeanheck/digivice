using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties.Digimons;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO.Parties.Digimons;

namespace Backend.Events.Diffing.Parties.Digimons;

public static class ResistancesDiffer
{
    public static ResistancesDTO? Diff(Resistances? previousResistances, Resistances newResistances)
    {
        if (newResistances.HasNoChanges(previousResistances))
        {
            return null;
        }

        if (previousResistances == null)
        {
            return ResistancesConverter.ToDTO(newResistances);
        }

        var dto = new ResistancesDTO();

        if (newResistances.Fire != previousResistances.Fire)
        {
            dto = dto with { Fire = newResistances.Fire };
        }
        if (newResistances.Water != previousResistances.Water)
        {
            dto = dto with { Water = newResistances.Water };
        }
        if (newResistances.Ice != previousResistances.Ice)
        {
            dto = dto with { Ice = newResistances.Ice };
        }
        if (newResistances.Wind != previousResistances.Wind)
        {
            dto = dto with { Wind = newResistances.Wind };
        }
        if (newResistances.Thunder != previousResistances.Thunder)
        {
            dto = dto with { Thunder = newResistances.Thunder };
        }
        if (newResistances.Machine != previousResistances.Machine)
        {
            dto = dto with { Machine = newResistances.Machine };
        }
        if (newResistances.Dark != previousResistances.Dark)
        {
            dto = dto with { Dark = newResistances.Dark };
        }

        return dto;
    }
}
