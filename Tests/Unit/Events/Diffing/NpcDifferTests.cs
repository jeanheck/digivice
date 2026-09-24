namespace Tests.Events.Diffing;

using Backend.Domain.Models;
using Backend.Events.Diffing;

public class NpcDifferTests
{
    private static Npc CreateNpc(bool firstWon, bool secondWon)
    {
        return new Npc
        {
            Battles =
            [
                new NpcBattle { Id = "first", Won = firstWon },
                new NpcBattle { Id = "second", Won = secondWon },
            ],
        };
    }

    [Fact]
    public void Diff_ShouldReturnNull_WhenNoChanges()
    {
        Assert.Null(NpcDiffer.Diff(CreateNpc(true, false), CreateNpc(true, false)));
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var result = NpcDiffer.Diff(null, CreateNpc(true, false));

        Assert.NotNull(result);
        var battles = result.Battles.Value!;
        Assert.Equal(2, battles.Count);
        Assert.Equal("first", battles[0].Id);
        Assert.True(battles[0].Won.Value);
        Assert.Equal("second", battles[1].Id);
        Assert.False(battles[1].Won.Value);
    }

    [Fact]
    public void Diff_ShouldReturnOnlyChangedBattle_WhenOneBattleIsWon()
    {
        var result = NpcDiffer.Diff(CreateNpc(true, false), CreateNpc(true, true));

        Assert.NotNull(result);
        var battle = Assert.Single(result.Battles.Value!);
        Assert.Equal("second", battle.Id);
        Assert.True(battle.Won.Value);
    }
}
