using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties.Digimons;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO.Parties.Digimons;

namespace Backend.Events.Diffing.Parties.Digimons;

public static class DigievolutionDiffer
{
    public static DigievolutionDTO? Diff(Digievolution? previousDigievolution, Digievolution? newDigievolution)
    {
        if (newDigievolution.HasNoChanges(previousDigievolution))
        {
            return null;
        }

        if (previousDigievolution == null)
        {
            return DigievolutionConverter.ToDTO(newDigievolution);
        }

        var dto = new DigievolutionDTO();

        if (newDigievolution.Level != previousDigievolution.Level)
        {
            dto = dto with { Level = newDigievolution.Level };
        }

        if (newDigievolution.Dvxp != previousDigievolution.Dvxp)
        {
            dto = dto with { Dvxp = newDigievolution.Dvxp };
        }

        return dto;
    }
}
