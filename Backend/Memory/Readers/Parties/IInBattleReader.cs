using Backend.Memory.Addresses.Parties;
using Backend.Memory.Resources.Parties;

namespace Backend.Memory.Readers.Parties
{
    public interface IInBattleReader
    {
        InBattleResource Read(InBattleAddresses addresses, int zeroBasedPartySlotIndex);
    }
}
