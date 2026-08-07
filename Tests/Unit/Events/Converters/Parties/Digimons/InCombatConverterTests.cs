namespace Tests.Events.Converters.Parties.Digimons;

using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties.Digimons;

public class InCombatConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapConditionVitalsAndAttributeBuffs()
    {
        var digimonInCombat = new InCombat
        {
            Condition = 0x04,
            Strength = 252,
            Defense = 185,
            Speed = 84,
            HP = new Vital { Current = 1400, Max = 1850 },
            MP = new Vital { Current = 900, Max = 1140 }
        };

        var dto = InCombatConverter.ToDTO(digimonInCombat);

        Assert.Equal(0x04, dto.Condition.Value);
        Assert.Equal(252, dto.Strength.Value);
        Assert.Equal(185, dto.Defense.Value);
        Assert.Equal(84, dto.Speed.Value);
        Assert.Equal(1400, dto.HP.Value!.Current.Value);
        Assert.Equal(1850, dto.HP.Value.Max.Value);
        Assert.Equal(900, dto.MP.Value!.Current.Value);
        Assert.Equal(1140, dto.MP.Value.Max.Value);
    }
}
