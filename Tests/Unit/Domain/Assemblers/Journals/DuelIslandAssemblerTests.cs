namespace Tests.Domain.Assemblers.Journals;

using Backend.Domain.Assemblers.Journals;
using Backend.Memory.Resources.Journals;
using Backend.Memory.Resources.Journals.Quests;

public class DuelIslandAssemblerTests
{
    [Fact]
    public void Assemble_ShouldNormalizeWhenTrophyIsObtained()
    {
        var resource = new QuestResource
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
        };

        var result = DuelIslandAssembler.Assemble(resource);

        Assert.True(result.Steps[0].IsDone);
        Assert.True(result.Steps[1].IsDone);
        Assert.True(result.Steps[2].IsDone);
        Assert.True(result.Steps[3].IsDone);
        Assert.True(result.Steps[4].IsDone);
        Assert.True(result.Steps[5].IsDone);
    }

    [Fact]
    public void Assemble_ShouldNotNormalizeWhenTrophyIsNotObtained()
    {
        var resource = new QuestResource
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
        };

        var result = DuelIslandAssembler.Assemble(resource);

        Assert.True(result.Steps[0].IsDone);
        Assert.True(result.Steps[1].IsDone);
        Assert.False(result.Steps[2].IsDone);
        Assert.False(result.Steps[5].IsDone);
    }

    [Fact]
    public void Assemble_ShouldNormalizeSunTrophyWhenTrophyIsObtained()
    {
        var resource = new QuestResource
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
        };

        var result = DuelIslandAssembler.Assemble(resource);

        Assert.True(result.Steps[0].IsDone);
        Assert.True(result.Steps[1].IsDone);
        Assert.True(result.Steps[2].IsDone);
        Assert.True(result.Steps[3].IsDone);
        Assert.True(result.Steps[4].IsDone);
        Assert.True(result.Steps[5].IsDone);
    }

    [Fact]
    public void Assemble_ShouldSuppressStepsWhenRequisiteIsNotMet()
    {
        var resource = new QuestResource
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
        };

        var result = DuelIslandAssembler.Assemble(resource);

        Assert.False(result.Steps[0].IsDone);
        Assert.False(result.Steps[1].IsDone);
        Assert.False(result.Steps[2].IsDone);
        Assert.False(result.Steps[3].IsDone);
        Assert.False(result.Steps[4].IsDone);
        Assert.False(result.Steps[5].IsDone);
    }

    [Fact]
    public void Assemble_ShouldSuppressStepsEvenWhenTrophyStepWouldCascade()
    {
        var resource = new QuestResource
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
        };

        var result = DuelIslandAssembler.Assemble(resource);

        Assert.False(result.Steps[0].IsDone);
        Assert.False(result.Steps[1].IsDone);
        Assert.False(result.Steps[2].IsDone);
        Assert.False(result.Steps[3].IsDone);
        Assert.False(result.Steps[4].IsDone);
        Assert.False(result.Steps[5].IsDone);
    }
}
