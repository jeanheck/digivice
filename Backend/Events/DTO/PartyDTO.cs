using System.Text.Json.Serialization;
using Backend.Events.DTO.Interfaces;
using Backend.Events.DTO.Party;

namespace Backend.Events.DTO;

public record class PartyDTO : IDTO
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<List<DigimonSlotDTO>> Slots { get; init; } = Optional<List<DigimonSlotDTO>>.Empty;
}
