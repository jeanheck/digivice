using Backend.Interfaces;
using Backend.Models;
using Backend.Models.Digimons;
using Backend.Utils;
using Backend.Constants;
using Backend.Models.Quests;
using Backend.DetailedQuests;
using Backend.DetailedQuests.SideQuests;

namespace Backend.Services
{
    public class GameStateService
    {
        private readonly IMemoryReaderService _memoryReader;
        private readonly PlayerStateService _playerStateService;
        private readonly PartyStateService _partyStateService;
        private readonly ItemsStateService _itemStateService;
        private readonly JournalStateService _journalStateService;

        public GameStateService(IMemoryReaderService memoryReader, PlayerStateService playerStateService, PartyStateService partyStateService, ItemsStateService itemStateService, JournalStateService journalStateService)
        {
            _memoryReader = memoryReader;
            _playerStateService = playerStateService;
            _partyStateService = partyStateService;
            _itemStateService = itemStateService;
            _journalStateService = journalStateService;
        }

        public State GetState()
        {
            return new State
            {
                Player = _playerStateService.GetPlayer(),
                Party = _partyStateService.GetParty(),
                ImportantItems = _itemStateService.GetImportantItems(),
                Journal = GetJournal()
            };
        }





        private Journal GetJournal()
        {
            var journal = new Journal();

            _journalStateService.ApplyMainQuest(journal);
            _journalStateService.ApplySideQuests(journal);

            return journal;
        }








    }
}
