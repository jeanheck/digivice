namespace Tests.Domain.Assemblers;

using Backend.Domain.Assemblers;
using Backend.Memory.Resources;
using Backend.Memory.Resources.Battles;
using Backend.Memory.Resources.Parties.Digimons;

public class BattleAssemblerTests
{
    [Fact]
    public void Assemble_ShouldMapEnemyFields()
    {
        var resource = new BattleResource
        {
            Enemy = new EnemyResource
            {
                Id = 122,
                Condition = 0x01,
                Strength = 0,
                Defense = 0,
                Speed = 84,
                HP = new VitalResource { Current = 600, Max = 672 },
                MP = new VitalResource { Current = 288, Max = 336 }
            }
        };

        var result = BattleAssembler.Assemble(resource);

        Assert.Equal(122, result.Enemy.Id);
        Assert.Equal(0x01, result.Enemy.Condition);
        Assert.Equal(84, result.Enemy.Speed);
        Assert.Equal(600, result.Enemy.HP.Current);
        Assert.Equal(672, result.Enemy.HP.Max);
        Assert.Equal(288, result.Enemy.MP.Current);
        Assert.Equal(336, result.Enemy.MP.Max);
    }
}
