using Backend.Domain.Models;
using Backend.Events.Diffing;
using Backend.Events.DTO.Extensions;
using Backend.Events.Models;

namespace Backend.Events.Factory;

public static class PlayerEventFactory
{
    public static IEnumerable<Event> Create(State previousState, State newState)
    {
        var dto = PlayerDiffer.Diff(previousState.Player, newState.Player);

        if (dto.IsNotEmpty())
        {
            return [new Event(EventType.PlayerChanged, dto)];
        }

        return [];
    }
}
