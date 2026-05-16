namespace Backend.Events.Models.Player;

public class PlayerNameChangedEvent(string name) : BaseEvent(EventType.PlayerNameChanged)
{
    public string Name { get; } = name;
}
