using Backend.Memory.Addresses;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers
{
    public interface IPlayerReader
    {
        PlayerResource Read(PlayerAddresses addresses);
    }
}
