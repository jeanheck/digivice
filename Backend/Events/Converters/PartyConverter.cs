using Backend.Events.Converters.Parties;
using Backend.Events.DTO;

namespace Backend.Events.Converters;

public static class PartyConverter
{
    public static PartyDTO ToDTO(Backend.Domain.Models.Party party) => new()
    {
        Slots = party.Slots.Select(DigimonSlotConverter.ToDTO).ToList()
    };
}
