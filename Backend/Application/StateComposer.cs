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
        IAuctionsProvider auctionsProvider,
        INpcsProvider npcsProvider,
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
                Auctions = auctionsProvider.Get(),
                Npcs = npcsProvider.Get(),
                Journal = journalProvider.Get(),
            };
        }
    }
}
