namespace Backend.Events.Data;

public class LocationChangedEvent : BaseEvent
{
    public string? Location { get; }

    public LocationChangedEvent(string? location) : base(EventType.LocationChanged)
    {
        Location = location;
    }
}
