using Backend.Memory.Readers.Battles;
using Backend.Memory.Repositories;
using Backend.Memory.Resources;

namespace Backend.Application.Loaders
{
    public class BattleLoader(IAddressesRepository addressesRepository, IEnemyReader enemyReader) : IBattleLoader
    {
        public BattleResource Load()
        {
            return new BattleResource
            {
                Enemy = enemyReader.Read(addressesRepository.GetEnemyAddresses())
            };
        }
    }
}
