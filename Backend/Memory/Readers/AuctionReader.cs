using Backend.Memory.Addresses;
using Backend.Memory.Readers.Helpers;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers
{
    public class AuctionReader(IMemoryReader memoryReader) : IAuctionReader
    {
        public AuctionResource Read(KeyValuePair<string, AuctionAddresses> AuctionAddresses)
        {
            return new AuctionResource
            {
                Id = AuctionAddresses.Key,
                Value = FlagByteHelper.Read(memoryReader, AuctionAddresses.Value.Address, AuctionAddresses.Value.BitMask),
            };
        }
    }
}