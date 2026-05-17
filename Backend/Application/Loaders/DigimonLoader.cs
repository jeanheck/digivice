using Backend.Memory.Repositories;
using Backend.Memory.Readers.Party.Digimon;
using Backend.Memory.Resources.Party;

namespace Backend.Application.Loaders
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
