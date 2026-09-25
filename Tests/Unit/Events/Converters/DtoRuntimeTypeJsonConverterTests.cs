namespace Tests.Events.Converters;

using System.Text.Json;
using Backend.Events.DTO;
using Backend.Events.DTO.Interfaces;
using Backend.Events.Models;

public class DtoRuntimeTypeJsonConverterTests
{
    [Fact]
    public void Serialize_Event_ShouldWriteConcreteDtoFields_WhenPayloadIsTypedAsIdto()
    {
        var ev = new Event(
            EventType.PlayerChanged,
            new PlayerDTO
            {
                Bits = 200,
                Location = "00AF"
            });

        var json = JsonSerializer.Serialize(ev);
        using var document = JsonDocument.Parse(json);
        var payload = document.RootElement.GetProperty("Payload");

        Assert.Equal(JsonValueKind.Object, payload.ValueKind);
        Assert.True(payload.EnumerateObject().Any(), "Payload must not serialize as empty object {}");
        Assert.Equal(200, payload.GetProperty("Bits").GetInt32());
        Assert.Equal("00AF", payload.GetProperty("Location").GetString());
    }

    [Fact]
    public void Serialize_IdtoWithoutRuntimeConverter_ShouldWriteEmptyObject()
    {
        var box = new IdtoBox(new PlayerDTO { Bits = 200 });

        var json = JsonSerializer.Serialize(box);
        using var document = JsonDocument.Parse(json);
        var payload = document.RootElement.GetProperty("Payload");

        Assert.Equal(JsonValueKind.Object, payload.ValueKind);
        Assert.False(payload.EnumerateObject().Any());
    }

    private sealed class IdtoBox(IDTO payload)
    {
        public IDTO Payload { get; } = payload;
    }
}
