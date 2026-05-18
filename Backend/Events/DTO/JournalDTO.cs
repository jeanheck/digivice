using System.Text.Json.Serialization;
using Backend.Domain.Models.Journals;
using Backend.Events.DTO.Interfaces;

namespace Backend.Events.DTO;

public record class JournalDTO : IDTO
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<Quest> MainQuest { get; init; } = Optional<Quest>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<List<Quest>> SideQuests { get; init; } = Optional<List<Quest>>.Empty;
}
