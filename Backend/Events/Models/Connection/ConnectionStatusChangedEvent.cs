namespace Backend.Events.Models.Connection;

public class ConnectionStatusChangedEvent(bool isConnected) : BaseEvent(EventType.ConnectionStatusChanged)
{
    public bool IsConnected { get; } = isConnected;
}
