using Backend.Models;
using Backend.Utils;
using Backend.Interfaces;

namespace Backend.Services
{
    public class PlayerStateService
    {
        private readonly IGameDatabase _database;
        private readonly IGameReader _reader;

        public PlayerStateService(IGameDatabase database, IGameReader reader)
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
                Bits = resource.Bits,
                MapId = resource.MapId.ToString("X4")
            };

            return player;
        }
    }
}
