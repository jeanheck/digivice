using Backend.Memory.Addresses.Parties;
using Backend.Memory.Resources.Parties;

namespace Backend.Memory.Readers.Parties
{
    public interface IInCombatReader
    {
        InCombatResource Read(InCombatAddresses addresses, int zeroBasedPartySlotIndex);
    }
}
