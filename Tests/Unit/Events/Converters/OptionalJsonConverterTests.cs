namespace Tests.Events.Converters;

using System.Text.Json;
using Backend.Events.Converters;
using Backend.Events.DTO;
using Backend.Events.DTO.Shared;

public class OptionalJsonConverterTests
{
    [Fact]
    public void Serialize_ShouldWriteOptionalValue_WhenValueIsPresent()
    {
        Optional<int> optional = 42;

        var json = JsonSerializer.Serialize(optional);

        Assert.Equal("42", json);
    }

    [Fact]
    public void Serialize_ShouldOmitOptionalProperties_WhenDtoValuesAreEmpty()
    {
        var dto = new PlayerDTO();

        var json = JsonSerializer.Serialize(dto);

        Assert.Equal("{}", json);
    }

    [Fact]
    public void Serialize_ShouldWriteOptionalProperties_WhenDtoValuesArePresent()
    {
        var dto = new PlayerDTO
        {
            Name = "Agumon",
            Bits = 123,
            Location = "00AF"
        };

        var json = JsonSerializer.Serialize(dto);
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        Assert.Equal("Agumon", root.GetProperty("Name").GetString());
        Assert.Equal(123, root.GetProperty("Bits").GetInt32());
        Assert.Equal("00AF", root.GetProperty("Location").GetString());
    }

    [Fact]
    public void Deserialize_ShouldCreateOptionalWithValue()
    {
        var optional = JsonSerializer.Deserialize<Optional<int>>("42");

        Assert.True(optional.HasValue);
        Assert.Equal(42, optional.Value);
    }

    [Fact]
    public void Deserialize_ShouldPreserveExplicitNullAsPresentValue()
    {
        var optional = JsonSerializer.Deserialize<Optional<string?>>("null");

        Assert.True(optional.HasValue);
        Assert.Null(optional.Value);
    }

    [Fact]
    public void Factory_ShouldOnlyConvertOptionalTypes()
    {
        var factory = new OptionalJsonConverterFactory();

        Assert.True(factory.CanConvert(typeof(Optional<int>)));
        Assert.True(factory.CanConvert(typeof(Optional<string>)));
        Assert.False(factory.CanConvert(typeof(int)));
        Assert.False(factory.CanConvert(typeof(int?)));
        Assert.False(factory.CanConvert(typeof(PlayerDTO)));
    }

    [Fact]
    public void Factory_ShouldCreateConverterForOptionalType()
    {
        var factory = new OptionalJsonConverterFactory();

        var converter = factory.CreateConverter(typeof(Optional<int>), new JsonSerializerOptions());

        Assert.IsType<OptionalJsonConverter<int>>(converter);
    }
}
