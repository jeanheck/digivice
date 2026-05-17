using Backend.Domain.Models;
using Backend.Application.Providers;

namespace Backend.Application
{
    public class StateComposer(
        PlayerProvider playerProvider,
        PartyProvider partyProvider,
        JournalProvider journalProvider)
    {
        public State Compose()
        {
            return new State
            {
                Player = playerProvider.Get(),
                Party = partyProvider.Get(),
                Journal = journalProvider.Get()
            };
        }
    }
}
