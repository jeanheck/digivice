using Backend.Domain.Models;
using Backend.Events.DTO;

namespace Backend.Events.Converters;

public static class CardBattleConverter
{
    public static CardBattleDTO ToDTO(CardBattle cardBattle) => new()
    {
        OpponentId = cardBattle.OpponentId,
    };
}
