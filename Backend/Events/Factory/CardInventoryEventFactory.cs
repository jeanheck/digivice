using Backend.Domain.Models;
using Backend.Events.Diffing;
using Backend.Events.Models;

namespace Backend.Events.Factory;

public static class CardInventoryEventFactory
{
    public static List<Event> Create(State? previousState, State newState)
    {
        var dto = CardInventoryDiffer.Diff(previousState?.CardInventory, newState.CardInventory);
        if (dto.Cards.HasValue)
        {
            return [new Event(EventType.CardInventoryChanged, dto)];
        }

        return [];
    }
}
