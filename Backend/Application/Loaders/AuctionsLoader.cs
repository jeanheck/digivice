using Backend.Application.Loaders.Interfaces;
using Backend.Memory.Repositories;
using Backend.Memory.Resources;
using Backend.Memory.Readers.Interfaces;

namespace Backend.Application.Loaders
{
    public class AuctionsLoader(
        IAddressesRepository addressesRepository,
        IAuctionsReader auctionsReader) : IAuctionsLoader
    {
        public AuctionsResource Load()
        {
            return auctionsReader.Read(addressesRepository.GetAuctionsAddresses());
        }
    }
}
