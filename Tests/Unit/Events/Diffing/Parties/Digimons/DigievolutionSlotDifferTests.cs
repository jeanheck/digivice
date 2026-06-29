namespace Tests.Events.Diffing.Parties.Digimons;

using Backend.Events.Diffing.Parties.Digimons;
using Backend.Domain.Models.Parties.Digimons;

public class DigievolutionSlotDifferTests
{
    [Fact]
    public void Diff_ShouldReturnNull_WhenNoChanges()
    {
        var previous = new DigievolutionSlot { Index = 1, DigievolutionId = 4, Digievolution = new Digievolution { Level = 5 } };
        var newObj = new DigievolutionSlot { Index = 1, DigievolutionId = 4, Digievolution = new Digievolution { Level = 5 } };

        var result = DigievolutionSlotDiffer.Diff(previous, newObj);

        Assert.Null(result);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var newObj = new DigievolutionSlot { Index = 1, DigievolutionId = 4, Digievolution = new Digievolution { Level = 5 } };

        var result = DigievolutionSlotDiffer.Diff(null, newObj);

        Assert.NotNull(result);
        Assert.Equal(1, result.Index);
        Assert.True(result.DigievolutionId.HasValue);
        Assert.Equal(4, result.DigievolutionId.Value);
        Assert.True(result.Digievolution.HasValue);
        Assert.NotNull(result.Digievolution.Value);
        Assert.True(result.Digievolution.Value.Level.HasValue);
        Assert.Equal(5, result.Digievolution.Value.Level.Value);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenDigievolutionIdChanged()
    {
        var previous = new DigievolutionSlot { Index = 1, DigievolutionId = 4, Digievolution = new Digievolution { Level = 5 } };
        var newObj = new DigievolutionSlot { Index = 1, DigievolutionId = 5, Digievolution = new Digievolution { Level = 5 } };

        var result = DigievolutionSlotDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.Equal(1, result.Index);
        Assert.True(result.DigievolutionId.HasValue);
        Assert.Equal(5, result.DigievolutionId.Value);
    }

    [Fact]
    public void Diff_ShouldReturnDelta_WhenOnlyNestedLevelChanged()
    {
        var previous = new DigievolutionSlot { Index = 1, DigievolutionId = 4, Digievolution = new Digievolution { Level = 5 } };
        var newObj = new DigievolutionSlot { Index = 1, DigievolutionId = 4, Digievolution = new Digievolution { Level = 6 } };

        var result = DigievolutionSlotDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.Equal(1, result.Index);
        Assert.False(result.DigievolutionId.HasValue);
        Assert.True(result.Digievolution.HasValue);
        Assert.NotNull(result.Digievolution.Value);
        Assert.True(result.Digievolution.Value.Level.HasValue);
        Assert.Equal(6, result.Digievolution.Value.Level.Value);
    }
}
