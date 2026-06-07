using Backend.Memory.Repositories;
using Backend.Memory.Readers;
using Backend.Memory.Resources;

namespace Backend.Application.Loaders
{
    public class AuctionLoader(IAddressesRepository addressesRepository, IAuctionReader auctionReader) : IAuctionLoader
    {
        public AuctionsResource Load()
        {
            return auctionReader.Read(addressesRepository.GetAuctionAddresses());
        }
    }
}
