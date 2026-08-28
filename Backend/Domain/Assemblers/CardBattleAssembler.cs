using Backend.Domain.Models;
using Backend.Memory.Resources;

namespace Backend.Domain.Assemblers
{
    public static class CardBattleAssembler
    {
        public static CardBattle Assemble(CardBattleResource resource)
        {
            return new CardBattle
            {
                OpponentId = resource.OpponentId ?? 0,
            };
        }
    }
}
