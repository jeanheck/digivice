using Backend.Domain.Models;
using Backend.Application.Services;

namespace Backend.Application.Providers
{
    public class StateProvider(
        PlayerProvider playerProvider,
        PartyStateService partyStateService,
        JournalProvider journalProvider)
    {
        public State Get()
        {
            return new State
            {
                Player = playerProvider.Get(),
                Party = partyStateService.GetParty(),
                Journal = journalProvider.Get()
            };
        }
    }
}
