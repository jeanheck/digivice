using Backend.Models;
using Backend.Utils;

namespace Backend.Services
{
    public class PlayerStateService
    {
        private readonly GameDatabase _database;
        private readonly GameReader _reader;

        public PlayerStateService(GameDatabase database, GameReader reader)
        {
            _database = database;
            _reader = reader;
        }

        public Player? GetPlayer()
        {
            var addresses = _database.GetPlayerAddresses();
            var resource = _reader.ReadPlayer(addresses);

            if (resource.NameBytes == null)
                return null;

            var player = new Player
            {
                Name = TextDecoder.DecodeName(resource.NameBytes),
                Bits = resource.Bits
            };

            return player;
        }
    }
}
