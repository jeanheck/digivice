namespace Tests.Events.DTO.Shared;

using Backend.Events.DTO;
using Backend.Events.DTO.Extensions;
using Backend.Events.DTO.Shared;

public class OptionalTests
{
    [Fact]
    public void Empty_ShouldNotHaveValue()
    {
        var optional = Optional<int>.Empty;

        Assert.False(optional.HasValue);
        Assert.Equal(0, optional.Value);
    }

    [Fact]
    public void ImplicitConversion_ShouldCreateOptionalWithValue()
    {
        Optional<string> optional = "Agumon";

        Assert.True(optional.HasValue);
        Assert.Equal("Agumon", optional.Value);
    }

    [Fact]
    public void ImplicitConversion_ShouldPreserveExplicitNullAsValue()
    {
        Optional<string?> optional = null;

        Assert.True(optional.HasValue);
        Assert.Null(optional.Value);
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenBothAreEmpty()
    {
        Assert.True(Optional<int>.Empty.Equals(Optional<int>.Empty));
        Assert.True(Optional<int>.Empty == Optional<int>.Empty);
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenBothHaveSameValue()
    {
        Optional<int> left = 200;
        Optional<int> right = 200;

        Assert.True(left.Equals(right));
        Assert.True(left == right);
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenEmptyComparedToPresentDefault()
    {
        Optional<int> presentZero = 0;

        Assert.False(Optional<int>.Empty.Equals(presentZero));
        Assert.True(Optional<int>.Empty != presentZero);
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenEmptyComparedToPresentNull()
    {
        Optional<string?> presentNull = null;

        Assert.False(Optional<string?>.Empty.Equals(presentNull));
        Assert.True(Optional<string?>.Empty != presentNull);
    }

    [Fact]
    public void EqualityComparer_ShouldMatchTypedEquals()
    {
        var comparer = EqualityComparer<Optional<int>>.Default;

        Assert.True(comparer.Equals(Optional<int>.Empty, Optional<int>.Empty));
        Assert.True(comparer.Equals(200, 200));
        Assert.False(comparer.Equals(Optional<int>.Empty, 0));
    }

    [Fact]
    public void PlayerDto_IsEmpty_ShouldRespectOptionalSemantics()
    {
        Assert.True(new PlayerDTO().IsEmpty());
        Assert.True(new PlayerDTO { Bits = 200 }.IsNotEmpty());
    }
}
