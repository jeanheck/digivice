namespace Tests.Events.Converters;

using System.Text.Json;
using Backend.Events.Converters;
using Backend.Events.DTO;
using Backend.Events.DTO.Shared;

public class OptionalJsonConverterFactoryTests
{
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
