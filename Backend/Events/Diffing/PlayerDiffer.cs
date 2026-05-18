using Backend.Domain.Models;
using Backend.Events.Models;
using Backend.Events.Models.Player;

namespace Backend.Events.Diffing;

public static class PlayerDiffer
{
    public static IEnumerable<BaseEvent> Diff(Player? previousPlayer, Player? newPlayer)
    {
        if (newPlayer == null)
        {
            return [];
        }

        // 1. Otimização de Performance: early exit caso os Records sejam idênticos por valor
        if (newPlayer == previousPlayer)
        {
            return [];
        }

        if (previousPlayer == null)
        {
            return [
                new PlayerChangedEvent(new PlayerDTO
                {
                    Name = newPlayer.Name,
                    Bits = newPlayer.Bits,
                    Location = newPlayer.MapId
                })
            ];
        }

        // 2. Acumulação baseada em imutabilidade usando expressões 'with'
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

        // 3. Como newPlayer != previousPlayer, com certeza pelo menos uma propriedade mudou.
        // Retornamos diretamente o evento sem checagens extras!
        return [new PlayerChangedEvent(dto)];
    }
}
