using Backend.Models;

namespace Backend.Services
{
    public class GameStateService(
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
