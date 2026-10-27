namespace Tests.Events.Diffing;

using Backend.Domain.Models;
using Backend.Events.Diffing;

public class CardInventoryDifferTests
{
    [Fact]
    public void Diff_ShouldReturnEmptyDTO_WhenNoChanges()
    {
        var previous = new CardInventory { Cards = new Dictionary<int, int> { [1009] = 1 } };
        var current = new CardInventory { Cards = new Dictionary<int, int> { [1009] = 1 } };

        var result = CardInventoryDiffer.Diff(previous, current);

        Assert.False(result.Cards.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var current = new CardInventory { Cards = new Dictionary<int, int> { [1009] = 1 } };

        var result = CardInventoryDiffer.Diff(null, current);

        Assert.True(result.Cards.HasValue);
        Assert.Equal(1, result.Cards.Value![1009]);
    }

    [Fact]
    public void Diff_ShouldReturnCardsDelta_WhenCardsChanged()
    {
        var previous = new CardInventory { Cards = new Dictionary<int, int> { [1009] = 1 } };
        var current = new CardInventory { Cards = new Dictionary<int, int> { [1009] = 2 } };

        var result = CardInventoryDiffer.Diff(previous, current);

        Assert.True(result.Cards.HasValue);
        Assert.Equal(2, result.Cards.Value![1009]);
    }
}
