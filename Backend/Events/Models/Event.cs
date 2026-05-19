using Backend.Events.DTO.Interfaces;

namespace Backend.Events.Models;

public class Event(Enum type, IDTO payload)
{
    public Enum Type { get; } = type;
    public DateTime Timestamp { get; } = DateTime.UtcNow;
    public IDTO Payload { get; } = payload;
}
