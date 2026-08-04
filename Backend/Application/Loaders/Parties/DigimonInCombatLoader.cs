using Backend.Memory.Readers.Parties;
using Backend.Memory.Repositories;
using Backend.Memory.Resources.Parties;

namespace Backend.Application.Loaders.Parties
{
    public class DigimonInCombatLoader(
        IAddressesRepository addressesRepository,
        IDigimonInCombatReader digimonInCombatReader) : IDigimonInCombatLoader
    {
        public DigimonInCombatResource Load(int zeroBasedPartySlotIndex)
        {
            var digimonInCombatAddresses = addressesRepository.GetDigimonInCombatAddresses();
            return digimonInCombatReader.Read(digimonInCombatAddresses, zeroBasedPartySlotIndex);
        }
    }
}
