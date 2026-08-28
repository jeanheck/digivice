using Backend.Domain.Models;
using Backend.Events.Converters;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO;

namespace Backend.Events.Diffing;

public static class CardBattleDiffer
{
    public static CardBattleDTO Diff(CardBattle? previousCardBattle, CardBattle newCardBattle)
    {
        if (newCardBattle.HasNoChanges(previousCardBattle))
        {
            return new CardBattleDTO();
        }

        if (previousCardBattle == null)
        {
            return CardBattleConverter.ToDTO(newCardBattle);
        }

        if (newCardBattle.OpponentId == previousCardBattle.OpponentId)
        {
            return new CardBattleDTO();
        }

        return new CardBattleDTO
        {
            OpponentId = newCardBattle.OpponentId,
        };
    }
}
