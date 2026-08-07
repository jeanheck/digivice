using System.Text.Json.Serialization;
using Backend.Events.DTO.Interfaces;
using Backend.Events.DTO.Parties.Digimons;
using Backend.Events.DTO.Shared;

namespace Backend.Events.DTO.Battles;

public record class EnemyDTO : IDTO
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> Id { get; init; } = Optional<int>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> Condition { get; init; } = Optional<int>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> Strength { get; init; } = Optional<int>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> Defense { get; init; } = Optional<int>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> Speed { get; init; } = Optional<int>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<VitalDTO> HP { get; init; } = Optional<VitalDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<VitalDTO> MP { get; init; } = Optional<VitalDTO>.Empty;
}
