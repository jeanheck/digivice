namespace Tests.Domain.Assemblers.Parties.Digimons;

using Backend.Domain.Assemblers.Parties.Digimons;
using Backend.Memory.Resources.Parties.Digimons;

public class DigievolutionAssemblerTests
{
    [Fact]
    public void Assemble_ShouldMapAllFieldsCorrectly()
    {
        var resource = new DigievolutionResource { Level = 45 };

        var result = DigievolutionAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Equal(45, result.Level);
    }
}
