namespace Tests.Domain.Assemblers;

using Backend.Domain.Assemblers;
using Backend.Memory.Resources;
using Backend.Memory.Resources.Battles;
using Backend.Memory.Resources.Parties.Digimons;

public class DigimonBattleAssemblerTests
{
    [Fact]
    public void Assemble_ShouldMapEnemyFields()
    {
        var resource = new DigimonBattleResource
        {
            Field = 0x02,
            Enemy = new EnemyResource
            {
                Id = 122,
                GroupId = 201,
                Condition = 0x01,
                Strength = 95,
                Defense = 63,
                Speed = 84,
                HP = new VitalResource { Current = 600, Max = 672 }
            }
        };

        var result = DigimonBattleAssembler.Assemble(resource);

        Assert.Equal(0x02, result.Field);
        Assert.NotNull(result.Enemy);
        Assert.Equal(201, result.Enemy.GroupId);
        Assert.Equal(122, result.Enemy.Id);
        Assert.Equal(0x01, result.Enemy.Condition);
        Assert.Equal(95, result.Enemy.Strength);
        Assert.Equal(63, result.Enemy.Defense);
        Assert.Equal(84, result.Enemy.Speed);
        Assert.Equal(600, result.Enemy.HP.Current);
        Assert.Equal(672, result.Enemy.HP.Max);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Assemble_ShouldReturnNullEnemy_WhenEnemyIdIsNotPositive(int rawEnemyId)
    {
        var resource = new DigimonBattleResource
        {
            Field = 0x00,
            Enemy = new EnemyResource
            {
                Id = rawEnemyId,
                GroupId = 201,
                Speed = 84,
                HP = new VitalResource { Current = 600, Max = 672 }
            }
        };

        var result = DigimonBattleAssembler.Assemble(resource);

        Assert.Equal(0x00, result.Field);
        Assert.Null(result.Enemy);
    }
}
