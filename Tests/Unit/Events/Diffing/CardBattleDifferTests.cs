namespace Tests.Events.Diffing;

using Backend.Domain.Models;
using Backend.Events.Diffing;
using Xunit;

public class CardBattleDifferTests
{
    [Fact]
    public void Diff_ShouldReturnEmptyDTO_WhenNoChanges()
    {
        var previousCardBattle = new CardBattle { OpponentId = 1 };
        var newCardBattle = new CardBattle { OpponentId = 1 };

        var result = CardBattleDiffer.Diff(previousCardBattle, newCardBattle);

        Assert.False(result.OpponentId.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousCardBattleIsNull()
    {
        var newCardBattle = new CardBattle { OpponentId = 3 };

        var result = CardBattleDiffer.Diff(null, newCardBattle);

        Assert.True(result.OpponentId.HasValue);
        Assert.Equal(3, result.OpponentId.Value);
    }

    [Fact]
    public void Diff_ShouldReturnOpponentIdDelta_WhenOnlyOpponentIdChanged()
    {
        var previousCardBattle = new CardBattle { OpponentId = 1 };
        var newCardBattle = new CardBattle { OpponentId = 11 };

        var result = CardBattleDiffer.Diff(previousCardBattle, newCardBattle);

        Assert.True(result.OpponentId.HasValue);
        Assert.Equal(11, result.OpponentId.Value);
    }
}
