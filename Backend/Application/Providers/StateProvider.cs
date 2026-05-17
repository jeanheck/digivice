using Backend.Domain.Models;

namespace Backend.Application.Providers
{
    public class StateProvider(
        PlayerProvider playerProvider,
        PartyProvider partyProvider,
        JournalProvider journalProvider)
    {
        public State Get()
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
