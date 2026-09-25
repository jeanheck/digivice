namespace Tests.Events.Factory;

using Backend.Domain.Models;
using Backend.Events.DTO;
using Backend.Events.Factory;
using Backend.Events.Models;

public class NpcsEventFactoryTests
{
    [Fact]
    public void Create_ShouldReturnNoEvents_WhenNpcsHaveNoChanges()
    {
        var previousState = CreateState(CreateBaseNpcs());
        var newState = CreateState(CreateBaseNpcs());

        var result = NpcsEventFactory.Create(previousState, newState);

        Assert.Empty(result);
    }

    [Fact]
    public void Create_ShouldReturnNpcsChangedEvent_WhenNpcsChange()
    {
        var previousState = CreateState(CreateBaseNpcs());
        var newNpcs = CreateBaseNpcs();
        newNpcs.Genji.Battles[0].Value = 0x20;
        var newState = CreateState(newNpcs);

        var result = NpcsEventFactory.Create(previousState, newState).ToList();

        var ev = Assert.Single(result);
        Assert.Equal(EventType.NpcsChanged, ev.Type);

        var dto = Assert.IsType<NpcsDTO>(ev.Payload);
        Assert.True(dto.Genji.HasValue);
    }

    private static State CreateState(Npcs npcs)
    {
        return new State { Npcs = npcs };
    }

    private static Npcs CreateBaseNpcs()
    {
        return new Npcs
        {
            Genji = new Npc
            {
                Battles = [new NpcBattle { Id = "first", Value = 0 }],
            },
        };
    }
}
