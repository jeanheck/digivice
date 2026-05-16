using Backend.Domain.Models;
using Backend.Memory.Repositories;
using Backend.Memory.Readers;
using Backend.Application.Shared;

namespace Backend.Application.Services
{
    public class PlayerStateService(IAddressesRepository addressesRepository, IResourceReader resourceReader)
    {
        public Player? GetPlayer()
        {
            var addresses = addressesRepository.GetPlayerAddresses();
            var resource = resourceReader.ReadPlayer(addresses);
            var playerName = PlayerNameDecoder.Decode(resource.NameInBytes);

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