using Backend.Memory.Addresses;
using Backend.Memory.Readers.Interfaces;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers
{
    public class CardInventoryReader(IMemoryReader memoryReader) : ICardInventoryReader
    {
        public CardInventoryResource Read(CardInventoryAddresses addresses)
        {
            return new CardInventoryResource
            {
                Quantities = memoryReader.ReadBytes(addresses.Start, addresses.TotalCards)
            };
        }
    }
}
