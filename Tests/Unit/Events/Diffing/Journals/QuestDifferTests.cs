namespace Tests.Events.Diffing.Journals;

using Backend.Events.Diffing.Journals;
using Backend.Domain.Models.Journals;
using Backend.Domain.Models.Journals.Quests;

public class QuestDifferTests
{
    [Fact]
    public void Diff_ShouldReturnNull_WhenNoChanges()
    {
        var previous = new Quest { Id = "12", Requisites = [], Steps = [] };
        var newObj = new Quest { Id = "12", Requisites = [], Steps = [] };

        var result = QuestDiffer.Diff(previous, newObj);

        Assert.Null(result);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var newObj = new Quest { Id = "12", Requisites = [], Steps = [new Step { Number = 0, IsDone = true, Requisites = [] }] };

        var result = QuestDiffer.Diff(null, newObj);

        Assert.NotNull(result);
        Assert.Equal("12", result.Id);
        Assert.True(result.Steps.HasValue);
        Assert.NotNull(result.Steps.Value);
        Assert.Single(result.Steps.Value);
    }

    [Fact]
    public void Diff_ShouldReturnDelta_WhenStepChanged()
    {
        var previous = new Quest { Id = "12", Requisites = [], Steps = [new Step { Number = 0, IsDone = false, Requisites = [] }] };
        var newObj = new Quest { Id = "12", Requisites = [], Steps = [new Step { Number = 0, IsDone = true, Requisites = [] }] };

        var result = QuestDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.Equal("12", result.Id);
        Assert.True(result.Steps.HasValue);
        Assert.NotNull(result.Steps.Value);
        Assert.Single(result.Steps.Value);
        Assert.True(result.Steps.Value[0].IsDone.HasValue);
        Assert.True(result.Steps.Value[0].IsDone.Value);
    }

    [Fact]
    public void Diff_ShouldReturnDelta_WhenRequisiteChanged()
    {
        var previous = new Quest { Id = "12", Requisites = [new Requisite { Id = "1", IsDone = false }], Steps = [] };
        var newObj = new Quest { Id = "12", Requisites = [new Requisite { Id = "1", IsDone = true }], Steps = [] };

        var result = QuestDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.Equal("12", result.Id);
        Assert.True(result.Requisites.HasValue);
        Assert.NotNull(result.Requisites.Value);
        Assert.Single(result.Requisites.Value);
        Assert.True(result.Requisites.Value[0].IsDone.Value);
        Assert.False(result.Steps.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnMultipleDeltas_WhenRequisiteAndStepChanged()
    {
        var previous = new Quest
        {
            Id = "12",
            Requisites = [new Requisite { Id = "1", IsDone = false }],
            Steps = [new Step { Number = 0, IsDone = false, Requisites = [] }]
        };
        var newObj = new Quest
        {
            Id = "12",
            Requisites = [new Requisite { Id = "1", IsDone = true }],
            Steps = [new Step { Number = 0, IsDone = true, Requisites = [] }]
        };

        var result = QuestDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.Equal("12", result.Id);
        Assert.True(result.Requisites.HasValue);
        Assert.NotNull(result.Requisites.Value);
        Assert.Single(result.Requisites.Value);
        Assert.True(result.Requisites.Value[0].IsDone.Value);
        Assert.True(result.Steps.HasValue);
        Assert.NotNull(result.Steps.Value);
        Assert.Single(result.Steps.Value);
        Assert.True(result.Steps.Value[0].IsDone.Value);
    }
}
