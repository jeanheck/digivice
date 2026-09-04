using Backend.Application.Loaders.Interfaces;
using Backend.Domain.Assemblers;
using Backend.Domain.Models;

namespace Backend.Application.Providers
{
    public class CardBattleProvider(ICardBattleLoader cardBattleLoader) : ICardBattleProvider
    {
        public CardBattle Get()
        {
            return CardBattleAssembler.Assemble(cardBattleLoader.Load());
        }
    }
}
