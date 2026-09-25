using Backend.Domain.Models;
using Backend.Events.Diffing;
using Backend.Events.DTO.Extensions;
using Backend.Events.Models;

namespace Backend.Events.Factory;

public static class CardBattleEventFactory
{
    public static IEnumerable<Event> Create(State previousState, State newState)
    {
        var dto = CardBattleDiffer.Diff(previousState.CardBattle, newState.CardBattle);

        if (dto.IsNotEmpty())
        {
            return [new Event(EventType.CardBattleChanged, dto)];
        }

        return [];
    }
}
