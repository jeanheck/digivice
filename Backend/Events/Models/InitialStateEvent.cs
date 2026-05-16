using Backend.Domain.Models;

namespace Backend.Events.Models;

public class InitialStateEvent(State state) : BaseEvent(EventType.InitialState)
{
    public State State { get; } = state;
}
