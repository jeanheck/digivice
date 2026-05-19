namespace Tests.Domain.Assemblers.Journals;

using Backend.Domain.Assemblers.Journals;
using Backend.Memory.Resources.Journals;
using Backend.Memory.Resources.Journals.Quests;

public class QuestAssemblerTests
{
    [Fact]
    public void Assemble_ShouldMapAllFieldsCorrectly()
    {
        var resource = new QuestResource
        {
            Id = "12",
            Requisites = [new RequisiteResource { Id = "1", Value = 1 }],
            Steps = [new StepResource { Number = 0, Value = 1, Requisites = [] }]
        };

        var result = QuestAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Equal("12", result.Id);
        Assert.Single(result.Requisites);
        Assert.Single(result.Steps);
        Assert.Equal(0, result.Steps[0].Number);
    }
}
