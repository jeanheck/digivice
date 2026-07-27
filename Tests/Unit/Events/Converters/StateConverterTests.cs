namespace Tests.Events.Converters;

using Backend.Domain.Models;
using Backend.Domain.Models.Journals;
using Backend.Events.Converters;

public class StateConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapPlayerPartyAndJournal()
    {
        var state = new State
        {
            Player = new Player { Bits = 100, MapId = "0001" },
            Party = new Party { Slots = [] },
            Journal = new Journal { MainQuest = new Quest { Id = "MainQuest" }, SideQuests = [] }
        };

        var dto = StateConverter.ToDTO(state);

        Assert.NotNull(dto.Player);
        Assert.True(dto.Player.Bits.HasValue);
        Assert.Equal(100, dto.Player.Bits.Value);

        Assert.NotNull(dto.Party);
        Assert.True(dto.Party.Slots.HasValue);
        Assert.Empty(dto.Party.Slots.Value!);

        Assert.NotNull(dto.Journal);
        Assert.True(dto.Journal.MainQuest.HasValue);
        Assert.Equal("MainQuest", dto.Journal.MainQuest.Value!.Id);
    }

    [Fact]
    public void ToDTO_ShouldPreserveNullEntities()
    {
        var state = new State
        {
            Player = null!,
            Party = null!,
            Journal = null!
        };

        var dto = StateConverter.ToDTO(state);

        Assert.Null(dto.Player);
        Assert.Null(dto.Party);
        Assert.Null(dto.Journal);
    }
}
