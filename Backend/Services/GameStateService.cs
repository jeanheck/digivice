using Backend.Models;

namespace Backend.Services
{
    public class GameStateService(
        PlayerStateService playerStateService,
        PartyStateService partyStateService,
        ItemsStateService itemStateService,
        JournalStateService journalStateService)
    {
        public State GetState()
        {
            return new State
            {
                Player = playerStateService.GetPlayer(),
                Party = partyStateService.GetParty(),
                ImportantItems = itemStateService.GetImportantItems(),
                Journal = journalStateService.GetJournal()
            };
        }
    }
}
