namespace Tests.Events.Diffing.Parties.Digimons;

using Backend.Events.Diffing.Parties.Digimons;
using Backend.Domain.Models.Parties.Digimons;

public class DigievolutionDifferTests
{
    [Fact]
    public void Diff_ShouldReturnNull_WhenNoChanges()
    {
        var previous = new Digievolution { Level = 15, Dvxp = 100 };
        var newObj = new Digievolution { Level = 15, Dvxp = 100 };

        var result = DigievolutionDiffer.Diff(previous, newObj);

        Assert.Null(result);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var newObj = new Digievolution { Level = 25, Dvxp = 300 };

        var result = DigievolutionDiffer.Diff(null, newObj);

        Assert.NotNull(result);
        Assert.True(result.Level.HasValue);
        Assert.Equal(25, result.Level.Value);
        Assert.True(result.Dvxp.HasValue);
        Assert.Equal(300, result.Dvxp.Value);
    }

    [Fact]
    public void Diff_ShouldReturnChangedLevel_WhenLevelChanged()
    {
        var previous = new Digievolution { Level = 15, Dvxp = 100 };
        var newObj = new Digievolution { Level = 16, Dvxp = 100 };

        var result = DigievolutionDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.True(result.Level.HasValue);
        Assert.Equal(16, result.Level.Value);
        Assert.False(result.Dvxp.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnChangedDvxp_WhenOnlyDvxpChanged()
    {
        var previous = new Digievolution { Level = 15, Dvxp = 100 };
        var newObj = new Digievolution { Level = 15, Dvxp = 144 };

        var result = DigievolutionDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.False(result.Level.HasValue);
        Assert.True(result.Dvxp.HasValue);
        Assert.Equal(144, result.Dvxp.Value);
    }
}
