namespace Backend.Events.Models;

public class ConnectionStatusChangedEvent(bool isConnected) : BaseEvent(EventType.ConnectionStatusChanged)
{
    public bool IsConnected { get; } = isConnected;
}
