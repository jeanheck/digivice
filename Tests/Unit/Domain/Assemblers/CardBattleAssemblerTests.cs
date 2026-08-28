namespace Tests.Domain.Assemblers;

using Backend.Domain.Assemblers;
using Backend.Memory.Resources;
using Xunit;

public class CardBattleAssemblerTests
{
    [Fact]
    public void Assemble_ShouldMapOpponentIdCorrectly_WhenResourceIsValid()
    {
        var resource = new CardBattleResource
        {
            OpponentId = 5,
        };

        var result = CardBattleAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Equal(5, result.OpponentId);
    }

    [Fact]
    public void Assemble_ShouldFallBackToZero_WhenOpponentIdIsNull()
    {
        var resource = new CardBattleResource
        {
            OpponentId = null,
        };

        var result = CardBattleAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Equal(0, result.OpponentId);
    }
}
