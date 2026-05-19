using System.Text.Json.Serialization;
using Backend.Events.DTO.Interfaces;
using Backend.Events.DTO.Shared;

namespace Backend.Events.DTO.Party.Digimon;

public record class DigievolutionDTO : IDTO
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> Level { get; init; } = Optional<int>.Empty;
}
