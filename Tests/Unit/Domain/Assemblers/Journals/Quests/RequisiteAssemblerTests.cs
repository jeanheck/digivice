namespace Tests.Domain.Assemblers.Journals;

using Backend.Domain.Assemblers.Journals.Quests;
using Backend.Memory.Resources.Journals.Quests;

public class RequisiteAssemblerTests
{
    [Fact]
    public void Assemble_ShouldMapAllFieldsCorrectly()
    {
        var resource = new RequisiteResource
        {
            Id = "5",
            Value = 1
        };

        var result = RequisiteAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Equal("5", result.Id);
        Assert.Equal(1, result.Value);
    }
}
