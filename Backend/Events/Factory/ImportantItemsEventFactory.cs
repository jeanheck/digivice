using Backend.Domain.Models;
using Backend.Events.Diffing;
using Backend.Events.DTO.Extensions;
using Backend.Events.Models;

namespace Backend.Events.Factory;

public static class ImportantItemsEventFactory
{
    public static IEnumerable<Event> Create(State previousState, State newState)
    {
        var dto = ImportantItemsDiffer.Diff(previousState.ImportantItems, newState.ImportantItems);

        if (dto.IsNotEmpty())
        {
            return [new Event(EventType.ImportantItemsChanged, dto)];
        }

        return [];
    }
}
