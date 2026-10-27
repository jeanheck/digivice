namespace Tests.Domain.Assemblers;

using Backend.Domain.Assemblers;
using Backend.Memory.Resources;

public class CardInventoryAssemblerTests
{
    [Fact]
    public void Assemble_ShouldMapQuantitiesToCardIds_WhenResourceHasOwnedCards()
    {
        byte[] quantities = new byte[314];
        quantities[0] = 2; // Card 1000
        quantities[9] = 1; // Card 1009
        quantities[313] = 3; // Card 1313

        var resource = new CardInventoryResource
        {
            Quantities = quantities
        };

        var result = CardInventoryAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Equal(3, result.Cards.Count);
        Assert.Equal(2, result.Cards[1000]);
        Assert.Equal(1, result.Cards[1009]);
        Assert.Equal(3, result.Cards[1313]);
        Assert.False(result.Cards.ContainsKey(1001));
    }

    [Fact]
    public void Assemble_ShouldReturnEmptyCards_WhenAllQuantitiesAreZero()
    {
        var resource = new CardInventoryResource
        {
            Quantities = new byte[314]
        };

        var result = CardInventoryAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Empty(result.Cards);
    }
}
