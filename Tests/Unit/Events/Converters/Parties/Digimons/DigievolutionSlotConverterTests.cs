namespace Tests.Events.Converters.Parties.Digimons;

using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties.Digimons;

public class DigievolutionSlotConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapSlotAndNestedDigievolution()
    {
        var slot = new DigievolutionSlot
        {
            Index = 2,
            DigievolutionId = 10,
            Digievolution = new Digievolution { Level = 4 }
        };

        var dto = DigievolutionSlotConverter.ToDTO(slot);

        Assert.Equal(2, dto.Index);
        Assert.Equal(10, dto.DigievolutionId.Value);
        Assert.True(dto.Digievolution.HasValue);
        Assert.NotNull(dto.Digievolution.Value);
        Assert.Equal(4, dto.Digievolution.Value!.Level.Value);
    }

    [Fact]
    public void ToDTO_ShouldPreserveNullDigievolution()
    {
        var slot = new DigievolutionSlot
        {
            Index = 3,
            DigievolutionId = 0,
            Digievolution = null
        };

        var dto = DigievolutionSlotConverter.ToDTO(slot);

        Assert.Equal(3, dto.Index);
        Assert.Equal(0, dto.DigievolutionId.Value);
        Assert.True(dto.Digievolution.HasValue);
        Assert.Null(dto.Digievolution.Value);
    }
}
