namespace Tests.Domain.Assemblers;

using Backend.Domain.Assemblers;
using Backend.Memory.Resources;
using Backend.Memory.Resources.Journals;
using Backend.Memory.Resources.Journals.Quests;

public class JournalAssemblerTests
{
    [Fact]
    public void Assemble_ShouldNotApplyCompletionCascadeToSideQuests()
    {
        var resource = new JournalResource
        {
            MainQuest = new QuestResource
            {
                Id = "1",
                Requisites = [],
                Steps = [new StepResource { Number = 0, Value = 0, Requisites = [] }]
            },
            SideQuests = [
                new QuestResource
                {
                    Id = "2",
                    Requisites = [],
                    Steps = [
                        new StepResource { Number = 0, Value = 0, Requisites = [] },
                        new StepResource { Number = 1, Value = 1, Requisites = [] }
                    ]
                }
            ]
        };

        var result = JournalAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Single(result.SideQuests);
        Assert.False(result.SideQuests[0].Steps[0].IsDone);
        Assert.True(result.SideQuests[0].Steps[1].IsDone);
    }
}
