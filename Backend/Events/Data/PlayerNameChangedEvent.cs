namespace Backend.Events.Data;

public class PlayerNameChangedEvent(string newName) : BaseEvent(EventType.PlayerNameChanged)
{
    public string NewName { get; } = newName;
}
