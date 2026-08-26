namespace Tests.Events.Diffing;

using Backend.Domain.Models;
using Backend.Domain.Models.Battles;
using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Diffing;

public class BattleDifferTests
{
    [Fact]
    public void Diff_ShouldReturnEmptyDTO_WhenNoChanges()
    {
        var previous = CreateBaseBattle();
        var newBattle = CreateBaseBattle();

        var result = BattleDiffer.Diff(previous, newBattle);

        Assert.False(result.Field.HasValue);
        Assert.False(result.Enemy.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var newBattle = CreateBaseBattle();
        newBattle.Field = 0x02;

        var result = BattleDiffer.Diff(null, newBattle);

        Assert.True(result.Field.HasValue);
        Assert.Equal((byte)0x02, result.Field.Value);
        Assert.True(result.Enemy.HasValue);
        Assert.Equal(122, result.Enemy.Value!.Id.Value);
    }

    [Fact]
    public void Diff_ShouldReturnFieldDelta_WhenOnlyFieldChanged()
    {
        var previous = CreateBaseBattle();
        var newBattle = CreateBaseBattle();
        newBattle.Field = 0x03;

        var result = BattleDiffer.Diff(previous, newBattle);

        Assert.True(result.Field.HasValue);
        Assert.Equal((byte)0x03, result.Field.Value);
        Assert.False(result.Enemy.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnBothDeltas_WhenFieldAndEnemyChanged()
    {
        var previous = CreateBaseBattle();
        var newBattle = CreateBaseBattle();
        newBattle.Field = 0x04;
        newBattle.Enemy.Speed = 90;

        var result = BattleDiffer.Diff(previous, newBattle);

        Assert.True(result.Field.HasValue);
        Assert.Equal((byte)0x04, result.Field.Value);
        Assert.True(result.Enemy.HasValue);
        Assert.Equal(90, result.Enemy.Value!.Speed.Value);
    }

    private static Battle CreateBaseBattle()
    {
        return new Battle
        {
            Field = 0x00,
            Enemy = new Enemy
            {
                Id = 122,
                Condition = 0x01,
                Strength = 0,
                Defense = 0,
                Speed = 84,
                HP = new Vital { Current = 600, Max = 672 }
            }
        };
    }
}
