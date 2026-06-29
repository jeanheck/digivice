using Backend.Memory.Addresses;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers
{
    public interface IAuctionReader
    {
        AuctionsResource Read(AuctionAddresses addresses);
    }
}
