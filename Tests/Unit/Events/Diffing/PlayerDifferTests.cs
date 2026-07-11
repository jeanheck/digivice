namespace Tests.Events.Diffing;

using Backend.Events.Diffing;
using Backend.Domain.Models;

public class PlayerDifferTests
{
    [Fact]
    public void Diff_ShouldReturnEmptyDTO_WhenNoChanges()
    {
        var previousPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "0001", SeabedRoute = 0x08, IsSubmerged = true };
        var newPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "0001", SeabedRoute = 0x08, IsSubmerged = true };

        var result = PlayerDiffer.Diff(previousPlayer, newPlayer);

        Assert.NotNull(result);
        Assert.False(result.Name.HasValue);
        Assert.False(result.Bits.HasValue);
        Assert.False(result.Location.HasValue);
        Assert.False(result.SeabedRoute.HasValue);
        Assert.False(result.IsSubmerged.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousPlayerIsNull()
    {
        var newPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "0001", SeabedRoute = 0x08, IsSubmerged = true };

        var result = PlayerDiffer.Diff(null, newPlayer);

        Assert.NotNull(result);
        Assert.True(result.Name.HasValue);
        Assert.Equal("Taichi", result.Name.Value);
        Assert.True(result.Bits.HasValue);
        Assert.Equal(100, result.Bits.Value);
        Assert.True(result.Location.HasValue);
        Assert.Equal("0001", result.Location.Value);
        Assert.True(result.SeabedRoute.HasValue);
        Assert.Equal((byte)0x08, result.SeabedRoute.Value);
        Assert.True(result.IsSubmerged.HasValue);
        Assert.True(result.IsSubmerged.Value);
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
        Assert.False(result.SeabedRoute.HasValue);
        Assert.False(result.IsSubmerged.HasValue);
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
        Assert.False(result.SeabedRoute.HasValue);
        Assert.False(result.IsSubmerged.HasValue);
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

    [Fact]
    public void Diff_ShouldReturnSeabedRouteDelta_WhenOnlySeabedRouteChanged()
    {
        var previousPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "0001", SeabedRoute = 0x00, IsSubmerged = false };
        var newPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "0001", SeabedRoute = 0x08, IsSubmerged = false };

        var result = PlayerDiffer.Diff(previousPlayer, newPlayer);

        Assert.True(result.SeabedRoute.HasValue);
        Assert.Equal((byte)0x08, result.SeabedRoute.Value);
        Assert.False(result.IsSubmerged.HasValue);
        Assert.False(result.Bits.HasValue);
        Assert.False(result.Location.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnIsSubmergedDelta_WhenOnlyIsSubmergedChanged()
    {
        var previousPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "0001", SeabedRoute = 0x08, IsSubmerged = false };
        var newPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "0001", SeabedRoute = 0x08, IsSubmerged = true };

        var result = PlayerDiffer.Diff(previousPlayer, newPlayer);

        Assert.True(result.IsSubmerged.HasValue);
        Assert.True(result.IsSubmerged.Value);
        Assert.False(result.SeabedRoute.HasValue);
        Assert.False(result.Bits.HasValue);
        Assert.False(result.Location.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnBothSeabedDeltas_WhenSeabedRouteAndIsSubmergedChanged()
    {
        var previousPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "023E", SeabedRoute = 0x00, IsSubmerged = false };
        var newPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "02E2", SeabedRoute = 0x08, IsSubmerged = true };

        var result = PlayerDiffer.Diff(previousPlayer, newPlayer);

        Assert.True(result.Location.HasValue);
        Assert.Equal("02E2", result.Location.Value);
        Assert.True(result.SeabedRoute.HasValue);
        Assert.Equal((byte)0x08, result.SeabedRoute.Value);
        Assert.True(result.IsSubmerged.HasValue);
        Assert.True(result.IsSubmerged.Value);
    }
}
