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
        Assert.True(result.MainQuest.Steps[0].IsDone);
        Assert.True(result.MainQuest.Steps[1].IsDone);
        Assert.True(result.MainQuest.Steps[2].IsDone);
        Assert.False(result.MainQuest.Steps[3].IsDone);
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
        Assert.False(result.SideQuests[0].Steps[0].IsDone);
        Assert.True(result.SideQuests[0].Steps[1].IsDone);
    }

    [Fact]
    public void Assemble_ShouldNormalizeDuelIslandWhenTrophyIsObtained()
    {
        var resource = new JournalResource
        {
            MainQuest = new QuestResource
            {
                Id = "mainQuest",
                Requisites = [],
                Steps = [new StepResource { Number = 1, Value = 0, Requisites = [] }]
            },
            DuelIsland = [
                new QuestResource
                {
                    Id = "asukaTrophy",
                    Requisites = [],
                    Steps = [
                        new StepResource { Number = 1, Value = 0, Requisites = [] },
                        new StepResource { Number = 2, Value = 0, Requisites = [] },
                        new StepResource { Number = 3, Value = 0, Requisites = [] },
                        new StepResource { Number = 4, Value = 0, Requisites = [] },
                        new StepResource { Number = 5, Value = 0, Requisites = [] },
                        new StepResource { Number = 6, Value = 1, Requisites = [] }
                    ]
                }
            ]
        };

        var result = JournalAssembler.Assemble(resource);

        var asukaTrophy = Assert.Single(result.DuelIsland);
        Assert.True(asukaTrophy.Steps[0].IsDone);
        Assert.True(asukaTrophy.Steps[1].IsDone);
        Assert.True(asukaTrophy.Steps[2].IsDone);
        Assert.True(asukaTrophy.Steps[3].IsDone);
        Assert.True(asukaTrophy.Steps[4].IsDone);
        Assert.True(asukaTrophy.Steps[5].IsDone);
    }

    [Fact]
    public void Assemble_ShouldNotNormalizeDuelIslandWhenTrophyIsNotObtained()
    {
        var resource = new JournalResource
        {
            MainQuest = new QuestResource
            {
                Id = "mainQuest",
                Requisites = [],
                Steps = [new StepResource { Number = 1, Value = 0, Requisites = [] }]
            },
            DuelIsland = [
                new QuestResource
                {
                    Id = "asukaTrophy",
                    Requisites = [],
                    Steps = [
                        new StepResource { Number = 1, Value = 0x80, Requisites = [] },
                        new StepResource { Number = 2, Value = 0x01, Requisites = [] },
                        new StepResource { Number = 3, Value = 0, Requisites = [] },
                        new StepResource { Number = 4, Value = 0, Requisites = [] },
                        new StepResource { Number = 5, Value = 0, Requisites = [] },
                        new StepResource { Number = 6, Value = 0, Requisites = [] }
                    ]
                }
            ]
        };

        var result = JournalAssembler.Assemble(resource);

        var asukaTrophy = Assert.Single(result.DuelIsland);
        Assert.True(asukaTrophy.Steps[0].IsDone);
        Assert.True(asukaTrophy.Steps[1].IsDone);
        Assert.False(asukaTrophy.Steps[2].IsDone);
        Assert.False(asukaTrophy.Steps[5].IsDone);
    }

    [Fact]
    public void Assemble_ShouldNormalizeSunTrophyWhenTrophyIsObtained()
    {
        var resource = new JournalResource
        {
            MainQuest = new QuestResource
            {
                Id = "mainQuest",
                Requisites = [],
                Steps = [new StepResource { Number = 1, Value = 0, Requisites = [] }]
            },
            DuelIsland = [
                new QuestResource
                {
                    Id = "sunTrophy",
                    Requisites = [
                        new RequisiteResource { Id = "asukaTrophy", Value = 1 }
                    ],
                    Steps = [
                        new StepResource { Number = 1, Value = 0, Requisites = [] },
                        new StepResource { Number = 2, Value = 0, Requisites = [] },
                        new StepResource { Number = 3, Value = 0, Requisites = [] },
                        new StepResource { Number = 4, Value = 0, Requisites = [] },
                        new StepResource { Number = 5, Value = 0, Requisites = [] },
                        new StepResource { Number = 6, Value = 1, Requisites = [] }
                    ]
                }
            ]
        };

        var result = JournalAssembler.Assemble(resource);

        var sunTrophy = Assert.Single(result.DuelIsland);
        Assert.True(sunTrophy.Steps[0].IsDone);
        Assert.True(sunTrophy.Steps[1].IsDone);
        Assert.True(sunTrophy.Steps[2].IsDone);
        Assert.True(sunTrophy.Steps[3].IsDone);
        Assert.True(sunTrophy.Steps[4].IsDone);
        Assert.True(sunTrophy.Steps[5].IsDone);
    }

    [Fact]
    public void Assemble_ShouldSuppressSunTrophyStepsWhenRequisiteIsNotMet()
    {
        var resource = new JournalResource
        {
            MainQuest = new QuestResource
            {
                Id = "mainQuest",
                Requisites = [],
                Steps = [new StepResource { Number = 1, Value = 0, Requisites = [] }]
            },
            DuelIsland = [
                new QuestResource
                {
                    Id = "sunTrophy",
                    Requisites = [
                        new RequisiteResource { Id = "asukaTrophy", Value = 0 }
                    ],
                    Steps = [
                        new StepResource { Number = 1, Value = 0x80, Requisites = [] },
                        new StepResource { Number = 2, Value = 0x01, Requisites = [] },
                        new StepResource { Number = 3, Value = 0x02, Requisites = [] },
                        new StepResource { Number = 4, Value = 0x04, Requisites = [] },
                        new StepResource { Number = 5, Value = 0x08, Requisites = [] },
                        new StepResource { Number = 6, Value = 0, Requisites = [] }
                    ]
                }
            ]
        };

        var result = JournalAssembler.Assemble(resource);

        var sunTrophy = Assert.Single(result.DuelIsland);
        Assert.False(sunTrophy.Steps[0].IsDone);
        Assert.False(sunTrophy.Steps[1].IsDone);
        Assert.False(sunTrophy.Steps[2].IsDone);
        Assert.False(sunTrophy.Steps[3].IsDone);
        Assert.False(sunTrophy.Steps[4].IsDone);
        Assert.False(sunTrophy.Steps[5].IsDone);
    }

    [Fact]
    public void Assemble_ShouldSuppressSunTrophyStepsEvenWhenTrophyStepWouldCascade()
    {
        var resource = new JournalResource
        {
            MainQuest = new QuestResource
            {
                Id = "mainQuest",
                Requisites = [],
                Steps = [new StepResource { Number = 1, Value = 0, Requisites = [] }]
            },
            DuelIsland = [
                new QuestResource
                {
                    Id = "sunTrophy",
                    Requisites = [
                        new RequisiteResource { Id = "asukaTrophy", Value = 0 }
                    ],
                    Steps = [
                        new StepResource { Number = 1, Value = 0, Requisites = [] },
                        new StepResource { Number = 2, Value = 0, Requisites = [] },
                        new StepResource { Number = 3, Value = 0, Requisites = [] },
                        new StepResource { Number = 4, Value = 0, Requisites = [] },
                        new StepResource { Number = 5, Value = 0, Requisites = [] },
                        new StepResource { Number = 6, Value = 1, Requisites = [] }
                    ]
                }
            ]
        };

        var result = JournalAssembler.Assemble(resource);

        var sunTrophy = Assert.Single(result.DuelIsland);
        Assert.False(sunTrophy.Steps[0].IsDone);
        Assert.False(sunTrophy.Steps[1].IsDone);
        Assert.False(sunTrophy.Steps[2].IsDone);
        Assert.False(sunTrophy.Steps[3].IsDone);
        Assert.False(sunTrophy.Steps[4].IsDone);
        Assert.False(sunTrophy.Steps[5].IsDone);
    }
}
