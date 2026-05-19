namespace Tests.Domain.Assemblers;

using Backend.Domain.Assemblers;
using Backend.Memory.Resources;
using Backend.Memory.Resources.Journals;
using Backend.Memory.Resources.Journals.Quests;

public class JournalAssemblerTests
{
    [Fact]
    public void Assemble_ShouldApplyCompletionCascadeToMainQuest()
    {
        var resource = new JournalResource
        {
            MainQuest = new QuestResource
            {
                Id = "1",
                Requisites = [],
                Steps = [
                    new StepResource { Number = 0, Value = 0, Requisites = [] },
                    new StepResource { Number = 1, Value = 0, Requisites = [] },
                    new StepResource { Number = 2, Value = 1, Requisites = [] },
                    new StepResource { Number = 3, Value = 0, Requisites = [] }
                ]
            },
            SideQuests = []
        };

        var result = JournalAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Equal(1, result.MainQuest.Steps[0].Value);
        Assert.Equal(1, result.MainQuest.Steps[1].Value);
        Assert.Equal(1, result.MainQuest.Steps[2].Value);
        Assert.Equal(0, result.MainQuest.Steps[3].Value);
    }

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
        Assert.Equal(0, result.SideQuests[0].Steps[0].Value);
        Assert.Equal(1, result.SideQuests[0].Steps[1].Value);
    }
}
