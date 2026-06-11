using Backend.Memory.Addresses;
using Backend.Memory.Readers.Helpers;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers
{
    public class AuctionReader(IMemoryReader memoryReader) : IAuctionReader
    {
        public AuctionsResource Read(AuctionAddresses addresses)
        {
            var auctionResources = addresses.Auctions.Select(auctionEntry => {
                return new AuctionResource
                {
                    Id = auctionEntry.Key,
                    Value = FlagByteHelper.Read(memoryReader, addresses.Address, auctionEntry.Value.BitMask),
                };
            }).ToList();

            return new AuctionsResource
            {
                Auctions = auctionResources,
            };
        }
    }
}
