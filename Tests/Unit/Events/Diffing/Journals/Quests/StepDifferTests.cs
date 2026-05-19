namespace Tests.Events.Diffing.Journals.Quests;

using Backend.Events.Diffing.Journals.Quests;
using Backend.Domain.Models.Journals.Quests;

public class StepDifferTests
{
    [Fact]
    public void Diff_ShouldReturnNull_WhenNoChanges()
    {
        var previous = new Step { Number = 1, Value = 1, Requisites = [] };
        var newObj = new Step { Number = 1, Value = 1, Requisites = [] };

        var result = StepDiffer.Diff(previous, newObj);

        Assert.Null(result);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var newObj = new Step { Number = 2, Value = 1, Requisites = [new Requisite { Id = "1", Value = 1 }] };

        var result = StepDiffer.Diff(null, newObj);

        Assert.NotNull(result);
        Assert.Equal(2, result.Number);
        Assert.True(result.Value.HasValue);
        Assert.Equal((byte)1, result.Value.Value);
        Assert.True(result.Requisites.HasValue);
        Assert.NotNull(result.Requisites.Value);
        Assert.Single(result.Requisites.Value);
        Assert.Equal("1", result.Requisites.Value[0].Id);
    }

    [Fact]
    public void Diff_ShouldReturnDelta_WhenValueChanged()
    {
        var previous = new Step { Number = 1, Value = 0, Requisites = [] };
        var newObj = new Step { Number = 1, Value = 1, Requisites = [] };

        var result = StepDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.Equal(1, result.Number);
        Assert.True(result.Value.HasValue);
        Assert.Equal((byte)1, result.Value.Value);
        Assert.False(result.Requisites.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnDelta_WhenNestedRequisiteChanged()
    {
        var previous = new Step { Number = 1, Value = 0, Requisites = [new Requisite { Id = "1", Value = 1 }] };
        var newObj = new Step { Number = 1, Value = 0, Requisites = [new Requisite { Id = "1", Value = 2 }] };

        var result = StepDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.Equal(1, result.Number);
        Assert.False(result.Value.HasValue);
        Assert.True(result.Requisites.HasValue);
        Assert.NotNull(result.Requisites.Value);
        Assert.Single(result.Requisites.Value);
        Assert.Equal((byte)2, result.Requisites.Value[0].Value.Value);
    }

    [Fact]
    public void Diff_ShouldReturnMultipleDeltas_WhenValueAndNestedRequisiteChanged()
    {
        var previous = new Step { Number = 1, Value = 0, Requisites = [new Requisite { Id = "1", Value = 1 }] };
        var newObj = new Step { Number = 1, Value = 1, Requisites = [new Requisite { Id = "1", Value = 2 }] };

        var result = StepDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.Equal(1, result.Number);
        Assert.True(result.Value.HasValue);
        Assert.Equal((byte)1, result.Value.Value);
        Assert.True(result.Requisites.HasValue);
        Assert.NotNull(result.Requisites.Value);
        Assert.Single(result.Requisites.Value);
        Assert.Equal((byte)2, result.Requisites.Value[0].Value.Value);
    }
}
