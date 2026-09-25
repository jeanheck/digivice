using Backend.Application.Loaders.Interfaces;
using Backend.Domain.Assemblers;
using Backend.Domain.Models;
using Backend.Application.Providers.Interfaces;

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
