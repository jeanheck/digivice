using Backend.Memory.Readers;
using Backend.Memory.Repositories;
using Backend.Memory.Resources;

namespace Backend.Application.Loaders
{
    public class AuctionLoader(IAddressesRepository addressesRepository, IAuctionReader auctionReader) : IAuctionLoader
    {
        public List<AuctionResource> LoadAuctions()
        {
            return [.. addressesRepository.GetAuctionAddresses().Select(auctionReader.Read)];
        }
    }
}
