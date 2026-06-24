namespace Tests.Events.Converters.Parties.Digimons;

using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties.Digimons;

public class ResistancesConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapAllResistanceFields()
    {
        var dto = ResistancesConverter.ToDTO(new Resistances
        {
            Fire = 1,
            Water = 2,
            Ice = 3,
            Wind = 4,
            Thunder = 5,
            Machine = 6,
            Dark = 7
        });

        Assert.Equal(1, dto.Fire.Value);
        Assert.Equal(2, dto.Water.Value);
        Assert.Equal(3, dto.Ice.Value);
        Assert.Equal(4, dto.Wind.Value);
        Assert.Equal(5, dto.Thunder.Value);
        Assert.Equal(6, dto.Machine.Value);
        Assert.Equal(7, dto.Dark.Value);
    }
}
