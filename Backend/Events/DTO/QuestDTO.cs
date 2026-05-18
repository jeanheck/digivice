using System.Collections.Generic;
using System.Text.Json.Serialization;
using Backend.Events.DTO.Interfaces;

namespace Backend.Events.DTO;

public record class QuestDTO : IDTO
{
    public string Id { get; init; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<List<RequisiteDTO>> Requisites { get; init; } = Optional<List<RequisiteDTO>>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<List<StepDTO>> Steps { get; init; } = Optional<List<StepDTO>>.Empty;
}
