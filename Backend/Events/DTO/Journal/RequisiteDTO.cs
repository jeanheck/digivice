using System.Text.Json.Serialization;
using Backend.Events.DTO.Interfaces;
using Backend.Events.DTO.Shared;

namespace Backend.Events.DTO.Journal;

public record class RequisiteDTO : IDTO
{
    public string Id { get; init; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<byte> Value { get; init; } = Optional<byte>.Empty;
}
