using System.Text.Json.Serialization;
using Backend.Events.DTO.Interfaces;

namespace Backend.Events.DTO;

public record class DigievolutionSlotDTO : IDTO
{
    public int Index { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> DigievolutionId { get; init; } = Optional<int>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<DigievolutionDTO?> Digievolution { get; init; } = Optional<DigievolutionDTO?>.Empty;
}
