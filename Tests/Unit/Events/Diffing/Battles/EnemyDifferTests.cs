namespace Tests.Events.Diffing.Battles;

using Backend.Domain.Models.Battles;
using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Diffing.Battles;

public class EnemyDifferTests
{
    [Fact]
    public void Diff_ShouldReturnNull_WhenNoChanges()
    {
        var previous = CreateBaseEnemy();
        var newEnemy = CreateBaseEnemy();

        Assert.Null(EnemyDiffer.Diff(previous, newEnemy));
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var newEnemy = CreateBaseEnemy();
        newEnemy.Id = 122;
        newEnemy.Speed = 84;

        var result = EnemyDiffer.Diff(null, newEnemy);

        Assert.NotNull(result);
        Assert.Equal(122, result.Id.Value);
        Assert.Equal(84, result.Speed.Value);
        Assert.Equal(672, result.HP.Value!.Current.Value);
    }

    [Fact]
    public void Diff_ShouldReturnOnlySpeed_WhenOnlySpeedChanges()
    {
        var previous = CreateBaseEnemy();
        var newEnemy = CreateBaseEnemy();
        newEnemy.Speed = 84;

        var result = EnemyDiffer.Diff(previous, newEnemy);

        Assert.NotNull(result);
        Assert.Equal(84, result.Speed.Value);
        Assert.False(result.Id.HasValue);
        Assert.False(result.Condition.HasValue);
        Assert.False(result.Strength.HasValue);
        Assert.False(result.Defense.HasValue);
        Assert.False(result.HP.HasValue);
        Assert.False(result.MP.HasValue);
    }

    private static Enemy CreateBaseEnemy()
    {
        return new Enemy
        {
            Id = 0,
            Condition = 0,
            Strength = 0,
            Defense = 0,
            Speed = 0,
            HP = new Vital { Current = 672, Max = 672 },
            MP = new Vital { Current = 336, Max = 336 }
        };
    }
}
