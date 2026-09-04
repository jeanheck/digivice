namespace Tests.Events.Converters;

using Backend.Domain.Models;
using Backend.Events.Converters;

public class NpcsConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapNpcBattles()
    {
        var npcs = new Npcs
        {
            Genji = new Npc
            {
                Battles =
                [
                    new NpcBattle { Id = "first", Value = 0x20 },
                    new NpcBattle { Id = "second", Value = 0x01 },
                ],
            },
            Natsumi = new Npc
            {
                Battles = [new NpcBattle { Id = "first", Value = 0 }],
            },
        };

        var dto = NpcsConverter.ToDTO(npcs);

        Assert.True(dto.Genji.HasValue);
        Assert.True(dto.Genji.Value!.Battles.HasValue);
        Assert.Equal(2, dto.Genji.Value.Battles.Value!.Count);
        Assert.Equal("first", dto.Genji.Value.Battles.Value[0].Id);
        Assert.Equal(0x20, dto.Genji.Value.Battles.Value[0].Value.Value);
        Assert.True(dto.Natsumi.HasValue);
        Assert.Equal(0, Assert.Single(dto.Natsumi.Value!.Battles.Value!).Value.Value);
    }
}
