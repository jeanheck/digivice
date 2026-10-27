using Backend.Domain.Models;
using Backend.Memory.Resources;

namespace Backend.Domain.Assemblers
{
    public static class CardInventoryAssembler
    {
        private const int FirstCardId = 1000;

        public static CardInventory Assemble(CardInventoryResource resource)
        {
            var cards = new Dictionary<int, int>();

            for (int i = 0; i < resource.Quantities.Length; i++)
            {
                byte quantity = resource.Quantities[i];
                if (quantity > 0)
                {
                    cards[FirstCardId + i] = quantity;
                }
            }

            return new CardInventory
            {
                Cards = cards
            };
        }
    }
}
