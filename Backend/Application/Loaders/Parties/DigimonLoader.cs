using Backend.Memory.Readers.Parties.Digimons;
using Backend.Memory.Repositories;
using Backend.Memory.Resources.Parties;

namespace Backend.Application.Loaders.Parties
{
    public class DigimonLoader(
        IAddressesRepository addressesRepository,
        IDigimonReader digimonReader) : IDigimonLoader
    {
        public DigimonResource? Load(int digimonId, int zeroBasedPartySlotIndex)
        {
            var digimonAddress = addressesRepository.GetDigimonAddressById(digimonId);
            if (digimonAddress is null)
            {
                return null;
            }

            var digimonStatusAddresses = addressesRepository.GetDigimonStatusAddresses();
            var digimonInCombatAddresses = addressesRepository.GetDigimonInCombatAddresses();
            return digimonReader.Read(
                digimonAddress,
                digimonStatusAddresses,
                digimonInCombatAddresses,
                zeroBasedPartySlotIndex);
        }
    }
}
