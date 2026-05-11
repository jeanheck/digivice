using Backend.Models;
using Backend.Utils;
using Backend.Interfaces;

namespace Backend.Services
{
    public class PlayerStateService(IGameDatabase database, IGameReader reader)
    {
        public Player? GetPlayer()
        {
            var addresses = database.GetPlayerAddresses();
            var resource = reader.ReadPlayer(addresses);
            var playerName = TextDecoder.DecodeName(resource.NameBytes);

            if (string.IsNullOrWhiteSpace(playerName))
                return null;

            var player = new Player
            {
                Name = playerName,
                Bits = resource.Bits,
                MapId = resource.MapId?.ToString("X4")
            };

            return player;
        }
    }
}
