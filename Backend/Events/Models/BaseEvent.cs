namespace Backend.Events.Models;

public abstract class BaseEvent(Enum type)
{
    public Enum Type { get; } = type;
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}
