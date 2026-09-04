namespace Tests.Events.Factory;

using Backend.Domain.Models;
using Backend.Events.DTO;
using Backend.Events.Factory;
using Backend.Events.Models;

public class AuctionsEventFactoryTests
{
    [Fact]
    public void Create_ShouldReturnNoEvents_WhenAuctionsHaveNoChanges()
    {
        var previousState = CreateState(new Auctions { DivineBarrier = true });
        var newState = CreateState(new Auctions { DivineBarrier = true });

        var result = AuctionsEventFactory.Create(previousState, newState);

        Assert.Empty(result);
    }

    [Fact]
    public void Create_ShouldReturnAuctionsChangedEvent_WhenAuctionsChange()
    {
        var previousState = CreateState(new Auctions { DivineBarrier = false });
        var newState = CreateState(new Auctions { DivineBarrier = true });

        var result = AuctionsEventFactory.Create(previousState, newState).ToList();

        var ev = Assert.Single(result);
        Assert.Equal(EventType.AuctionsChanged, ev.Type);

        var dto = Assert.IsType<AuctionsDTO>(ev.Payload);
        Assert.True(dto.DivineBarrier.HasValue);
        Assert.True(dto.DivineBarrier.Value);
    }

    private static State CreateState(Auctions auctions)
    {
        return new State { Auctions = auctions };
    }
}
