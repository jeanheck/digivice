using Backend.Domain.Models;
using Backend.Events.Diffing;
using Backend.Events.DTO.Extensions;
using Backend.Events.Models;

namespace Backend.Events.Factory;

public static class PartyEventFactory
{
    public static IEnumerable<Event> Create(State previousState, State newState)
    {
        var dto = PartyDiffer.Diff(previousState.Party, newState.Party);

        if (dto.IsNotEmpty())
        {
            return [new Event(EventType.PartyChanged, dto)];
        }

        return [];
    }
}
