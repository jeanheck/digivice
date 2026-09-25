using Backend.Memory.Addresses;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers.Interfaces
{
    public interface INpcsReader
    {
        NpcsResource Read(NpcsAddresses addresses);
    }
}
