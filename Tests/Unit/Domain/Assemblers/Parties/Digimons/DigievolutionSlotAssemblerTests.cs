namespace Tests.Domain.Assemblers.Parties.Digimons;

using Backend.Domain.Assemblers.Parties.Digimons;
using Backend.Memory.Resources.Parties.Digimons;

public class DigievolutionSlotAssemblerTests
{
    [Fact]
    public void Assemble_ShouldUseStoredDigievolution_WhenIdIsFound()
    {
        var resource = new DigievolutionSlotResource { Index = 1, DigievolutionId = 32 };
        List<StoredDigievolutionResource> storedDigievolutions =
        [
            new() { DigievolutionId = 10, Level = 3, Dvxp = 50 },
            new() { DigievolutionId = 32, Level = 15, Dvxp = 700 }
        ];

        var result = DigievolutionSlotAssembler.Assemble(resource, storedDigievolutions);

        Assert.Equal(1, result.Index);
        Assert.Equal(32, result.DigievolutionId);
        Assert.NotNull(result.Digievolution);
        Assert.Equal(15, result.Digievolution.Level);
        Assert.Equal(700, result.Digievolution.Dvxp);
    }

    [Fact]
    public void Assemble_ShouldUseDefaultLevelAndDvxp_WhenIdIsNotStored()
    {
        var resource = new DigievolutionSlotResource { Index = 2, DigievolutionId = 32 };
        List<StoredDigievolutionResource> storedDigievolutions =
        [
            new() { DigievolutionId = 10, Level = 3, Dvxp = 50 }
        ];

        var result = DigievolutionSlotAssembler.Assemble(resource, storedDigievolutions);

        Assert.Equal(2, result.Index);
        Assert.Equal(32, result.DigievolutionId);
        Assert.NotNull(result.Digievolution);
        Assert.Equal(1, result.Digievolution.Level);
        Assert.Equal(0, result.Digievolution.Dvxp);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(int.MinValue, 0)]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    public void Assemble_ShouldClampNegativeDvxpToZero(int rawDvxp, int expectedDvxp)
    {
        var resource = new DigievolutionSlotResource { Index = 1, DigievolutionId = 32 };
        List<StoredDigievolutionResource> storedDigievolutions =
        [
            new() { DigievolutionId = 32, Level = 4, Dvxp = rawDvxp }
        ];

        var result = DigievolutionSlotAssembler.Assemble(resource, storedDigievolutions);

        Assert.NotNull(result.Digievolution);
        Assert.Equal(expectedDvxp, result.Digievolution.Dvxp);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Assemble_ShouldReturnNullFields_WhenDigievolutionIdIsNotPositive(int rawDigievolutionId)
    {
        var resource = new DigievolutionSlotResource { Index = 3, DigievolutionId = rawDigievolutionId };

        var result = DigievolutionSlotAssembler.Assemble(resource, []);

        Assert.Equal(3, result.Index);
        Assert.Null(result.DigievolutionId);
        Assert.Null(result.Digievolution);
    }
}
