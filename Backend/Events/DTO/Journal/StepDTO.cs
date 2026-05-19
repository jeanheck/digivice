using System.Text.Json.Serialization;
using Backend.Events.DTO.Interfaces;
using Backend.Events.DTO.Shared;

namespace Backend.Events.DTO.Journal;

public record class StepDTO : IDTO
{
    public int Number { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<byte> Value { get; init; } = Optional<byte>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<List<RequisiteDTO>> Requisites { get; init; } = Optional<List<RequisiteDTO>>.Empty;
}
