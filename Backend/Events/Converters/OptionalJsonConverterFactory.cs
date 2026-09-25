using System.Text.Json;
using System.Text.Json.Serialization;
using Backend.Events.DTO.Shared;

namespace Backend.Events.Converters;

public class OptionalJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Optional<>);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var typeArgument = typeToConvert.GetGenericArguments()[0];
        return (JsonConverter)Activator.CreateInstance(
            typeof(OptionalJsonConverter<>).MakeGenericType(typeArgument)
        )!;
    }
}
