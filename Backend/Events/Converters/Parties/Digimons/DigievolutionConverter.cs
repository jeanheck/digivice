using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.DTO.Parties.Digimons;

namespace Backend.Events.Converters.Parties.Digimons;

public static class DigievolutionConverter
{
    public static DigievolutionDTO ToDTO(Digievolution digievolution) => new()
    {
        Level = digievolution.Level
    };
}
