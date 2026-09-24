namespace Tests.Events.Diffing;

using Backend.Domain.Models;
using Backend.Events.Diffing;

public class NpcBattleDifferTests
{
    [Fact]
    public void Diff_ShouldReturnNull_WhenBattlesAreEqual()
    {
        var previousBattle = new NpcBattle { Id = "first", Won = true };
        var newBattle = new NpcBattle { Id = "first", Won = true };

        var result = NpcBattleDiffer.Diff(previousBattle, newBattle);

        Assert.Null(result);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var newBattle = new NpcBattle { Id = "first", Won = true };

        var result = NpcBattleDiffer.Diff(null, newBattle);

        Assert.NotNull(result);
        Assert.Equal("first", result.Id);
        Assert.True(result.Won.Value);
    }

    [Fact]
    public void Diff_ShouldReturnWonDelta_WhenWonChanged()
    {
        var previousBattle = new NpcBattle { Id = "first", Won = false };
        var newBattle = new NpcBattle { Id = "first", Won = true };

        var result = NpcBattleDiffer.Diff(previousBattle, newBattle);

        Assert.NotNull(result);
        Assert.Equal("first", result.Id);
        Assert.True(result.Won.Value);
    }
}
