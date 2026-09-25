using System.Text.Json.Serialization;
using Backend.Events.DTO.Interfaces;
using Backend.Events.DTO.Parties.Digimons;
using Backend.Events.DTO.Shared;

namespace Backend.Events.DTO.Parties;

public record class DigimonDTO : IDTO
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> Level { get; init; } = Optional<int>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> TP { get; init; } = Optional<int>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> Blast { get; init; } = Optional<int>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> Experience { get; init; } = Optional<int>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<VitalDTO> HP { get; init; } = Optional<VitalDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<VitalDTO> MP { get; init; } = Optional<VitalDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<InBattleDTO> InBattle { get; init; } = Optional<InBattleDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<AttributesDTO> Attributes { get; init; } = Optional<AttributesDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<ResistancesDTO> Resistances { get; init; } = Optional<ResistancesDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<EquipmentsDTO> Equipments { get; init; } = Optional<EquipmentsDTO>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<List<DigievolutionSlotDTO>> Digievolutions { get; init; } = Optional<List<DigievolutionSlotDTO>>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> ActiveDigievolutionId { get; init; } = Optional<int>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<List<StoredDigievolutionDTO>> StoredDigievolutions { get; init; } = Optional<List<StoredDigievolutionDTO>>.Empty;
}
