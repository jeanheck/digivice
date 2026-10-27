namespace Tests.Events.Factory;

using Backend.Domain.Models;
using Backend.Events.DTO;
using Backend.Events.Factory;
using Backend.Events.Models;

public class CardInventoryEventFactoryTests
{
    [Fact]
    public void Create_ShouldReturnCardInventoryChangedEvent_WhenCardsChange()
    {
        var previousState = new State { CardInventory = new CardInventory() };
        var newState = new State { CardInventory = new CardInventory { Cards = new Dictionary<int, int> { [1009] = 1 } } };

        var result = CardInventoryEventFactory.Create(previousState, newState).ToList();

        var ev = Assert.Single(result);
        Assert.Equal(EventType.CardInventoryChanged, ev.Type);
        var dto = Assert.IsType<CardInventoryDTO>(ev.Payload);
        Assert.True(dto.Cards.HasValue);
        Assert.Equal(1, dto.Cards.Value![1009]);
    }

    [Fact]
    public void Create_ShouldReturnNoEvents_WhenNoChanges()
    {
        var previousState = new State { CardInventory = new CardInventory { Cards = new Dictionary<int, int> { [1009] = 1 } } };
        var newState = new State { CardInventory = new CardInventory { Cards = new Dictionary<int, int> { [1009] = 1 } } };

        var result = CardInventoryEventFactory.Create(previousState, newState);

        Assert.Empty(result);
    }
}
