namespace Tests.Events.Diffing.Npcs;

using Backend.Domain.Models.Npcs;
using Backend.Events.Diffing.Npcs;
using Xunit;

public class NpcBattleDifferTests
{
    [Fact]
    public void Diff_ShouldReturnNull_WhenValueIsUnchanged()
    {
        var previousBattle = new NpcBattle { Id = "first", Value = 0x10 };
        var newBattle = new NpcBattle { Id = "first", Value = 0x10 };

        var result = NpcBattleDiffer.Diff(previousBattle, newBattle);

        Assert.Null(result);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var newBattle = new NpcBattle { Id = "first", Value = 0x08 };

        var result = NpcBattleDiffer.Diff(null, newBattle);

        Assert.NotNull(result);
        Assert.Equal("first", result.Id);
        Assert.True(result.Value.HasValue);
        Assert.Equal(0x08, result.Value.Value);
    }

    [Fact]
    public void Diff_ShouldReturnValueDelta_WhenBattleIsCompleted()
    {
        var previousBattle = new NpcBattle { Id = "first", Value = 0x00 };
        var newBattle = new NpcBattle { Id = "first", Value = 0x10 };

        var result = NpcBattleDiffer.Diff(previousBattle, newBattle);

        Assert.NotNull(result);
        Assert.Equal("first", result.Id);
        Assert.True(result.Value.HasValue);
        Assert.Equal(0x10, result.Value.Value);
    }
}
