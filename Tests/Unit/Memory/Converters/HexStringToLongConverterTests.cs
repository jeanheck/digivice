namespace Tests.Memory.Converters;

using System.Text.Json;
using System.Text.Json.Serialization;
using Backend.Memory.Converters;

public class HexStringToLongConverterTests
{
    private record Wrapper([property: JsonConverter(typeof(HexStringToLongConverter))] long Value);

    [Theory]
    [InlineData("\"0x7FFA20\"", 8387104L)]
    [InlineData("\"8387104\"", 8387104L)]
    [InlineData("\"-5\"", -5L)]
    [InlineData("\"\"", 0L)]
    [InlineData("null", 0L)]
    [InlineData("\"invalid\"", 0L)]
    public void Read_ShouldCorrectlyParseJsonValues(string jsonValue, long expected)
    {
        var json = $"{{\"Value\": {jsonValue}}}";

        var result = JsonSerializer.Deserialize<Wrapper>(json);

        Assert.NotNull(result);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void Write_ShouldSerializeToHexadecimalString()
    {
        var wrapper = new Wrapper(8387104L);

        var json = JsonSerializer.Serialize(wrapper);

        Assert.Contains("\"Value\":\"0x7FFA20\"", json);
    }
}
