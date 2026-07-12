using Backend.Domain.Models;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO;

namespace Backend.Events.Diffing;

public static class PlayerDiffer
{
    public static PlayerDTO Diff(Player? previousPlayer, Player newPlayer)
    {
        if (newPlayer.HasNoChanges(previousPlayer))
        {
            return new PlayerDTO();
        }

        if (previousPlayer == null)
        {
            return Converters.PlayerConverter.ToDTO(newPlayer);
        }

        var dto = new PlayerDTO();
        if (newPlayer.Bits != previousPlayer.Bits)
        {
            dto = dto with { Bits = newPlayer.Bits };
        }
        if (newPlayer.MapId != previousPlayer.MapId)
        {
            dto = dto with { Location = newPlayer.MapId };
        }
        if (newPlayer.SeabedRoute != previousPlayer.SeabedRoute)
        {
            dto = dto with { SeabedRoute = newPlayer.SeabedRoute };
        }
        if (newPlayer.SeabedRouteType != previousPlayer.SeabedRouteType)
        {
            dto = dto with { SeabedRouteType = newPlayer.SeabedRouteType };
        }

        return dto;
    }
}
