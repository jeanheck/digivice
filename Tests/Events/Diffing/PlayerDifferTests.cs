namespace Tests.Events.Diffing;

using Backend.Events.Diffing;
using Backend.Domain.Models;

public class PlayerDifferTests
{
    [Fact]
    public void Diff_ShouldReturnEmptyDTO_WhenNoChanges()
    {
        var previousPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "0001" };
        var newPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "0001" };

        var result = PlayerDiffer.Diff(previousPlayer, newPlayer);

        Assert.NotNull(result);
        Assert.False(result.Name.HasValue);
        Assert.False(result.Bits.HasValue);
        Assert.False(result.Location.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousPlayerIsNull()
    {
        var newPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "0001" };

        var result = PlayerDiffer.Diff(null, newPlayer);

        Assert.NotNull(result);
        Assert.True(result.Name.HasValue);
        Assert.Equal("Taichi", result.Name.Value);
        Assert.True(result.Bits.HasValue);
        Assert.Equal(100, result.Bits.Value);
        Assert.True(result.Location.HasValue);
        Assert.Equal("0001", result.Location.Value);
    }

    [Fact]
    public void Diff_ShouldReturnBitsDelta_WhenOnlyBitsChanged()
    {
        var previousPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "0001" };
        var newPlayer = new Player { Name = "Taichi", Bits = 200, MapId = "0001" };

        var result = PlayerDiffer.Diff(previousPlayer, newPlayer);

        Assert.NotNull(result);
        Assert.False(result.Name.HasValue);
        Assert.True(result.Bits.HasValue);
        Assert.Equal(200, result.Bits.Value);
        Assert.False(result.Location.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnLocationDelta_WhenOnlyMapIdChanged()
    {
        var previousPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "0001" };
        var newPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "0002" };

        var result = PlayerDiffer.Diff(previousPlayer, newPlayer);

        Assert.NotNull(result);
        Assert.False(result.Name.HasValue);
        Assert.False(result.Bits.HasValue);
        Assert.True(result.Location.HasValue);
        Assert.Equal("0002", result.Location.Value);
    }

    [Fact]
    public void Diff_ShouldReturnMultipleDeltas_WhenBitsAndMapIdChanged()
    {
        var previousPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "0001" };
        var newPlayer = new Player { Name = "Taichi", Bits = 200, MapId = "0002" };

        var result = PlayerDiffer.Diff(previousPlayer, newPlayer);

        Assert.NotNull(result);
        Assert.False(result.Name.HasValue);
        Assert.True(result.Bits.HasValue);
        Assert.Equal(200, result.Bits.Value);
        Assert.True(result.Location.HasValue);
        Assert.Equal("0002", result.Location.Value);
    }
}
