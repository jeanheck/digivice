using Backend.Domain.Models;
using Backend.Events.Converters;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO;

namespace Backend.Events.Diffing;

public static class CardInventoryDiffer
{
    public static CardInventoryDTO Diff(CardInventory? previousCardInventory, CardInventory newCardInventory)
    {
        if (newCardInventory.HasNoChanges(previousCardInventory))
        {
            return new CardInventoryDTO();
        }

        if (previousCardInventory == null)
        {
            return CardInventoryConverter.ToDTO(newCardInventory);
        }

        return new CardInventoryDTO
        {
            Cards = new Dictionary<int, int>(newCardInventory.Cards)
        };
    }
}
