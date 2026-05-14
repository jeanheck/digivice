namespace Backend.Events.Data;

public class PlayerLocationChangedEvent(string? location) : BaseEvent(EventType.PlayerLocationChanged)
{
    public string? Location { get; } = location;
}
