using Backend.Domain.Models.Parties;
using Backend.Events.DTO.Party;

namespace Backend.Events.Converters.Parties;

public static class DigimonSlotConverter
{
    public static DigimonSlotDTO ToDTO(DigimonSlot slot) => new()
    {
        Index = slot.Index,
        DigimonId = slot.DigimonId,
        Digimon = slot.Digimon != null ? DigimonConverter.ToDTO(slot.Digimon) : null
    };
}
