using System.Text.Json.Serialization;
using Backend.Events.DTO.Interfaces;
using Backend.Events.DTO.Npcs;
using Backend.Events.DTO.Shared;

namespace Backend.Events.DTO;

public record class NpcsDTO : IDTO
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<NpcDTO> Genji { get; init; } = Optional<NpcDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<NpcDTO> Natsumi { get; init; } = Optional<NpcDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<NpcDTO> Catherine { get; init; } = Optional<NpcDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<NpcDTO> Lucia { get; init; } = Optional<NpcDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<NpcDTO> Robert { get; init; } = Optional<NpcDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<NpcDTO> Akiba { get; init; } = Optional<NpcDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<NpcDTO> Chris { get; init; } = Optional<NpcDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<NpcDTO> Tomomi { get; init; } = Optional<NpcDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<NpcDTO> Mitch { get; init; } = Optional<NpcDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<NpcDTO> Bob { get; init; } = Optional<NpcDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<NpcDTO> Andy { get; init; } = Optional<NpcDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<NpcDTO> George { get; init; } = Optional<NpcDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<NpcDTO> MeiLin { get; init; } = Optional<NpcDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<NpcDTO> Jessica { get; init; } = Optional<NpcDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<NpcDTO> Gordon { get; init; } = Optional<NpcDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<NpcDTO> Alice { get; init; } = Optional<NpcDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<NpcDTO> Nakano { get; init; } = Optional<NpcDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<NpcDTO> SeiryuLeader { get; init; } = Optional<NpcDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<NpcDTO> Keith { get; init; } = Optional<NpcDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<NpcDTO> SuzakuLeader { get; init; } = Optional<NpcDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<NpcDTO> FakeByakkoLeader { get; init; } = Optional<NpcDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<NpcDTO> ByakkoLeader { get; init; } = Optional<NpcDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<NpcDTO> AoaAttacker { get; init; } = Optional<NpcDTO>.Empty;
}
