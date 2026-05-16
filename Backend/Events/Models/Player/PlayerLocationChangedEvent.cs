namespace Backend.Events.Models.Player;

public class PlayerLocationChangedEvent(string? location) : BaseEvent(EventType.PlayerLocationChanged)
{
    public string? Location { get; } = location;
}
