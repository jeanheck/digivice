using Backend.Memory.Addresses;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers.Battles
{
    public interface IDigimonBattleReader
    {
        DigimonBattleResource Read(DigimonBattleAddresses digimonBattleAddresses, EnemyAddresses enemyAddresses);
    }
}
