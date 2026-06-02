using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.DTO.Parties.Digimons;

namespace Backend.Events.Converters.Parties.Digimons;

public static class StoredDigievolutionConverter
{
    public static StoredDigievolutionDTO ToDTO(StoredDigievolution storedDigievolution) => new()
    {
        DigievolutionId = storedDigievolution.DigievolutionId,
        Level = storedDigievolution.Level
    };
}
