namespace Tests.Events.Diffing.Journals.Quests;

using Backend.Events.Diffing.Journals.Quests;
using Backend.Domain.Models.Journals.Quests;

public class StepDifferTests
{
    [Fact]
    public void Diff_ShouldReturnNull_WhenNoChanges()
    {
        var previous = new Step { Number = 1, IsDone = true, Requisites = [] };
        var newObj = new Step { Number = 1, IsDone = true, Requisites = [] };

        var result = StepDiffer.Diff(previous, newObj);

        Assert.Null(result);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var newObj = new Step { Number = 2, IsDone = true, Requisites = [new Requisite { Id = "1", IsDone = true }] };

        var result = StepDiffer.Diff(null, newObj);

        Assert.NotNull(result);
        Assert.Equal(2, result.Number);
        Assert.True(result.IsDone.HasValue);
        Assert.True(result.IsDone.Value);
        Assert.True(result.Requisites.HasValue);
        Assert.NotNull(result.Requisites.Value);
        Assert.Single(result.Requisites.Value);
        Assert.Equal("1", result.Requisites.Value[0].Id);
    }

    [Fact]
    public void Diff_ShouldReturnDelta_WhenIsDoneChanged()
    {
        var previous = new Step { Number = 1, IsDone = false, Requisites = [] };
        var newObj = new Step { Number = 1, IsDone = true, Requisites = [] };

        var result = StepDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.Equal(1, result.Number);
        Assert.True(result.IsDone.HasValue);
        Assert.True(result.IsDone.Value);
        Assert.False(result.Requisites.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnDelta_WhenNestedRequisiteChanged()
    {
        var previous = new Step { Number = 1, IsDone = false, Requisites = [new Requisite { Id = "1", IsDone = false }] };
        var newObj = new Step { Number = 1, IsDone = false, Requisites = [new Requisite { Id = "1", IsDone = true }] };

        var result = StepDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.Equal(1, result.Number);
        Assert.False(result.IsDone.HasValue);
        Assert.True(result.Requisites.HasValue);
        Assert.NotNull(result.Requisites.Value);
        Assert.Single(result.Requisites.Value);
        Assert.True(result.Requisites.Value[0].IsDone.Value);
    }

    [Fact]
    public void Diff_ShouldReturnMultipleDeltas_WhenIsDoneAndNestedRequisiteChanged()
    {
        var previous = new Step { Number = 1, IsDone = false, Requisites = [new Requisite { Id = "1", IsDone = false }] };
        var newObj = new Step { Number = 1, IsDone = true, Requisites = [new Requisite { Id = "1", IsDone = true }] };

        var result = StepDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.Equal(1, result.Number);
        Assert.True(result.IsDone.HasValue);
        Assert.True(result.IsDone.Value);
        Assert.True(result.Requisites.HasValue);
        Assert.NotNull(result.Requisites.Value);
        Assert.Single(result.Requisites.Value);
        Assert.True(result.Requisites.Value[0].IsDone.Value);
    }
}
