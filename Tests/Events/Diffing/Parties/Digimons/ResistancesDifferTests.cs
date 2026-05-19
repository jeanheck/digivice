namespace Tests.Events.Diffing.Parties.Digimons;

using Backend.Events.Diffing.Parties.Digimons;
using Backend.Domain.Models.Parties.Digimons;

public class ResistancesDifferTests
{
    [Fact]
    public void Diff_ShouldReturnNull_WhenNoChanges()
    {
        var previous = new Resistances { Fire = 1, Water = 1, Ice = 1, Wind = 1, Thunder = 1, Machine = 1, Dark = 1 };
        var newObj = new Resistances { Fire = 1, Water = 1, Ice = 1, Wind = 1, Thunder = 1, Machine = 1, Dark = 1 };

        var result = ResistancesDiffer.Diff(previous, newObj);

        Assert.Null(result);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var newObj = new Resistances { Fire = 1, Water = 2, Ice = 3, Wind = 4, Thunder = 5, Machine = 6, Dark = 7 };

        var result = ResistancesDiffer.Diff(null, newObj);

        Assert.NotNull(result);
        Assert.True(result.Fire.HasValue);
        Assert.Equal(1, result.Fire.Value);
        Assert.True(result.Water.HasValue);
        Assert.Equal(2, result.Water.Value);
        Assert.True(result.Ice.HasValue);
        Assert.Equal(3, result.Ice.Value);
        Assert.True(result.Wind.HasValue);
        Assert.Equal(4, result.Wind.Value);
        Assert.True(result.Thunder.HasValue);
        Assert.Equal(5, result.Thunder.Value);
        Assert.True(result.Machine.HasValue);
        Assert.Equal(6, result.Machine.Value);
        Assert.True(result.Dark.HasValue);
        Assert.Equal(7, result.Dark.Value);
    }

    [Fact]
    public void Diff_ShouldReturnOnlyChangedFields_WhenPartialChanges()
    {
        var previous = new Resistances { Fire = 1, Water = 1, Ice = 1, Wind = 1, Thunder = 1, Machine = 1, Dark = 1 };
        var newObj = new Resistances { Fire = 1, Water = 3, Ice = 1, Wind = 1, Thunder = 1, Machine = 5, Dark = 1 };

        var result = ResistancesDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.False(result.Fire.HasValue);
        Assert.True(result.Water.HasValue);
        Assert.Equal(3, result.Water.Value);
        Assert.False(result.Ice.HasValue);
        Assert.False(result.Wind.HasValue);
        Assert.False(result.Thunder.HasValue);
        Assert.True(result.Machine.HasValue);
        Assert.Equal(5, result.Machine.Value);
        Assert.False(result.Dark.HasValue);
    }
}
