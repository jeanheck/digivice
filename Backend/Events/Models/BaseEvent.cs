namespace Backend.Events.Models;

public abstract class BaseEvent(EventType type)
{
    public EventType Type { get; } = type;
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}
