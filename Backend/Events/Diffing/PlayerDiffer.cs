using Backend.Domain.Models;
using Backend.Events.Models;
using Backend.Events.DTO;
using Backend.Events.Types;

namespace Backend.Events.Diffing;

public static class PlayerDiffer
{
    public static IEnumerable<BaseEvent> Diff(Player? previousPlayer, Player? newPlayer)
    {
        if (newPlayer == previousPlayer || newPlayer == null)
        {
            return [];
        }

        if (previousPlayer == null)
        {
            return [
                new BaseEvent(PlayerEvent.Changed, new PlayerDTO
                {
                    Name = newPlayer.Name,
                    Bits = newPlayer.Bits,
                    Location = newPlayer.MapId
                })
            ];
        }

        var dto = new PlayerDTO();

        if (newPlayer.Name != previousPlayer.Name)
        {
            dto = dto with { Name = newPlayer.Name };
        }
        if (newPlayer.Bits != previousPlayer.Bits)
        {
            dto = dto with { Bits = newPlayer.Bits };
        }
        if (newPlayer.MapId != previousPlayer.MapId)
        {
            dto = dto with { Location = newPlayer.MapId };
        }

        return [new BaseEvent(PlayerEvent.Changed, dto)];
    }
}
