namespace Tests.Events.Converters.Battles;

using Backend.Domain.Models.Battles;
using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Battles;

public class EnemyConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapEveryField()
    {
        var enemy = new Enemy
        {
            Id = 122,
            GroupId = 201,
            Condition = 0x01,
            Strength = 95,
            Defense = 63,
            Speed = 84,
            HP = new Vital { Current = 600, Max = 672 }
        };

        var result = EnemyConverter.ToDTO(enemy);

        Assert.Equal(122, result.Id.Value);
        Assert.Equal(201, result.GroupId.Value);
        Assert.Equal(0x01, result.Condition.Value);
        Assert.Equal(95, result.Strength.Value);
        Assert.Equal(63, result.Defense.Value);
        Assert.Equal(84, result.Speed.Value);
        Assert.Equal(600, result.HP.Value!.Current.Value);
        Assert.Equal(672, result.HP.Value!.Max.Value);
    }

    [Fact]
    public void ToDTO_ShouldKeepZeroValuesPresent()
    {
        var result = EnemyConverter.ToDTO(new Enemy { Id = 122 });

        Assert.True(result.Condition.HasValue);
        Assert.Equal(0, result.Condition.Value);
        Assert.True(result.Strength.HasValue);
        Assert.True(result.Defense.HasValue);
        Assert.True(result.HP.Value!.Current.HasValue);
    }
}
