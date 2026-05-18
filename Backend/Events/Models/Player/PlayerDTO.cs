using System.Text.Json.Serialization;
using Backend.Events.DTO;

namespace Backend.Events.Models.Player;

public record class PlayerDTO
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<string> Name { get; init; } = Optional<string>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> Bits { get; init; } = Optional<int>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<string> Location { get; init; } = Optional<string>.Empty;
}
