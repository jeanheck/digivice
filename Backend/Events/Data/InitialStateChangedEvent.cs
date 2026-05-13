namespace Backend.Events.Data;

public class InitialStateChangedEvent : BaseEvent
{
    public Models.State InitialState { get; }

    public InitialStateChangedEvent(Models.State initialState) : base(EventType.InitialStateChanged)
    {
        InitialState = initialState;
    }
}
