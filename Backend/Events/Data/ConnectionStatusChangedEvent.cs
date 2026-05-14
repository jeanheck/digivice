namespace Backend.Events.Data;

public class ConnectionStatusChangedEvent(bool isConnected) : BaseEvent(EventType.ConnectionStatusChanged)
{
    public bool IsConnected { get; } = isConnected;
}
