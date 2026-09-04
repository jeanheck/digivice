using System.Text.Json.Serialization;
using Backend.Events.DTO.Battles;
using Backend.Events.DTO.Interfaces;
using Backend.Events.DTO.Shared;

namespace Backend.Events.DTO;

public record class DigimonBattleDTO : IDTO
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<byte> Field { get; init; } = Optional<byte>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<EnemyDTO> Enemy { get; init; } = Optional<EnemyDTO>.Empty;
}
