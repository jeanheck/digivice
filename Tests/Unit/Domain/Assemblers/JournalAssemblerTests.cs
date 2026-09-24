namespace Tests.Domain.Assemblers;

using Backend.Domain.Assemblers;
using Backend.Domain.Models.Journals;
using Backend.Memory.Resources;
using Backend.Memory.Resources.Journals;
using Backend.Memory.Resources.Journals.Quests;

public class JournalAssemblerTests
{
    [Fact]
    public void Assemble_ShouldApplyCompletionCascadeToMainQuest()
    {
        var resource = new JournalResource { MainQuest = CreateQuestWithOnlyLastStepDone("mainQuest") };

        var result = JournalAssembler.Assemble(resource);

        Assert.All(result.MainQuest.Steps, step => Assert.True(step.IsDone));
    }

    [Fact]
    public void Assemble_ShouldNotApplyCompletionCascadeToSideQuests()
    {
        var resource = new JournalResource { SideQuests = [CreateQuestWithOnlyLastStepDone("treeBoots")] };

        var result = JournalAssembler.Assemble(resource);

        AssertOnlyLastStepDone(Assert.Single(result.SideQuests));
    }

    [Fact]
    public void Assemble_ShouldNotApplyCompletionCascadeToLegendaryWeapons()
    {
        var resource = new JournalResource { LegendaryWeapons = [CreateQuestWithOnlyLastStepDone("eternally")] };

        var result = JournalAssembler.Assemble(resource);

        AssertOnlyLastStepDone(Assert.Single(result.LegendaryWeapons));
    }

    [Fact]
    public void Assemble_ShouldNotApplyCompletionCascadeToDriAgents()
    {
        var resource = new JournalResource { DriAgents = [CreateQuestWithOnlyLastStepDone("driAgentGuilmon")] };

        var result = JournalAssembler.Assemble(resource);

        AssertOnlyLastStepDone(Assert.Single(result.DriAgents));
    }

    [Fact]
    public void Assemble_ShouldApplyDuelIslandRulesToDuelIsland()
    {
        var suppressedQuest = new QuestResource
        {
            Id = "sunTrophy",
            Requisites = [new RequisiteResource { Id = "asukaTrophy", Value = 0 }],
            Steps = [
                new StepResource { Number = 1, Value = 0x80, Requisites = [] },
                new StepResource { Number = 2, Value = 0, Requisites = [] }
            ]
        };
        var resource = new JournalResource
        {
            DuelIsland = [CreateQuestWithOnlyLastStepDone("asukaTrophy"), suppressedQuest]
        };

        var result = JournalAssembler.Assemble(resource);

        Assert.Equal(2, result.DuelIsland.Count);
        Assert.All(result.DuelIsland[0].Steps, step => Assert.True(step.IsDone));
        Assert.All(result.DuelIsland[1].Steps, step => Assert.False(step.IsDone));
    }

    private static QuestResource CreateQuestWithOnlyLastStepDone(string id)
    {
        return new QuestResource
        {
            Id = id,
            Requisites = [],
            Steps = [
                new StepResource { Number = 1, Value = 0, Requisites = [] },
                new StepResource { Number = 2, Value = 0, Requisites = [] },
                new StepResource { Number = 3, Value = 1, Requisites = [] }
            ]
        };
    }

    private static void AssertOnlyLastStepDone(Quest quest)
    {
        Assert.False(quest.Steps[0].IsDone);
        Assert.False(quest.Steps[1].IsDone);
        Assert.True(quest.Steps[2].IsDone);
    }
}
