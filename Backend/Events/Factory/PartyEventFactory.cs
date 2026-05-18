using Backend.Domain.Models;
using Backend.Events.Diffing;
using Backend.Events.Models;

namespace Backend.Events.Factory;

public static class PartyEventFactory
{
    public static IEnumerable<BaseEvent> Create(State previousState, State newState)
    {
        return PartyDiffer.Diff(previousState.Party, newState.Party);
    }
}
