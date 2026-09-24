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

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Assemble_ShouldReturnNullId_WhenIdIsNotPositive(int rawId)
    {
        var resource = new CardBattleResource
        {
            Id = rawId,
        };

        var result = CardBattleAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Null(result.Id);
    }
}
