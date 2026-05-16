using Backend.Domain.Models;

namespace Backend.Application.Services
{
    public class StateService(
        PlayerStateService playerStateService,
        PartyStateService partyStateService,
        JournalStateService journalStateService)
    {
        public State GetState()
        {
            return new State
            {
                Player = playerStateService.GetPlayer(),
                Party = partyStateService.GetParty(),
                Journal = journalStateService.GetJournal()
            };
        }
    }
}
