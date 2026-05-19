using Backend.Memory.Repositories;
using Backend.Memory.Readers.Parties.Digimons;
using Backend.Memory.Resources.Parties;

namespace Backend.Application.Loaders.Parties
{
    public class DigimonLoader(
        IAddressesRepository addressesRepository,
        IDigimonReader digimonReader)
    {
        public DigimonResource Load(int digimonId)
        {
            var digimonStatusAddresses = addressesRepository.GetDigimonStatusAddresses();
            var digimonAddress = addressesRepository.GetDigimonAddressById(digimonId)
                ?? throw new InvalidOperationException($"Address not found for Digimon ID {digimonId}");

            return digimonReader.Read(digimonAddress, digimonStatusAddresses);
        }
    }
}
