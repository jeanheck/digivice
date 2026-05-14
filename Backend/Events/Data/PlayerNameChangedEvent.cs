namespace Backend.Events.Data;

public class PlayerNameChangedEvent(string name) : BaseEvent(EventType.PlayerNameChanged)
{
    public string Name { get; } = name;
}
