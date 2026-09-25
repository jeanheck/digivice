namespace Tests.Events.Converters.Parties.Digimons;

using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties.Digimons;

public class VitalConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapAllVitalFields()
    {
        var dto = VitalConverter.ToDTO(new Vital { Current = 1, Max = 2 });

        Assert.Equal(1, dto.Current.Value);
        Assert.Equal(2, dto.Max.Value);
    }
}
