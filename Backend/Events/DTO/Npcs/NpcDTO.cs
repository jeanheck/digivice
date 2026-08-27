using System.Text.Json.Serialization;
using Backend.Events.DTO.Interfaces;
using Backend.Events.DTO.Shared;

namespace Backend.Events.DTO.Npcs;

public record class NpcDTO : IDTO
{
    public string Id { get; init; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<List<NpcBattleDTO>> DigimonBattles { get; init; } = Optional<List<NpcBattleDTO>>.Empty;
}
