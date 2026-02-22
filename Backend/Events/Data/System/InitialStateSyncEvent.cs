namespace Backend.Events.Data.System;

public class InitialStateSyncEvent : BaseEvent
{
    public Models.Player InitialState { get; }

    public InitialStateSyncEvent(Models.Player initialState) : base(EventType.InitialStateSync)
    {
        InitialState = initialState;
    }
}
