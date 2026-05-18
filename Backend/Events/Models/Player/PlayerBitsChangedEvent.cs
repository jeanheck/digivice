using Backend.Events.Types;

namespace Backend.Events.Models.Player;

public class PlayerBitsChangedEvent(int bits) : BaseEvent(PlayerEvent.BitsChanged)
{
    public int Bits { get; } = bits;
}
