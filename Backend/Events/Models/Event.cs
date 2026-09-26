using System.Text.Json.Serialization;
using Backend.Events.Converters;
using Backend.Events.DTO.Interfaces;

namespace Backend.Events.Models;

public class Event(EventType type, IDTO payload)
{
    public EventType Type { get; } = type;
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    [JsonConverter(typeof(DtoRuntimeTypeJsonConverter))]
    public IDTO Payload { get; } = payload;
}
