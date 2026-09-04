using Backend.Domain.Models;
using Backend.Events.Diffing;
using Backend.Events.DTO.Extensions;
using Backend.Events.Models;

namespace Backend.Events.Factory;

public static class AuctionsEventFactory
{
    public static IEnumerable<Event> Create(State previousState, State newState)
    {
        var dto = AuctionsDiffer.Diff(previousState.Auctions, newState.Auctions);

        if (dto.IsNotEmpty())
        {
            return [new Event(EventType.AuctionsChanged, dto)];
        }

        return [];
    }
}
