namespace Backend.Events.Data.System;

public class InitialStateSyncEvent : BaseEvent
{
    public Models.State InitialState { get; }

    public InitialStateSyncEvent(Models.State initialState) : base(EventType.InitialStateSync)
    {
        InitialState = initialState;
    }
}
