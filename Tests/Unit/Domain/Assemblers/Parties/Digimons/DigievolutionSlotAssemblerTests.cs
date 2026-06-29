namespace Tests.Domain.Assemblers.Parties.Digimons;

using Backend.Domain.Assemblers.Parties.Digimons;
using Backend.Memory.Resources.Parties.Digimons;

public class DigievolutionSlotAssemblerTests
{
    [Fact]
    public void Assemble_ShouldMapAllFieldsCorrectly()
    {
        var resource = new DigievolutionSlotResource
        {
            Index = 1,
            DigievolutionId = 32,
            DigievolutionResource = new DigievolutionResource { Level = 15 }
        };

        var result = DigievolutionSlotAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Equal(1, result.Index);
        Assert.Equal(32, result.DigievolutionId);
        Assert.NotNull(result.Digievolution);
        Assert.Equal(15, result.Digievolution.Level);
    }

    [Fact]
    public void Assemble_ShouldReturnNullFields_WhenResourceIsEmpty()
    {
        var resource = new DigievolutionSlotResource
        {
            Index = 3,
            DigievolutionId = null,
            DigievolutionResource = null
        };

        var result = DigievolutionSlotAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Equal(3, result.Index);
        Assert.Null(result.DigievolutionId);
        Assert.Null(result.Digievolution);
    }
}
