using Backend.Memory.Addresses;
using Backend.Memory.Resources;
using Backend.Memory.Readers.Interfaces;

namespace Backend.Memory.Readers
{
    public class DigimonBattleReader(IMemoryReader memoryReader, IEnemyReader enemyReader) : IDigimonBattleReader
    {
        public DigimonBattleResource Read(DigimonBattleAddresses digimonBattleAddresses, EnemyAddresses enemyAddresses)
        {
            return new DigimonBattleResource
            {
                Field = memoryReader.ReadBytes(digimonBattleAddresses.Field, 1) is { Length: > 0 } fieldBytes
                    ? fieldBytes[0]
                    : (byte)0,
                Enemy = enemyReader.Read(enemyAddresses)
            };
        }
    }
}
