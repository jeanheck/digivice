using Backend.Memory.Addresses.Parties;
using Backend.Memory.Resources.Parties;

namespace Backend.Memory.Readers.Parties
{
    public interface IDigimonInCombatReader
    {
        DigimonInCombatResource Read(DigimonInCombatAddresses addresses, int zeroBasedPartySlotIndex);
    }
}
