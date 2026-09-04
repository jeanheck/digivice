using Backend.Domain.Models;
using Backend.Application.Providers;

namespace Backend.Application
{
    public class StateComposer(
        IPlayerProvider playerProvider,
        IImportantItemsProvider importantItemsProvider,
        IPartyProvider partyProvider,
        IDigimonBattleProvider digimonBattleProvider,
        ICardBattleProvider cardBattleProvider,
        IJournalProvider journalProvider)
    {
        public State Compose()
        {
            return new State
            {
                Player = playerProvider.Get(),
                ImportantItems = importantItemsProvider.Get(),
                Party = partyProvider.Get(),
                DigimonBattle = digimonBattleProvider.Get(),
                CardBattle = cardBattleProvider.Get(),
                Journal = journalProvider.Get(),
            };
        }
    }
}
