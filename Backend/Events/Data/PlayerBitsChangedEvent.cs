namespace Backend.Events.Data;

public class PlayerBitsChangedEvent : BaseEvent
{
    public int? NewBits { get; }

    public PlayerBitsChangedEvent(int? newBits) : base(EventType.PlayerBitsChanged)
    {
        NewBits = newBits;
    }
}
