namespace Tests.Events.Diffing;

using Backend.Domain.Models;
using Backend.Domain.Models.Battles;
using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Diffing;

public class DigimonBattleDifferTests
{
    [Fact]
    public void Diff_ShouldReturnEmptyDTO_WhenNoChanges()
    {
        var previous = CreateBaseDigimonBattle();
        var newDigimonBattle = CreateBaseDigimonBattle();

        var result = DigimonBattleDiffer.Diff(previous, newDigimonBattle);

        Assert.False(result.Field.HasValue);
        Assert.False(result.Enemy.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var newDigimonBattle = CreateBaseDigimonBattle();
        newDigimonBattle.Field = 0x02;

        var result = DigimonBattleDiffer.Diff(null, newDigimonBattle);

        Assert.True(result.Field.HasValue);
        Assert.Equal((byte)0x02, result.Field.Value);
        Assert.True(result.Enemy.HasValue);
        Assert.Equal(122, result.Enemy.Value!.Id.Value);
        Assert.Equal(201, result.Enemy.Value!.GroupId.Value);
    }

    [Fact]
    public void Diff_ShouldReturnFieldDelta_WhenOnlyFieldChanged()
    {
        var previous = CreateBaseDigimonBattle();
        var newDigimonBattle = CreateBaseDigimonBattle();
        newDigimonBattle.Field = 0x03;

        var result = DigimonBattleDiffer.Diff(previous, newDigimonBattle);

        Assert.True(result.Field.HasValue);
        Assert.Equal((byte)0x03, result.Field.Value);
        Assert.False(result.Enemy.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnBothDeltas_WhenFieldAndEnemyChanged()
    {
        var previous = CreateBaseDigimonBattle();
        var newDigimonBattle = CreateBaseDigimonBattle();
        newDigimonBattle.Field = 0x04;
        newDigimonBattle.Enemy.Speed = 90;

        var result = DigimonBattleDiffer.Diff(previous, newDigimonBattle);

        Assert.True(result.Field.HasValue);
        Assert.Equal((byte)0x04, result.Field.Value);
        Assert.True(result.Enemy.HasValue);
        Assert.Equal(90, result.Enemy.Value!.Speed.Value);
    }

    private static DigimonBattle CreateBaseDigimonBattle()
    {
        return new DigimonBattle
        {
            Field = 0x00,
            Enemy = new Enemy
            {
                Id = 122,
                GroupId = 201,
                Condition = 0x01,
                Strength = 0,
                Defense = 0,
                Speed = 84,
                HP = new Vital { Current = 600, Max = 672 }
            }
        };
    }
}
