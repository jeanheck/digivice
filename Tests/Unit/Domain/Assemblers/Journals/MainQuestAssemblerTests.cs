namespace Tests.Domain.Assemblers.Journals;

using Backend.Domain.Assemblers.Journals;
using Backend.Memory.Resources.Journals;
using Backend.Memory.Resources.Journals.Quests;

public class MainQuestAssemblerTests
{
    private static QuestResource CreateMainQuest(params byte[] stepValues)
    {
        return new QuestResource
        {
            Id = "mainQuest",
            Requisites = [],
            Steps = [.. stepValues.Select((value, index) => new StepResource { Number = index + 1, Value = value, Requisites = [] })]
        };
    }

    [Fact]
    public void Assemble_ShouldApplyCompletionCascade()
    {
        var result = MainQuestAssembler.Assemble(CreateMainQuest(0, 0, 1, 0));

        Assert.True(result.Steps[0].IsDone);
        Assert.True(result.Steps[1].IsDone);
        Assert.True(result.Steps[2].IsDone);
        Assert.False(result.Steps[3].IsDone);
    }

    [Fact]
    public void Assemble_ShouldCascadeFromLastStep_WhenOnlyLastStepIsDone()
    {
        var result = MainQuestAssembler.Assemble(CreateMainQuest(0, 0, 0, 0x80));

        Assert.All(result.Steps, step => Assert.True(step.IsDone));
    }

    [Fact]
    public void Assemble_ShouldKeepAllStepsNotDone_WhenNoStepIsDone()
    {
        var result = MainQuestAssembler.Assemble(CreateMainQuest(0, 0, 0));

        Assert.All(result.Steps, step => Assert.False(step.IsDone));
    }

    [Fact]
    public void Assemble_ShouldKeepAllStepsDone_WhenEveryStepIsDone()
    {
        var result = MainQuestAssembler.Assemble(CreateMainQuest(1, 1, 1));

        Assert.All(result.Steps, step => Assert.True(step.IsDone));
    }

    [Fact]
    public void Assemble_ShouldHandleEmptySteps()
    {
        var result = MainQuestAssembler.Assemble(CreateMainQuest());

        Assert.Equal("mainQuest", result.Id);
        Assert.Empty(result.Steps);
    }
}
