using System.Text.Json.Serialization;
using Backend.Events.DTO.Interfaces;
using Backend.Events.DTO.Journals;
using Backend.Events.DTO.Shared;

namespace Backend.Events.DTO;

public record class JournalDTO : IDTO
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<QuestDTO> MainQuest { get; init; } = Optional<QuestDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<List<QuestDTO>> SideQuests { get; init; } = Optional<List<QuestDTO>>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<List<QuestDTO>> LegendaryWeapons { get; init; } = Optional<List<QuestDTO>>.Empty;
}
