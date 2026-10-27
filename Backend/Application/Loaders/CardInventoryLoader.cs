using Backend.Application.Loaders.Interfaces;
using Backend.Memory.Readers.Interfaces;
using Backend.Memory.Repositories;
using Backend.Memory.Resources;

namespace Backend.Application.Loaders
{
    public class CardInventoryLoader(
        IAddressesRepository addressesRepository,
        ICardInventoryReader cardInventoryReader) : ICardInventoryLoader
    {
        public CardInventoryResource Load()
        {
            return cardInventoryReader.Read(addressesRepository.GetCardInventoryAddresses());
        }
    }
}
