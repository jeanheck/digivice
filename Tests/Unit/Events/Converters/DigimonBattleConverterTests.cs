namespace Tests.Events.Converters;

using Backend.Domain.Models;
using Backend.Domain.Models.Battles;
using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters;

public class DigimonBattleConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapAllDigimonBattleFields()
    {
        var digimonBattle = new DigimonBattle
        {
            Field = 0x02,
            Enemy = new Enemy
            {
                Id = 122,
                GroupId = 201,
                Condition = 0x01,
                Strength = 0,
                Defense = 0,
                Speed = 84,
                HP = new Vital { Current = 600, Max = 672 }
            }
        };

        var dto = DigimonBattleConverter.ToDTO(digimonBattle);

        Assert.True(dto.Field.HasValue);
        Assert.Equal((byte)0x02, dto.Field.Value);
        Assert.True(dto.Enemy.HasValue);
        Assert.Equal(201, dto.Enemy.Value!.GroupId.Value);
        Assert.Equal(122, dto.Enemy.Value!.Id.Value);
    }
}
