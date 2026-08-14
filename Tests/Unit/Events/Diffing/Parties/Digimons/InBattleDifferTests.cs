namespace Tests.Events.Diffing.Parties.Digimons;

using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Diffing.Parties.Digimons;

public class InBattleDifferTests
{
    [Fact]
    public void Diff_ShouldReturnNull_WhenNoChanges()
    {
        var previous = CreateBaseInBattle();
        var newObj = CreateBaseInBattle();

        var result = InBattleDiffer.Diff(previous, newObj);

        Assert.Null(result);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var newObj = CreateBaseInBattle();
        newObj.Condition = 0x04;
        newObj.Speed = 84;

        var result = InBattleDiffer.Diff(null, newObj);

        Assert.NotNull(result);
        Assert.Equal(0x04, result.Condition.Value);
        Assert.Equal(0, result.Strength.Value);
        Assert.Equal(0, result.Defense.Value);
        Assert.Equal(84, result.Speed.Value);
        Assert.Equal(100, result.HP.Value!.Current.Value);
    }

    [Fact]
    public void Diff_ShouldReturnOnlyChangedFields_WhenPartialChanges()
    {
        var previous = CreateBaseInBattle();
        var newObj = CreateBaseInBattle();
        newObj.Condition = 0x01;
        newObj.HP.Current = 50;

        var result = InBattleDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.Equal(0x01, result.Condition.Value);
        Assert.Equal(50, result.HP.Value!.Current.Value);
        Assert.False(result.HP.Value.Max.HasValue);
        Assert.False(result.MP.HasValue);
        Assert.False(result.Strength.HasValue);
        Assert.False(result.Defense.HasValue);
        Assert.False(result.Speed.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnOnlySpeed_WhenOnlySpeedChanges()
    {
        var previous = CreateBaseInBattle();
        var newObj = CreateBaseInBattle();
        newObj.Speed = 84;

        var result = InBattleDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.Equal(84, result.Speed.Value);
        Assert.False(result.Condition.HasValue);
        Assert.False(result.Strength.HasValue);
        Assert.False(result.Defense.HasValue);
        Assert.False(result.HP.HasValue);
        Assert.False(result.MP.HasValue);
    }

    private static InBattle CreateBaseInBattle()
    {
        return new InBattle
        {
            Condition = 0,
            Strength = 0,
            Defense = 0,
            Speed = 0,
            HP = new Vital { Current = 100, Max = 100 },
            MP = new Vital { Current = 50, Max = 50 }
        };
    }
}
