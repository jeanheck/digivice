using Backend.Domain.Models;
using Backend.Events.Diffing;
using Backend.Events.DTO.Extensions;
using Backend.Events.Models;
using Backend.Events.Types;

namespace Backend.Events.Factory;

public static class JournalEventFactory
{
    public static IEnumerable<BaseEvent> Create(State previousState, State newState)
    {
        var dto = JournalDiffer.Diff(previousState.Journal, newState.Journal);

        if (dto.IsNotEmpty())
        {
            return [new BaseEvent(JournalEvent.JournalChanged, dto)];
        }

        return [];
    }
}
