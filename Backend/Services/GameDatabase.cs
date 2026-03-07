using System.Text.Json;
using Backend.Models.Addresses;

namespace Backend.Services
{
    public class GameDatabase
    {
        private readonly string _dataDirectory;
        private PlayerAddresses? _playerAddresses;

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
    }
}
