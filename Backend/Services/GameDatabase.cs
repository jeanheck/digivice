using System.Text.Json;
using Backend.Models.Addresses;
using Backend.Models.Quests;

namespace Backend.Services
{
    public class GameDatabase
    {
        private readonly string _dataDirectory;
        private PlayerAddresses? _playerAddresses;
        private PartyAddresses? _partyAddresses;
        private ImportantItemsAddresses? _importantItemsAddresses;
        private ConsumableItemsAddresses? _consumableItemsAddresses;
        private DigimonAddresses? _digimonAddresses;
        private MainQuest? _mainQuest;
        private Dictionary<string, SideQuest> _sideQuests = new();

        public GameDatabase()
        {
            // Assuming the app runs from the Backend folder
            _dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Database");
        }

        public PlayerAddresses GetPlayerAddresses()
        {
            if (_playerAddresses != null) return _playerAddresses;

            var path = Path.Combine(_dataDirectory, "Player.json");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Database file not found: {path}");
            }

            var json = File.ReadAllText(path);
            _playerAddresses = JsonSerializer.Deserialize<PlayerAddresses>(json) ?? new PlayerAddresses();

            return _playerAddresses;
        }

        public PartyAddresses GetPartyAddresses()
        {
            if (_partyAddresses != null) return _partyAddresses;

            var path = Path.Combine(_dataDirectory, "Party.json");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Database file not found: {path}");
            }

            var json = File.ReadAllText(path);
            _partyAddresses = JsonSerializer.Deserialize<PartyAddresses>(json) ?? new PartyAddresses();

            return _partyAddresses;
        }

        public ImportantItemsAddresses GetImportantItemsAddresses()
        {
            if (_importantItemsAddresses != null) return _importantItemsAddresses;

            var path = Path.Combine(_dataDirectory, "ImportantItems.json");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Database file not found: {path}");
            }

            var json = File.ReadAllText(path);
            _importantItemsAddresses = JsonSerializer.Deserialize<ImportantItemsAddresses>(json) ?? new ImportantItemsAddresses();

            return _importantItemsAddresses;
        }

        public ConsumableItemsAddresses GetConsumableItemsAddresses()
        {
            if (_consumableItemsAddresses != null) return _consumableItemsAddresses;

            var path = Path.Combine(_dataDirectory, "ConsumableItems.json");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Database file not found: {path}");
            }

            var json = File.ReadAllText(path);
            _consumableItemsAddresses = JsonSerializer.Deserialize<ConsumableItemsAddresses>(json) ?? new ConsumableItemsAddresses();

            return _consumableItemsAddresses;
        }

        public DigimonAddresses GetDigimonAddresses()
        {
            if (_digimonAddresses != null) return _digimonAddresses;

            var path = Path.Combine(_dataDirectory, "Digimons.json");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Database file not found: {path}");
            }

            var json = File.ReadAllText(path);
            _digimonAddresses = JsonSerializer.Deserialize<DigimonAddresses>(json) ?? new DigimonAddresses();

            return _digimonAddresses;
        }

        public MainQuest GetMainQuest()
        {
            if (_mainQuest != null) return _mainQuest;

            var path = Path.Combine(_dataDirectory, "MainQuest.json");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Database file not found: {path}");
            }

            var json = File.ReadAllText(path);
            _mainQuest = JsonSerializer.Deserialize<MainQuest>(json) ?? new MainQuest();

            return _mainQuest;
        }

        public SideQuest GetSideQuest(string questName)
        {
            if (_sideQuests.TryGetValue(questName, out var cachedQuest))
            {
                return cachedQuest;
            }

            var path = Path.Combine(_dataDirectory, "SideQuests", $"{questName}.json");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Database file not found: {path}");
            }

            var json = File.ReadAllText(path);
            var parsed = JsonSerializer.Deserialize<SideQuest>(json) ?? new SideQuest();

            _sideQuests[questName] = parsed;

            return parsed;
        }
    }
}
