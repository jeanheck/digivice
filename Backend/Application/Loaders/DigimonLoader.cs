using Backend.Application.Loaders.Interfaces;
using Backend.Memory.Repositories;
using Backend.Memory.Resources.Parties;
using Backend.Memory.Readers.Interfaces;

namespace Backend.Application.Loaders
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
            var inBattleAddresses = addressesRepository.GetInBattleAddresses();
            return digimonReader.Read(
                digimonAddress,
                digimonStatusAddresses,
                inBattleAddresses,
                zeroBasedPartySlotIndex);
        }
    }
}
