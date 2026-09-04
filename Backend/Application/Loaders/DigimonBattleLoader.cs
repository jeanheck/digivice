using Backend.Memory.Readers.Battles;
using Backend.Memory.Repositories;
using Backend.Memory.Resources;

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
