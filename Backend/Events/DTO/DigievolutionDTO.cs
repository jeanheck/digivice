using System.Text.Json.Serialization;
using Backend.Events.DTO.Interfaces;

namespace Backend.Events.DTO;

public record class DigievolutionDTO : IDTO
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> Level { get; init; } = Optional<int>.Empty;
}
