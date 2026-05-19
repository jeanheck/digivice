using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.DTO.Party.Digimon;

namespace Backend.Events.Converters.Parties;

public static class DigievolutionConverter
{
    public static DigievolutionDTO ToDTO(Digievolution digievolution) => new()
    {
        Level = digievolution.Level
    };
}
