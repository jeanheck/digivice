using Backend.Events.Types;

namespace Backend.Events.Models.Player;

public class PlayerChangedEvent(PlayerDTO player) : BaseEvent(PlayerEvent.Changed)
{
    public PlayerDTO Player { get; } = player;
}
