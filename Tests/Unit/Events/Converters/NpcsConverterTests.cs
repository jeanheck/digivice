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
                    new NpcBattle { Id = "first", Won = true },
                    new NpcBattle { Id = "second", Won = true },
                ],
            },
            Natsumi = new Npc
            {
                Battles = [new NpcBattle { Id = "first", Won = false }],
            },
        };

        var dto = NpcsConverter.ToDTO(npcs);

        Assert.True(dto.Genji.HasValue);
        Assert.True(dto.Genji.Value!.Battles.HasValue);
        Assert.Equal(2, dto.Genji.Value.Battles.Value!.Count);
        Assert.Equal("first", dto.Genji.Value.Battles.Value[0].Id);
        Assert.True(dto.Genji.Value.Battles.Value[0].Won.Value);
        Assert.True(dto.Natsumi.HasValue);
        Assert.False(Assert.Single(dto.Natsumi.Value!.Battles.Value!).Won.Value);
    }
}
