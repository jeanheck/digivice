using Backend.Application.Loaders.Interfaces;
using Backend.Memory.Readers.Parties.Digimons;
using Backend.Memory.Repositories;
using Backend.Memory.Resources.Parties;

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
