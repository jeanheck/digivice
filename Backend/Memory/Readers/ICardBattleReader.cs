using Backend.Memory.Addresses;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers
{
    public interface ICardBattleReader
    {
        CardBattleResource Read(CardBattleAddresses addresses);
    }
}
