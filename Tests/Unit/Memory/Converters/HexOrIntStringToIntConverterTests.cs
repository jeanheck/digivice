namespace Tests.Memory.Converters;

using System.Text.Json;
using System.Text.Json.Serialization;
using Backend.Memory.Converters;

public class HexOrIntStringToIntConverterTests
{
    private record Wrapper([property: JsonConverter(typeof(HexOrIntStringToIntConverter))] int Value);

    [Theory]
    [InlineData("20", 20)]
    [InlineData("\"20\"", 20)]
    [InlineData("\"0x14\"", 20)]
    [InlineData("\"-5\"", -5)]
    [InlineData("\"\"", 0)]
    [InlineData("null", 0)]
    [InlineData("\"invalid\"", 0)]
    public void Read_ShouldCorrectlyParseJsonValues(string jsonValue, int expected)
    {
        var json = $"{{\"Value\": {jsonValue}}}";

        var result = JsonSerializer.Deserialize<Wrapper>(json);

        Assert.NotNull(result);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void Write_ShouldSerializeToNumericValue()
    {
        var wrapper = new Wrapper(20);

        var json = JsonSerializer.Serialize(wrapper);

        Assert.Contains("\"Value\":20", json);
    }
}
