namespace Tests.Events.DTO.Shared;

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
}
