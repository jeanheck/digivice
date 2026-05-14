using Backend.Models;

namespace Backend.Events.Data;

public class InitialStateChangedEvent : BaseEvent
{
    public State InitialState { get; }

    public InitialStateChangedEvent(State initialState) : base(EventType.InitialStateChanged)
    {
        InitialState = initialState;
    }
}
