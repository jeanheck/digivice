namespace Tests.Events.Converters;

using Backend.Domain.Models;
using Backend.Events.Converters;

public class CardInventoryConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapCardsCorrectly()
    {
        var inventory = new CardInventory
        {
            Cards = new Dictionary<int, int> { [1009] = 1, [1023] = 3 }
        };

        var dto = CardInventoryConverter.ToDTO(inventory);

        Assert.True(dto.Cards.HasValue);
        Assert.Equal(2, dto.Cards.Value!.Count);
        Assert.Equal(1, dto.Cards.Value[1009]);
        Assert.Equal(3, dto.Cards.Value[1023]);
    }
}
