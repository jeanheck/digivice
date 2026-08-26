using Backend.Memory.Readers;
using Backend.Memory.Readers.Battles;
using Backend.Memory.Repositories;
using Backend.Memory.Resources;

namespace Backend.Application.Loaders
{
    public class BattleLoader(
        IAddressesRepository addressesRepository,
        IEnemyReader enemyReader,
        IMemoryReader memoryReader) : IBattleLoader
    {
        public BattleResource Load()
        {
            var addresses = addressesRepository.GetEnemyAddresses();

            return new BattleResource
            {
                Field = memoryReader.ReadBytes(addresses.Field, 1) is { Length: > 0 } fieldBytes
                    ? fieldBytes[0]
                    : (byte)0,
                Enemy = enemyReader.Read(addresses)
            };
        }
    }
}
