using Backend.Domain.Models;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO;
using Backend.Events.DTO.Extensions;
using Backend.Events.Models;
using Backend.Events.Types;

namespace Backend.Events.Diffing;

public static class PlayerDiffer
{
    public static IEnumerable<BaseEvent> Diff(Player? previousPlayer, Player? newPlayer)
    {
        if (newPlayer.HasNoChanges(previousPlayer))
        {
            return [];
        }

        if (previousPlayer == null)
        {
            return [
                new BaseEvent(PlayerEvent.PlayerChanged, new PlayerDTO
                {
                    Name = newPlayer.Name,
                    Bits = newPlayer.Bits,
                    Location = newPlayer.MapId
                })
            ];
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

        if (dto.IsNotEmpty())
        {
            return [new BaseEvent(PlayerEvent.PlayerChanged, dto)];
        }

        return [];
    }
}
