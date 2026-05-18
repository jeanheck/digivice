using Backend.Events.Types;

namespace Backend.Events.Models.Player;

public class PlayerNameChangedEvent(string name) : BaseEvent(PlayerEvent.NameChanged)
{
    public string Name { get; } = name;
}
