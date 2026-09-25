namespace Tests.Events.Diffing;

using Backend.Domain.Models;
using Backend.Events.Diffing;
using Xunit;

public class CardBattleDifferTests
{
    [Fact]
    public void Diff_ShouldReturnEmptyDTO_WhenNoChanges()
    {
        var previousCardBattle = new CardBattle { Id = 1 };
        var newCardBattle = new CardBattle { Id = 1 };

        var result = CardBattleDiffer.Diff(previousCardBattle, newCardBattle);

        Assert.False(result.Id.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousCardBattleIsNull()
    {
        var newCardBattle = new CardBattle { Id = 3 };

        var result = CardBattleDiffer.Diff(null, newCardBattle);

        Assert.True(result.Id.HasValue);
        Assert.Equal(3, result.Id.Value);
    }

    [Fact]
    public void Diff_ShouldReturnIdDelta_WhenOnlyIdChanged()
    {
        var previousCardBattle = new CardBattle { Id = 1 };
        var newCardBattle = new CardBattle { Id = 11 };

        var result = CardBattleDiffer.Diff(previousCardBattle, newCardBattle);

        Assert.True(result.Id.HasValue);
        Assert.Equal(11, result.Id.Value);
    }
}
