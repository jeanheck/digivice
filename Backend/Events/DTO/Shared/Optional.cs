using System.Text.Json.Serialization;
using Backend.Events.Converters;

namespace Backend.Events.DTO.Shared;

[JsonConverter(typeof(OptionalJsonConverterFactory))]
public readonly struct Optional<T>(T value) : IEquatable<Optional<T>>
{
    public bool HasValue { get; } = true;
    public T? Value { get; } = value;

    public static implicit operator Optional<T>(T value) => new(value);
    public static Optional<T> Empty => default;

    public bool Equals(Optional<T> other)
    {
        if (HasValue != other.HasValue)
        {
            return false;
        }

        return !HasValue || EqualityComparer<T?>.Default.Equals(Value, other.Value);
    }

    public override bool Equals(object? obj) => obj is Optional<T> other && Equals(other);

    public override int GetHashCode() => HasValue ? HashCode.Combine(HasValue, Value) : 0;

    public static bool operator ==(Optional<T> left, Optional<T> right) => left.Equals(right);

    public static bool operator !=(Optional<T> left, Optional<T> right) => !left.Equals(right);
}
