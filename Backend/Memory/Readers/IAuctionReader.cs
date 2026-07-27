using Backend.Memory.Addresses;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers
{
    public interface IAuctionReader
    {
        AuctionResource Read(KeyValuePair<string, AuctionAddresses> auction);
    }
}
