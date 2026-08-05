namespace Tests.Events.Converters.Parties.Digimons;

using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties.Digimons;

public class DigimonInCombatConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapConditionAndVitals()
    {
        var digimonInCombat = new DigimonInCombat
        {
            Condition = 0x04,
            HP = new Vital { Current = 1400, Max = 1850 },
            MP = new Vital { Current = 900, Max = 1140 }
        };

        var dto = DigimonInCombatConverter.ToDTO(digimonInCombat);

        Assert.Equal(0x04, dto.Condition.Value);
        Assert.Equal(1400, dto.HP.Value!.Current.Value);
        Assert.Equal(1850, dto.HP.Value.Max.Value);
        Assert.Equal(900, dto.MP.Value!.Current.Value);
        Assert.Equal(1140, dto.MP.Value.Max.Value);
    }
}
