namespace Tests.Domain.Assemblers;

using Backend.Domain.Assemblers;
using Backend.Memory.Resources;
using Xunit;

public class NpcsAssemblerTests
{
    [Fact]
    public void Assemble_ShouldMapBattlesFromResourceAsWonBool()
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
            Catherine = new NpcResource
            {
                Battles = [new NpcBattleResource { Id = "first", Value = 0 }],
            },
        };

        var result = NpcsAssembler.Assemble(resource);

        Assert.Equal(2, result.Genji.Battles.Count);
        Assert.True(result.Genji.Battles.Single(battle => battle.Id == "first").Won);
        Assert.True(result.Genji.Battles.Single(battle => battle.Id == "second").Won);
        Assert.True(Assert.Single(result.Natsumi.Battles).Won);
        Assert.False(Assert.Single(result.Catherine.Battles).Won);
        Assert.Empty(result.Lucia.Battles);
    }
}
