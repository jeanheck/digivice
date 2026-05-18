using Backend.Events.Types;

namespace Backend.Events.Models.Player;

public class PlayerLocationChangedEvent(string? location) : BaseEvent(PlayerEvent.LocationChanged)
{
    public string? Location { get; } = location;
}
