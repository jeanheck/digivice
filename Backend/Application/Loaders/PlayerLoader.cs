using Backend.Memory.Repositories;
using Backend.Memory.Readers;
using Backend.Memory.Resources;

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
