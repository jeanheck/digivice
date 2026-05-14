namespace Backend.Events.Data;

public class PlayerBitsChangedEvent(int? bits) : BaseEvent(EventType.PlayerBitsChanged)
{
    public int? Bits { get; } = bits;
}
