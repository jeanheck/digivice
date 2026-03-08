using System.Text.Json;
using Backend.Models.Addresses;

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
        private Dictionary<string, QuestAddresses> _sideQuestsAddresses = new();

        public GameDatabase()
        {
            // Assuming the app runs from the Backend folder
            _dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data");
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

        public QuestAddresses GetSideQuestAddresses(string questName)
        {
            if (_sideQuestsAddresses.TryGetValue(questName, out var cachedAddresses))
            {
                return cachedAddresses;
            }

            var path = Path.Combine(_dataDirectory, "SideQuests", $"{questName}.json");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Database file not found: {path}");
            }

            var json = File.ReadAllText(path);
            var parsed = JsonSerializer.Deserialize<QuestAddresses>(json) ?? new QuestAddresses();

            _sideQuestsAddresses[questName] = parsed;

            return parsed;
        }
    }
}
