using System;

namespace Backend.Events.Data;

public abstract class BaseEvent
{
    public EventType Type { get; }
    public DateTime Timestamp { get; }

    protected BaseEvent(EventType type)
    {
        Type = type;
        Timestamp = DateTime.UtcNow;
    }
}
