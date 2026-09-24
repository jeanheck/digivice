namespace Tests.Domain.Assemblers.Journals;

using Backend.Domain.Assemblers.Journals;
using Backend.Memory.Resources.Journals;
using Backend.Memory.Resources.Journals.Quests;

public class MainQuestAssemblerTests
{
    [Fact]
    public void Assemble_ShouldApplyCompletionCascade()
    {
        var resource = new QuestResource
        {
            Id = "1",
            Requisites = [],
            Steps = [
                new StepResource { Number = 0, Value = 0, Requisites = [] },
                new StepResource { Number = 1, Value = 0, Requisites = [] },
                new StepResource { Number = 2, Value = 1, Requisites = [] },
                new StepResource { Number = 3, Value = 0, Requisites = [] }
            ]
        };

        var result = MainQuestAssembler.Assemble(resource);

        Assert.True(result.Steps[0].IsDone);
        Assert.True(result.Steps[1].IsDone);
        Assert.True(result.Steps[2].IsDone);
        Assert.False(result.Steps[3].IsDone);
    }
}
