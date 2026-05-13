namespace Backend.Events.Data;

public class PlayerLocationChangedEvent : BaseEvent
{
    public string? Location { get; }

    public PlayerLocationChangedEvent(string? location) : base(EventType.PlayerLocationChanged)
    {
        Location = location;
    }
}
