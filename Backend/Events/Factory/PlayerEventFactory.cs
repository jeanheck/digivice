using Backend.Domain.Models;
using Backend.Events.Diffing;
using Backend.Events.DTO.Extensions;
using Backend.Events.Models;
using Backend.Events.Types;

namespace Backend.Events.Factory;

public static class PlayerEventFactory
{
    public static IEnumerable<BaseEvent> Create(State previousState, State newState)
    {
        var dto = PlayerDiffer.Diff(previousState.Player, newState.Player);

        if (dto.IsNotEmpty())
        {
            return [new BaseEvent(PlayerEvent.PlayerChanged, dto)];
        }

        return [];
    }
}
