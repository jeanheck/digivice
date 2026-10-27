using Backend.Domain.Models;
using Backend.Events.DTO;

namespace Backend.Events.Converters;

public static class CardInventoryConverter
{
    public static CardInventoryDTO ToDTO(CardInventory cardInventory) => new()
    {
        Cards = new Dictionary<int, int>(cardInventory.Cards)
    };
}
