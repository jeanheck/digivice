namespace Tests.Events.Converters.Parties.Digimons;

using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties.Digimons;

public class VitalsConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapAllVitalsFields()
    {
        var dto = VitalsConverter.ToDTO(new Vitals { CurrentHP = 1, MaxHP = 2, CurrentMP = 3, MaxMP = 4 });

        Assert.Equal(1, dto.CurrentHP.Value);
        Assert.Equal(2, dto.MaxHP.Value);
        Assert.Equal(3, dto.CurrentMP.Value);
        Assert.Equal(4, dto.MaxMP.Value);
    }
}
