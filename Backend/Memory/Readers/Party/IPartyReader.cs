using Backend.Memory.Addresses;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers.Party
{
    public interface IPartyReader
    {
        PartyResource Read(PartyAddresses addresses);
    }
}
