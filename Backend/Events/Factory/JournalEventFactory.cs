using Backend.Domain.Models;
using Backend.Events.Diffing;
using Backend.Events.DTO.Extensions;
using Backend.Events.Models;

namespace Backend.Events.Factory;

public static class JournalEventFactory
{
    public static IEnumerable<Event> Create(State previousState, State newState)
    {
        var dto = JournalDiffer.Diff(previousState.Journal, newState.Journal);

        if (dto.IsNotEmpty())
        {
            return [new Event(EventType.JournalChanged, dto)];
        }

        return [];
    }
}
