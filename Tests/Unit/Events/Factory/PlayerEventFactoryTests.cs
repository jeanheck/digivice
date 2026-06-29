namespace Tests.Events.Factory;

using Backend.Domain.Models;
using Backend.Events.DTO;
using Backend.Events.Factory;
using Backend.Events.Models;

public class PlayerEventFactoryTests
{
    [Fact]
    public void Create_ShouldReturnNoEvents_WhenPlayerHasNoChanges()
    {
        var previousState = CreateState(new Player { Name = "Agumon", Bits = 100, MapId = "0001" });
        var newState = CreateState(new Player { Name = "Agumon", Bits = 100, MapId = "0001" });

        var result = PlayerEventFactory.Create(previousState, newState);

        Assert.Empty(result);
    }

    [Fact]
    public void Create_ShouldReturnPlayerChangedEvent_WhenPlayerChanges()
    {
        var previousState = CreateState(new Player { Name = "Agumon", Bits = 100, MapId = "0001" });
        var newState = CreateState(new Player { Name = "Agumon", Bits = 200, MapId = "0001" });

        var result = PlayerEventFactory.Create(previousState, newState).ToList();

        var ev = Assert.Single(result);
        Assert.Equal(EventType.PlayerChanged, ev.Type);

        var dto = Assert.IsType<PlayerDTO>(ev.Payload);
        Assert.True(dto.Bits.HasValue);
        Assert.Equal(200, dto.Bits.Value);
    }

    private static State CreateState(Player player)
    {
        return new State { Player = player };
    }
}
