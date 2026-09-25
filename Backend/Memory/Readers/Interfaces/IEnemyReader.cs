using Backend.Memory.Addresses;
using Backend.Memory.Resources.Battles;

namespace Backend.Memory.Readers.Interfaces
{
    public interface IEnemyReader
    {
        EnemyResource Read(EnemyAddresses addresses);
    }
}
