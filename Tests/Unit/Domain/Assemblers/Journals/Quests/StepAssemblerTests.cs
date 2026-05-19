namespace Tests.Domain.Assemblers.Journals;

using Backend.Domain.Assemblers.Journals.Quests;
using Backend.Memory.Resources.Journals.Quests;

public class StepAssemblerTests
{
    [Fact]
    public void Assemble_ShouldMapAllFieldsCorrectly()
    {
        var resource = new StepResource
        {
            Number = 3,
            Value = 1,
            Requisites = [
                new RequisiteResource { Id = "1", Value = 1 }
            ]
        };

        var result = StepAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Equal(3, result.Number);
        Assert.Equal(1, result.Value);
        Assert.Single(result.Requisites);
        Assert.Equal("1", result.Requisites[0].Id);
    }
}
