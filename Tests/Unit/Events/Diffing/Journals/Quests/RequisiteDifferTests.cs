namespace Tests.Events.Diffing.Journals.Quests;

using Backend.Events.Diffing.Journals.Quests;
using Backend.Domain.Models.Journals.Quests;

public class RequisiteDifferTests
{
    [Fact]
    public void Diff_ShouldReturnNull_WhenNoChanges()
    {
        var previous = new Requisite { Id = "5", IsDone = true };
        var newObj = new Requisite { Id = "5", IsDone = true };

        var result = RequisiteDiffer.Diff(previous, newObj);

        Assert.Null(result);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var newObj = new Requisite { Id = "5", IsDone = true };

        var result = RequisiteDiffer.Diff(null, newObj);

        Assert.NotNull(result);
        Assert.Equal("5", result.Id);
        Assert.True(result.IsDone.HasValue);
        Assert.True(result.IsDone.Value);
    }

    [Fact]
    public void Diff_ShouldReturnDelta_WhenIsDoneChanged()
    {
        var previous = new Requisite { Id = "5", IsDone = false };
        var newObj = new Requisite { Id = "5", IsDone = true };

        var result = RequisiteDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.Equal("5", result.Id);
        Assert.True(result.IsDone.HasValue);
        Assert.True(result.IsDone.Value);
    }
}
