namespace Tests.Events.Converters;

using Backend.Domain.Models;
using Backend.Domain.Models.Battles;
using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters;

public class BattleConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapAllBattleFields()
    {
        var battle = new Battle
        {
            Field = 0x02,
            Enemy = new Enemy
            {
                Id = 122,
                Condition = 0x01,
                Strength = 0,
                Defense = 0,
                Speed = 84,
                HP = new Vital { Current = 600, Max = 672 }
            }
        };

        var dto = BattleConverter.ToDTO(battle);

        Assert.True(dto.Field.HasValue);
        Assert.Equal((byte)0x02, dto.Field.Value);
        Assert.True(dto.Enemy.HasValue);
        Assert.Equal(122, dto.Enemy.Value!.Id.Value);
    }
}
