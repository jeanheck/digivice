using Backend.Domain.Models;
using Backend.Events.Diffing;
using Backend.Events.DTO.Extensions;
using Backend.Events.Models;
using Backend.Events.Types;

namespace Backend.Events.Factory;

public static class PartyEventFactory
{
    public static IEnumerable<BaseEvent> Create(State previousState, State newState)
    {
        var dto = PartyDiffer.Diff(previousState.Party, newState.Party);

        if (dto.IsNotEmpty())
        {
            return [new BaseEvent(PartyEvent.DigimonsChanged, dto)];
        }

        return [];
    }
}
