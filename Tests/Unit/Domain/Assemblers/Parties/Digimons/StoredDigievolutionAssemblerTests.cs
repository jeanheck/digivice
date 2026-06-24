namespace Tests.Domain.Assemblers.Parties.Digimons;

using Backend.Domain.Assemblers.Parties.Digimons;
using Backend.Memory.Resources.Parties.Digimons;

public class StoredDigievolutionAssemblerTests
{
    [Fact]
    public void Assemble_ShouldMapAllFieldsCorrectly()
    {
        var resource = new StoredDigievolutionResource { DigievolutionId = 386, Level = 14 };

        var result = StoredDigievolutionAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Equal(386, result.DigievolutionId);
        Assert.Equal(14, result.Level);
    }
}
