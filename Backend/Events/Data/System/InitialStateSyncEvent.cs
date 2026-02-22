using Backend.Models;

namespace Backend.Events.Data.System;

public class InitialStateSyncEvent : BaseEvent
{
    public Backend.Models.Player InitialState { get; }

    public InitialStateSyncEvent(Backend.Models.Player initialState) : base(EventType.InitialStateSync)
    {
        InitialState = initialState;
    }
}
