using Backend.Memory.Addresses.Digimon;
using Backend.Memory.Repositories;

namespace Backend.Application.Resolvers
{
    public class DigimonAddressResolver(IAddressesRepository addressesRepository)
    {
        public DigimonAddress? Resolve(int digimonId)
        {
            return addressesRepository.GetDigimonAddressById(digimonId);
        }
    }
}
