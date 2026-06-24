namespace Tests.Events.Diffing.Parties.Digimons;

using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Diffing.Parties.Digimons;

public class StoredDigievolutionDifferTests
{
    [Fact]
    public void Diff_ShouldReturnNull_WhenNoChanges()
    {
        var previous = new StoredDigievolution { DigievolutionId = 386, Level = 14 };
        var newObj = new StoredDigievolution { DigievolutionId = 386, Level = 14 };

        var result = StoredDigievolutionDiffer.Diff(previous, newObj);

        Assert.Null(result);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var newObj = new StoredDigievolution { DigievolutionId = 260, Level = 1 };

        var result = StoredDigievolutionDiffer.Diff(null, newObj);

        Assert.NotNull(result);
        Assert.True(result.DigievolutionId.HasValue);
        Assert.Equal(260, result.DigievolutionId.Value);
        Assert.True(result.Level.HasValue);
        Assert.Equal(1, result.Level.Value);
    }

    [Fact]
    public void Diff_ShouldReturnChangedLevelWithDigievolutionId_WhenLevelChanged()
    {
        var previous = new StoredDigievolution { DigievolutionId = 386, Level = 14 };
        var newObj = new StoredDigievolution { DigievolutionId = 386, Level = 15 };

        var result = StoredDigievolutionDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.True(result.DigievolutionId.HasValue);
        Assert.Equal(386, result.DigievolutionId.Value);
        Assert.True(result.Level.HasValue);
        Assert.Equal(15, result.Level.Value);
    }
}
