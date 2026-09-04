namespace Tests.Domain.Assemblers;

using Backend.Domain.Assemblers;
using Backend.Memory.Resources;
using Xunit;

public class NpcsAssemblerTests
{
    [Fact]
    public void Assemble_ShouldMapBattlesFromResource()
    {
        var resource = new NpcsResource
        {
            Genji = new NpcResource
            {
                Battles =
                [
                    new NpcBattleResource { Id = "first", Value = 0x20 },
                    new NpcBattleResource { Id = "second", Value = 0x01 },
                ],
            },
            Natsumi = new NpcResource
            {
                Battles = [new NpcBattleResource { Id = "first", Value = 0x02 }],
            },
        };

        var result = NpcsAssembler.Assemble(resource);

        Assert.Equal(2, result.Genji.Battles.Count);
        Assert.Equal(0x20, result.Genji.Battles.Single(battle => battle.Id == "first").Value);
        Assert.Equal(0x01, result.Genji.Battles.Single(battle => battle.Id == "second").Value);
        Assert.Equal(0x02, Assert.Single(result.Natsumi.Battles).Value);
        Assert.Empty(result.Catherine.Battles);
    }
}
