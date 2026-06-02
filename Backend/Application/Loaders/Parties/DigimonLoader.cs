using Backend.Memory.Repositories;
using Backend.Memory.Readers.Parties.Digimons;
using Backend.Memory.Resources.Parties;

namespace Backend.Application.Loaders.Parties
{
    public class DigimonLoader(
        IAddressesRepository addressesRepository,
        IDigimonReader digimonReader)
    {
        public DigimonResource? Load(int digimonId)
        {
            var digimonAddress = addressesRepository.GetDigimonAddressById(digimonId);
            if (digimonAddress is null)
            {
                return null;
            }

            var digimonStatusAddresses = addressesRepository.GetDigimonStatusAddresses();
            return digimonReader.Read(digimonAddress, digimonStatusAddresses);
        }
    }
}
