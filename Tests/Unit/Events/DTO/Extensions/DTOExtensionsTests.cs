namespace Tests.Events.DTO.Extensions;

using Backend.Events.DTO.Battles;
using Backend.Events.DTO.Extensions;
using Backend.Events.DTO.Parties.Digimons;
using Backend.Events.DTO.Shared;

public class DTOExtensionsTests
{
    [Fact]
    public void IsEmpty_ShouldReturnTrue_WhenNoPropertyIsSet()
    {
        var dto = new VitalDTO();

        Assert.True(dto.IsEmpty());
        Assert.False(dto.IsNotEmpty());
    }

    [Fact]
    public void IsEmpty_ShouldReturnFalse_WhenPropertyIsSetToZero()
    {
        var dto = new VitalDTO { Current = 0 };

        Assert.False(dto.IsEmpty());
        Assert.True(dto.IsNotEmpty());
    }

    [Fact]
    public void IsNotEmpty_ShouldReturnTrue_WhenOptionalHoldsNull()
    {
        var dto = new EnemyDTO { HP = new Optional<VitalDTO>(null!) };

        Assert.True(dto.IsNotEmpty());
    }
}
