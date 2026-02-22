namespace Backend.Events.Data.System;

public class ConnectionStatusChangedEvent : BaseEvent
{
    public bool IsConnected { get; }

    public ConnectionStatusChangedEvent(bool isConnected) : base(EventType.ConnectionStatusChanged)
    {
        IsConnected = isConnected;
    }
}
