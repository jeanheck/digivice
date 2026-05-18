using System.Text.Json.Serialization;
using Backend.Events.DTO.Interfaces;
using Backend.Events.DTO.Party.Digimon;

namespace Backend.Events.DTO.Party;

public record class DigimonDTO : IDTO
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> Level { get; init; } = Optional<int>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int> Experience { get; init; } = Optional<int>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<VitalsDTO> Vitals { get; init; } = Optional<VitalsDTO>.Empty;

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
}
