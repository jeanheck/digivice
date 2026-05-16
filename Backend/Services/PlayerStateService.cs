using Backend.Domain.Models;
using Backend.Utils;
using Backend.Interfaces;

namespace Backend.Services
{
    public class PlayerStateService(IAddressesRepository addressesRepository, IResourceReader resourceReader)
    {
        public Player? GetPlayer()
        {
            var addresses = addressesRepository.GetPlayerAddresses();
            var resource = resourceReader.ReadPlayer(addresses);
            var playerName = TextDecoder.DecodeName(resource.NameInBytes);

            if (string.IsNullOrWhiteSpace(playerName))
            {
                return null;
            }

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
