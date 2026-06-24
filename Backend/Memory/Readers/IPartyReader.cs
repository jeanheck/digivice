using Backend.Memory.Addresses;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers
{
    public interface IPartyReader
    {
        PartyResource Read(PartyAddresses addresses);
    }
}
