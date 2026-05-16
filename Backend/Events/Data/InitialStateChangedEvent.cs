using Backend.Domain.Models;

namespace Backend.Events.Data;

public class InitialStateChangedEvent(State state) : BaseEvent(EventType.InitialStateChanged)
{
    public State State { get; } = state;
}
