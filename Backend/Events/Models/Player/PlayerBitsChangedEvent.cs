namespace Backend.Events.Models.Player;

public class PlayerBitsChangedEvent(int? bits) : BaseEvent(EventType.PlayerBitsChanged)
{
    public int? Bits { get; } = bits;
}
