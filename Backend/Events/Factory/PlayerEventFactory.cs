using Backend.Domain.Models;
using Backend.Events.Diffing;
using Backend.Events.Models;

namespace Backend.Events.Factory;

public static class PlayerEventFactory
{
    public static IEnumerable<BaseEvent> Create(State previousState, State newState)
    {
        return PlayerDiffer.Diff(previousState.Player, newState.Player);
    }
}
