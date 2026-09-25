using System.Text.Json;
using System.Text.Json.Serialization;
using Backend.Events.DTO.Interfaces;

namespace Backend.Events.Converters;

/// <summary>
/// Serializes <see cref="IDTO"/> using the runtime type so SignalR JSON
/// keeps concrete DTO fields (same behavior as typing the property as object).
/// </summary>
public class DtoRuntimeTypeJsonConverter : JsonConverter<IDTO>
{
    public override IDTO Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotSupportedException("Event payload deserialization is not supported; the backend only sends events.");
    }

    public override void Write(Utf8JsonWriter writer, IDTO value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
