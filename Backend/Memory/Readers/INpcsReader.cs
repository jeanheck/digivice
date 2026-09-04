using Backend.Memory.Addresses;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers
{
    public interface INpcsReader
    {
        NpcsResource Read(NpcsAddresses addresses);
    }
}
