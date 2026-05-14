namespace Backend.Events.Data;

public abstract class BaseEvent(EventType type)
{
    public EventType Type { get; } = type;
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}
