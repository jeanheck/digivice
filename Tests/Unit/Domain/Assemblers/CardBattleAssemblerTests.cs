namespace Tests.Domain.Assemblers;

using Backend.Domain.Assemblers;
using Backend.Memory.Resources;
using Xunit;

public class CardBattleAssemblerTests
{
    [Fact]
    public void Assemble_ShouldMapIdCorrectly_WhenResourceIsValid()
    {
        var resource = new CardBattleResource
        {
            Id = 5,
        };

        var result = CardBattleAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Equal(5, result.Id);
    }

    [Fact]
    public void Assemble_ShouldFallBackToZero_WhenIdIsNull()
    {
        var resource = new CardBattleResource
        {
            Id = null,
        };

        var result = CardBattleAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Equal(0, result.Id);
    }
}
