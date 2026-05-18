using System.Text.Json.Serialization;
using Backend.Events.Converters;

namespace Backend.Events.DTO;

[JsonConverter(typeof(OptionalJsonConverterFactory))]
public readonly struct Optional<T>(T value)
{
    public bool HasValue { get; } = true;
    public T? Value { get; } = value;

    public static implicit operator Optional<T>(T value) => new(value);
    public static Optional<T> Empty => default;
}
