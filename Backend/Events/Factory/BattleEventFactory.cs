using Backend.Domain.Models;
using Backend.Events.Diffing;
using Backend.Events.DTO.Extensions;
using Backend.Events.Models;

namespace Backend.Events.Factory;

public static class BattleEventFactory
{
    public static IEnumerable<Event> Create(State previousState, State newState)
    {
        var dto = BattleDiffer.Diff(previousState.Battle, newState.Battle);

        if (dto.IsNotEmpty())
        {
            return [new Event(EventType.BattleChanged, dto)];
        }

        return [];
    }
}
