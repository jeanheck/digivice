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
        var newObj = new Quest { Id = "12", Requisites = [], Steps = [new Step { Number = 0, Value = 1, Requisites = [] }] };

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
        var previous = new Quest { Id = "12", Requisites = [], Steps = [new Step { Number = 0, Value = 0, Requisites = [] }] };
        var newObj = new Quest { Id = "12", Requisites = [], Steps = [new Step { Number = 0, Value = 1, Requisites = [] }] };

        var result = QuestDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.Equal("12", result.Id);
        Assert.True(result.Steps.HasValue);
        Assert.NotNull(result.Steps.Value);
        Assert.Single(result.Steps.Value);
        Assert.True(result.Steps.Value[0].Value.HasValue);
        Assert.Equal((byte)1, result.Steps.Value[0].Value.Value);
    }

    [Fact]
    public void Diff_ShouldReturnDelta_WhenRequisiteChanged()
    {
        var previous = new Quest { Id = "12", Requisites = [new Requisite { Id = "1", Value = 1 }], Steps = [] };
        var newObj = new Quest { Id = "12", Requisites = [new Requisite { Id = "1", Value = 2 }], Steps = [] };

        var result = QuestDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.Equal("12", result.Id);
        Assert.True(result.Requisites.HasValue);
        Assert.NotNull(result.Requisites.Value);
        Assert.Single(result.Requisites.Value);
        Assert.Equal((byte)2, result.Requisites.Value[0].Value.Value);
        Assert.False(result.Steps.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnMultipleDeltas_WhenRequisiteAndStepChanged()
    {
        var previous = new Quest 
        { 
            Id = "12", 
            Requisites = [new Requisite { Id = "1", Value = 1 }], 
            Steps = [new Step { Number = 0, Value = 0, Requisites = [] }] 
        };
        var newObj = new Quest 
        { 
            Id = "12", 
            Requisites = [new Requisite { Id = "1", Value = 2 }], 
            Steps = [new Step { Number = 0, Value = 1, Requisites = [] }] 
        };

        var result = QuestDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.Equal("12", result.Id);
        Assert.True(result.Requisites.HasValue);
        Assert.NotNull(result.Requisites.Value);
        Assert.Single(result.Requisites.Value);
        Assert.Equal((byte)2, result.Requisites.Value[0].Value.Value);
        Assert.True(result.Steps.HasValue);
        Assert.NotNull(result.Steps.Value);
        Assert.Single(result.Steps.Value);
        Assert.Equal((byte)1, result.Steps.Value[0].Value.Value);
    }
}
