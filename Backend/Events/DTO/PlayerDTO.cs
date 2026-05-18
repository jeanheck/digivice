using System.Text.Json.Serialization;
using Backend.Events.DTO.Interfaces;

namespace Backend.Events.DTO;

public record class PlayerDTO : IDTO
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<string> Name { get; init; } = Optional<string>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> Bits { get; init; } = Optional<int>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<string> Location { get; init; } = Optional<string>.Empty;
}
