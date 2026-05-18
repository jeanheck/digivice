using Backend.Events.Types;

namespace Backend.Events.Models.Connection;

public class ConnectionStatusChangedEvent(bool isConnected) : BaseEvent(ConnectionType.StatusChanged)
{
    public bool IsConnected { get; } = isConnected;
}
