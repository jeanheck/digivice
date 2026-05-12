using Backend.Models;
using Backend.Utils;
using Backend.Interfaces;

namespace Backend.Services
{
    public class PlayerStateService(IGameDatabase gameDatabase, IGameReader gameReader)
    {
        public Player? GetPlayer()
        {
            var addresses = gameDatabase.GetPlayerAddresses();
            var resource = gameReader.ReadPlayer(addresses);
            var playerName = TextDecoder.DecodeName(resource.NameInBytes);

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
