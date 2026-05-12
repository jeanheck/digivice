namespace Backend.Events.Data;

public class PlayerNameChangedEvent : BaseEvent
{
    public string NewName { get; }

    public PlayerNameChangedEvent(string newName) : base(EventType.PlayerNameChanged)
    {
        NewName = newName;
    }
}
