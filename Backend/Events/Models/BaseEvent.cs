using Backend.Events.DTO.Interfaces;

namespace Backend.Events.Models;

public class BaseEvent(Enum type, IDTO data)
{
    public Enum Type { get; } = type;
    public DateTime Timestamp { get; } = DateTime.UtcNow;
    public IDTO Data { get; } = data;
}
