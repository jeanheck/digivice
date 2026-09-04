using Backend.Application.Loaders.Interfaces;
using Backend.Memory.Repositories;
using Backend.Memory.Resources;
using Backend.Memory.Readers.Interfaces;

namespace Backend.Application.Loaders
{
    public class DigimonBattleLoader(
        IAddressesRepository addressesRepository,
        IDigimonBattleReader digimonBattleReader) : IDigimonBattleLoader
    {
        public DigimonBattleResource Load()
        {
            return digimonBattleReader.Read(
                addressesRepository.GetDigimonBattleAddresses(),
                addressesRepository.GetEnemyAddresses());
        }
    }
}
