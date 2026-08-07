using Backend.Domain.Models;
using Backend.Application.Providers;

namespace Backend.Application
{
    public class StateComposer(
        IPlayerProvider playerProvider,
        IPartyProvider partyProvider,
        IBattleProvider battleProvider,
        IJournalProvider journalProvider)
    {
        public State Compose()
        {
            return new State
            {
                Player = playerProvider.Get(),
                Party = partyProvider.Get(),
                Battle = battleProvider.Get(),
                Journal = journalProvider.Get(),
            };
        }
    }
}
