namespace Tests.Events.Diffing;

using Backend.Domain.Models;
using Backend.Events.Diffing;

public class NpcsDifferTests
{
    [Fact]
    public void Diff_ShouldReturnEmptyDTO_WhenNoChanges()
    {
        var previous = CreateBaseNpcs();
        var current = CreateBaseNpcs();

        var result = NpcsDiffer.Diff(previous, current);

        Assert.False(result.Genji.HasValue);
        Assert.False(result.Natsumi.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var current = CreateBaseNpcs();
        current.Genji.Battles[0].Value = 0x20;

        var result = NpcsDiffer.Diff(null, current);

        Assert.True(result.Genji.HasValue);
        Assert.True(result.Genji.Value!.Battles.HasValue);
        Assert.Equal(0x20, result.Genji.Value.Battles.Value![0].Value.Value);
        Assert.True(result.Natsumi.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnGenjiDelta_WhenOnlyGenjiChanged()
    {
        var previous = CreateBaseNpcs();
        var current = CreateBaseNpcs();
        current.Genji.Battles[0].Value = 0x20;

        var result = NpcsDiffer.Diff(previous, current);

        Assert.True(result.Genji.HasValue);
        Assert.True(result.Genji.Value!.Battles.HasValue);
        Assert.Equal("first", result.Genji.Value.Battles.Value![0].Id);
        Assert.Equal(0x20, result.Genji.Value.Battles.Value[0].Value.Value);
        Assert.False(result.Natsumi.HasValue);
    }

    private static Npcs CreateBaseNpcs()
    {
        return new Npcs
        {
            Genji = new Npc
            {
                Battles =
                [
                    new NpcBattle { Id = "first", Value = 0 },
                    new NpcBattle { Id = "second", Value = 0 },
                ],
            },
            Natsumi = new Npc
            {
                Battles = [new NpcBattle { Id = "first", Value = 0 }],
            },
        };
    }
}
