namespace Tests.Events.Diffing.Journals.Quests;

using Backend.Events.Diffing.Journals.Quests;
using Backend.Domain.Models.Journals.Quests;

public class RequisiteDifferTests
{
    [Fact]
    public void Diff_ShouldReturnNull_WhenNoChanges()
    {
        var previous = new Requisite { Id = "5", Value = 1 };
        var newObj = new Requisite { Id = "5", Value = 1 };

        var result = RequisiteDiffer.Diff(previous, newObj);

        Assert.Null(result);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var newObj = new Requisite { Id = "5", Value = 2 };

        var result = RequisiteDiffer.Diff(null, newObj);

        Assert.NotNull(result);
        Assert.Equal("5", result.Id);
        Assert.True(result.Value.HasValue);
        Assert.Equal((byte)2, result.Value.Value);
    }

    [Fact]
    public void Diff_ShouldReturnDelta_WhenValueChanged()
    {
        var previous = new Requisite { Id = "5", Value = 1 };
        var newObj = new Requisite { Id = "5", Value = 3 };

        var result = RequisiteDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.Equal("5", result.Id);
        Assert.True(result.Value.HasValue);
        Assert.Equal((byte)3, result.Value.Value);
    }
}
