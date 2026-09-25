using Backend.Domain.Models;
using Backend.Events.Diffing;
using Backend.Events.DTO.Extensions;
using Backend.Events.Models;

namespace Backend.Events.Factory;

public static class DigimonBattleEventFactory
{
    public static IEnumerable<Event> Create(State previousState, State newState)
    {
        var dto = DigimonBattleDiffer.Diff(previousState.DigimonBattle, newState.DigimonBattle);

        if (dto.IsNotEmpty())
        {
            return [new Event(EventType.DigimonBattleChanged, dto)];
        }

        return [];
    }
}
