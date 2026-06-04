namespace Tests.Memory.Converters;

using System.Text.Json;
using System.Text.Json.Serialization;
using Backend.Memory.Converters;

public class HexStringListToLongListConverterTests
{
    private record Wrapper([property: JsonConverter(typeof(HexStringListToLongListConverter))] List<long> BitMasks);

    [Fact]
    public void Read_ShouldDeserializeHexStringArray()
    {
        var json = """
            {
                "BitMasks": ["0x01", "0x08"]
            }
            """;

        var result = JsonSerializer.Deserialize<Wrapper>(json);

        Assert.NotNull(result);
        Assert.Equal([0x01, 0x08], result.BitMasks);
    }

    [Fact]
    public void Read_ShouldDeserializeEmptyArray()
    {
        var json = """
            {
                "BitMasks": []
            }
            """;

        var result = JsonSerializer.Deserialize<Wrapper>(json);

        Assert.NotNull(result);
        Assert.Empty(result.BitMasks);
    }

    [Fact]
    public void Write_ShouldSerializeToHexStringArray()
    {
        var wrapper = new Wrapper([0x01, 0x08]);

        var json = JsonSerializer.Serialize(wrapper);

        Assert.Contains("\"BitMasks\":[\"0x1\",\"0x8\"]", json.Replace(" ", ""));
    }
}
