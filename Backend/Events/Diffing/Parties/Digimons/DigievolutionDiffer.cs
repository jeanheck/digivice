using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties.Digimons;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO.Party.Digimon;

namespace Backend.Events.Diffing.Parties.Digimons;

public static class DigievolutionDiffer
{
    public static DigievolutionDTO? Diff(Digievolution? previousDigievolution, Digievolution newDigievolution)
    {
        if (newDigievolution.HasNoChanges(previousDigievolution))
        {
            return null;
        }

        if (previousDigievolution == null)
        {
            return DigievolutionConverter.ToDTO(newDigievolution);
        }

        return new DigievolutionDTO { Level = newDigievolution.Level };
    }
}
