using Backend.Domain.Models;
using Backend.Events.DTO;

namespace Backend.Events.Converters;

public static class PlayerConverter
{
    public static PlayerDTO ToDTO(Player player)
    {
        return new PlayerDTO
        {
            Bits = player.Bits,
            Location = player.MapId,
            PreviousMapId = player.PreviousMapId,
            SeabedRoute = player.SeabedRoute,
            MapVariant = player.MapVariant
        };
    }
}
