using Backend.Application.Loaders.Interfaces;
using Backend.Application.Providers.Interfaces;
using Backend.Domain.Assemblers;
using Backend.Domain.Models;

namespace Backend.Application.Providers
{
    public class CardInventoryProvider(ICardInventoryLoader cardInventoryLoader) : ICardInventoryProvider
    {
        public CardInventory Get()
        {
            return CardInventoryAssembler.Assemble(cardInventoryLoader.Load());
        }
    }
}
