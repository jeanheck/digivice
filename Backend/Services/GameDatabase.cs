using System.Text.Json;
using Backend.Models.Addresses;

namespace Backend.Services
{
    public class GameDatabase
    {
        private readonly string _dataDirectory;
        private PlayerAddresses? _playerAddresses;
        private PartyAddresses? _partyAddresses;
        private Dictionary<string, string>? _importantItemsAddresses;
        private Dictionary<string, string>? _consumableItemsAddresses;

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

        public Dictionary<string, string> GetImportantItemsAddresses()
        {
            if (_importantItemsAddresses != null) return _importantItemsAddresses;

            var path = Path.Combine(_dataDirectory, "ImportantItems.json");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Database file not found: {path}");
            }

            var json = File.ReadAllText(path);
            _importantItemsAddresses = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();

            return _importantItemsAddresses;
        }

        public Dictionary<string, string> GetConsumableItemsAddresses()
        {
            if (_consumableItemsAddresses != null) return _consumableItemsAddresses;

            var path = Path.Combine(_dataDirectory, "ConsumableItems.json");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Database file not found: {path}");
            }

            var json = File.ReadAllText(path);
            _consumableItemsAddresses = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();

            return _consumableItemsAddresses;
        }
    }
}
