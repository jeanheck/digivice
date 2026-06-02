using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties.Digimons;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO.Parties.Digimons;

namespace Backend.Events.Diffing.Parties.Digimons;

public static class StoredDigievolutionDiffer
{
    public static StoredDigievolutionDTO? Diff(
        StoredDigievolution? previousStoredDigievolution,
        StoredDigievolution newStoredDigievolution)
    {
        if (newStoredDigievolution.HasNoChanges(previousStoredDigievolution))
        {
            return null;
        }

        if (previousStoredDigievolution == null)
        {
            return StoredDigievolutionConverter.ToDTO(newStoredDigievolution);
        }

        var dto = new StoredDigievolutionDTO
        {
            DigievolutionId = newStoredDigievolution.DigievolutionId
        };

        if (newStoredDigievolution.Level != previousStoredDigievolution.Level)
        {
            dto = dto with { Level = newStoredDigievolution.Level };
        }

        return dto;
    }
}
