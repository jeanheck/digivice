using Backend.Memory.Addresses.Parties;

namespace Backend.Memory.Readers.Parties
{
    public interface IInCombatReader
    {
        InCombat ReadSlot(InCombatAddresses addresses, int zeroBasedPartySlotIndex);
    }
}
