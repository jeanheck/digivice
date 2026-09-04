using Backend.Application.Loaders.Interfaces;
using Backend.Memory.Repositories;
using Backend.Memory.Resources;
using Backend.Memory.Readers.Interfaces;

namespace Backend.Application.Loaders
{
    public class PlayerLoader(IAddressesRepository addressesRepository, IPlayerReader playerReader) : IPlayerLoader
    {
        public PlayerResource Load()
        {
            return playerReader.Read(addressesRepository.GetPlayerAddresses());
        }
    }
}
