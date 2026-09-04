using Backend.Memory.Addresses;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers.Interfaces
{
    public interface IAuctionsReader
    {
        AuctionsResource Read(AuctionsAddresses addresses);
    }
}
