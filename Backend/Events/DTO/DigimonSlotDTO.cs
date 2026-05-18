using System.Text.Json.Serialization;
using Backend.Events.DTO.Interfaces;
using Backend.Events.DTO.Party;

namespace Backend.Events.DTO;

public record class DigimonSlotDTO : IDTO
{
    public int Index { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<int?> DigimonId { get; init; } = Optional<int?>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<DigimonDTO?> Digimon { get; init; } = Optional<DigimonDTO?>.Empty;
}
