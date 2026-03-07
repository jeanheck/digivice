using System.Text.Json;
using Backend.Models.Addresses;

namespace Backend.Services
{
    public class GameDatabase
    {
        private readonly string _dataDirectory;
        private PlayerAddresses? _playerAddresses;
        private PartyAddresses? _partyAddresses;

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
    }
}
