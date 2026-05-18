using System.Text.Json.Serialization;
using Backend.Events.DTO.Interfaces;

namespace Backend.Events.DTO;

public record class ResistancesDTO : IDTO
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> Fire { get; init; } = Optional<int>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> Water { get; init; } = Optional<int>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> Ice { get; init; } = Optional<int>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> Wind { get; init; } = Optional<int>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> Thunder { get; init; } = Optional<int>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> Machine { get; init; } = Optional<int>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> Dark { get; init; } = Optional<int>.Empty;
}
