using Backend.Memory.Addresses;
using Backend.Memory.Resources;
using Backend.Memory.Readers.Interfaces;

namespace Backend.Memory.Readers
{
    public class CardBattleReader(IMemoryReader memoryReader) : ICardBattleReader
    {
        public CardBattleResource Read(CardBattleAddresses addresses)
        {
            return new CardBattleResource
            {
                OpponentId = memoryReader.ReadInt32(addresses.OpponentId),
            };
        }
    }
}
