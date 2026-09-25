using Backend.Memory.Addresses.Parties;
using Backend.Memory.Resources.Parties;

namespace Backend.Memory.Readers.Interfaces
{
    public interface IDigimonReader
    {
        DigimonResource? Read(
            DigimonAddress digimonAddress,
            DigimonStatusAddresses digimonStatusAddresses,
            InBattleAddresses inBattleAddresses,
            int zeroBasedPartySlotIndex);
    }
}
