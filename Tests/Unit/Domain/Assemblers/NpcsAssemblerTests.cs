namespace Tests.Domain.Assemblers;

using Backend.Domain.Assemblers;
using Backend.Domain.Models;
using Backend.Memory.Resources;

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

    [Fact]
    public void Assemble_ShouldMapEveryNpcToItsOwnProperty()
    {
        var resource = new NpcsResource();
        var resourceProperties = typeof(NpcsResource).GetProperties();
        foreach (var property in resourceProperties)
        {
            property.SetValue(resource, new NpcResource
            {
                Battles = [new NpcBattleResource { Id = property.Name, Value = 1 }],
            });
        }

        var result = NpcsAssembler.Assemble(resource);

        var modelProperties = typeof(Npcs).GetProperties();
        Assert.Equal(23, resourceProperties.Length);
        Assert.Equal(
            resourceProperties.Select(property => property.Name).Order(),
            modelProperties.Select(property => property.Name).Order());
        foreach (var property in modelProperties)
        {
            var npc = (Npc)property.GetValue(result)!;
            var battle = Assert.Single(npc.Battles);
            Assert.Equal(property.Name, battle.Id);
            Assert.True(battle.Won);
        }
    }
}
