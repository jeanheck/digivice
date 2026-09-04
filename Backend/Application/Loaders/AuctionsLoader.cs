using Backend.Application.Loaders.Interfaces;
using Backend.Memory.Readers;
using Backend.Memory.Repositories;
using Backend.Memory.Resources;

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
