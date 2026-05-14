namespace Backend.Events.Data;

public class PlayerBitsChangedEvent(int? newBits) : BaseEvent(EventType.PlayerBitsChanged)
{
    public int? NewBits { get; } = newBits;
}
