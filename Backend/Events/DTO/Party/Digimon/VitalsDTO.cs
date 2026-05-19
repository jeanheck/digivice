using System.Text.Json.Serialization;
using Backend.Events.DTO.Interfaces;
using Backend.Events.DTO.Shared;

namespace Backend.Events.DTO.Party.Digimon;

public record class VitalsDTO : IDTO
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> MaxHP { get; init; } = Optional<int>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> MaxMP { get; init; } = Optional<int>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> CurrentHP { get; init; } = Optional<int>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> CurrentMP { get; init; } = Optional<int>.Empty;
}
