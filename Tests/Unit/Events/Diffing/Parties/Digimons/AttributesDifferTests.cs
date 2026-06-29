namespace Tests.Events.Diffing.Parties.Digimons;

using Backend.Events.Diffing.Parties.Digimons;
using Backend.Domain.Models.Parties.Digimons;

public class AttributesDifferTests
{
    [Fact]
    public void Diff_ShouldReturnNull_WhenNoChanges()
    {
        var previous = new Attributes { Strength = 10, Defense = 10, Spirit = 10, Wisdom = 10, Speed = 10, Charisma = 10 };
        var newObj = new Attributes { Strength = 10, Defense = 10, Spirit = 10, Wisdom = 10, Speed = 10, Charisma = 10 };

        var result = AttributesDiffer.Diff(previous, newObj);

        Assert.Null(result);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var newObj = new Attributes { Strength = 10, Defense = 11, Spirit = 12, Wisdom = 13, Speed = 14, Charisma = 15 };

        var result = AttributesDiffer.Diff(null, newObj);

        Assert.NotNull(result);
        Assert.True(result.Strength.HasValue);
        Assert.Equal(10, result.Strength.Value);
        Assert.True(result.Defense.HasValue);
        Assert.Equal(11, result.Defense.Value);
        Assert.True(result.Spirit.HasValue);
        Assert.Equal(12, result.Spirit.Value);
        Assert.True(result.Wisdom.HasValue);
        Assert.Equal(13, result.Wisdom.Value);
        Assert.True(result.Speed.HasValue);
        Assert.Equal(14, result.Speed.Value);
        Assert.True(result.Charisma.HasValue);
        Assert.Equal(15, result.Charisma.Value);
    }

    [Fact]
    public void Diff_ShouldReturnOnlyChangedFields_WhenPartialChanges()
    {
        var previous = new Attributes { Strength = 10, Defense = 10, Spirit = 10, Wisdom = 10, Speed = 10, Charisma = 10 };
        var newObj = new Attributes { Strength = 10, Defense = 12, Spirit = 10, Wisdom = 10, Speed = 15, Charisma = 10 };

        var result = AttributesDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.False(result.Strength.HasValue);
        Assert.True(result.Defense.HasValue);
        Assert.Equal(12, result.Defense.Value);
        Assert.False(result.Spirit.HasValue);
        Assert.False(result.Wisdom.HasValue);
        Assert.True(result.Speed.HasValue);
        Assert.Equal(15, result.Speed.Value);
        Assert.False(result.Charisma.HasValue);
    }
}
