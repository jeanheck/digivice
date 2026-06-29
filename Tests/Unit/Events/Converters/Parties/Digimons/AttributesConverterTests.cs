namespace Tests.Events.Converters.Parties.Digimons;

using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties.Digimons;

public class AttributesConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapAllAttributesFields()
    {
        var dto = AttributesConverter.ToDTO(new Attributes
        {
            Strength = 1,
            Defense = 2,
            Spirit = 3,
            Wisdom = 4,
            Speed = 5,
            Charisma = 6
        });

        Assert.Equal(1, dto.Strength.Value);
        Assert.Equal(2, dto.Defense.Value);
        Assert.Equal(3, dto.Spirit.Value);
        Assert.Equal(4, dto.Wisdom.Value);
        Assert.Equal(5, dto.Speed.Value);
        Assert.Equal(6, dto.Charisma.Value);
    }
}
