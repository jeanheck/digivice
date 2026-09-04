using System.Text.Json.Serialization;
using Backend.Events.DTO.Interfaces;
using Backend.Events.DTO.Shared;

namespace Backend.Events.DTO.Npcs;

public record class NpcDTO : IDTO
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<List<NpcBattleDTO>> Battles { get; init; } = Optional<List<NpcBattleDTO>>.Empty;
}
