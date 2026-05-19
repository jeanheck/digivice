using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO.Party.Digimon;

namespace Backend.Events.Diffing.Parties.Digimons;

public static class DigievolutionSlotDiffer
{
    public static DigievolutionSlotDTO? Diff(
        DigievolutionSlot? previousDigievolutionSlot,
        DigievolutionSlot newDigievolutionSlot)
    {
        if (newDigievolutionSlot.HasNoChanges(previousDigievolutionSlot))
        {
            return null;
        }

        if (previousDigievolutionSlot == null)
        {
            return DigievolutionSlotConverter.ToDTO(newDigievolutionSlot);
        }

        if (previousDigievolutionSlot.DigievolutionId != newDigievolutionSlot.DigievolutionId)
        {
            return DigievolutionSlotConverter.ToDTO(newDigievolutionSlot);
        }

        var digievolutionDelta = DigievolutionDiffer.Diff(
            previousDigievolutionSlot.Digievolution!,
            newDigievolutionSlot.Digievolution!);

        if (digievolutionDelta != null)
        {
            return new DigievolutionSlotDTO
            {
                Index = newDigievolutionSlot.Index,
                Digievolution = digievolutionDelta
            };
        }

        return null;
    }
}
