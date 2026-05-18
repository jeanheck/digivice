using Backend.Domain.Models;
using Backend.Events.DTO;

namespace Backend.Events.Converters;

public static class PartyConverter
{
    public static PartyDTO ToDTO(Party party)
    {
        return new PartyDTO();
    }
}
