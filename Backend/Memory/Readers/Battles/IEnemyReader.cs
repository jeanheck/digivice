using Backend.Memory.Addresses;
using Backend.Memory.Resources.Battles;

namespace Backend.Memory.Readers.Battles
{
    public interface IEnemyReader
    {
        EnemyResource Read(EnemyAddresses addresses);
    }
}
