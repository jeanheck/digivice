using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO.Party.Digimon;

namespace Backend.Events.Diffing.Parties;

public static class DigievolutionDiffer
{
    public static DigievolutionDTO? Diff(Digievolution? previous, Digievolution current)
    {
        if (current.HasNoChanges(previous))
        {
            return null;
        }

        if (previous == null)
        {
            return DigievolutionConverter.ToDTO(current);
        }

        return new DigievolutionDTO { Level = current.Level };
    }
}
