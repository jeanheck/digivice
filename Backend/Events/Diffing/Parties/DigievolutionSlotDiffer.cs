using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO.Party.Digimon;

namespace Backend.Events.Diffing.Parties;

public static class DigievolutionSlotDiffer
{
    public static DigievolutionSlotDTO? Diff(DigievolutionSlot? previous, DigievolutionSlot current)
    {
        if (current.HasNoChanges(previous))
        {
            return null;
        }

        if (previous == null)
        {
            return DigievolutionSlotConverter.ToDTO(current);
        }

        bool idChanged = previous.DigievolutionId != current.DigievolutionId;
        var digievolutionDelta = current.Digievolution != null
            ? DigievolutionDiffer.Diff(previous.Digievolution, current.Digievolution)
            : null;

        bool hasDigievolutionChanged = previous.Digievolution != null && current.Digievolution == null;

        if (!idChanged && digievolutionDelta == null && !hasDigievolutionChanged)
        {
            return null;
        }

        var dto = new DigievolutionSlotDTO { Index = current.Index };
        if (idChanged)
        {
            dto = dto with { DigievolutionId = current.DigievolutionId };
        }
        if (digievolutionDelta != null || hasDigievolutionChanged)
        {
            dto = dto with { Digievolution = digievolutionDelta };
        }
        return dto;
    }
}
