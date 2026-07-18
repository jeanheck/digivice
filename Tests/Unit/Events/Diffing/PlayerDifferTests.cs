namespace Tests.Events.Diffing;

using Backend.Events.Diffing;
using Backend.Domain.Models;

public class PlayerDifferTests
{
    [Fact]
    public void Diff_ShouldReturnEmptyDTO_WhenNoChanges()
    {
        var previousPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "0001", PreviousMapId = "023E", SeabedRoute = 0x08, MapVariant = 0x01 };
        var newPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "0001", PreviousMapId = "023E", SeabedRoute = 0x08, MapVariant = 0x01 };

        var result = PlayerDiffer.Diff(previousPlayer, newPlayer);

        Assert.NotNull(result);
        Assert.False(result.Name.HasValue);
        Assert.False(result.Bits.HasValue);
        Assert.False(result.Location.HasValue);
        Assert.False(result.PreviousMapId.HasValue);
        Assert.False(result.SeabedRoute.HasValue);
        Assert.False(result.MapVariant.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousPlayerIsNull()
    {
        var newPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "0001", PreviousMapId = "023E", SeabedRoute = 0x08, MapVariant = 0x01 };

        var result = PlayerDiffer.Diff(null, newPlayer);

        Assert.NotNull(result);
        Assert.True(result.Name.HasValue);
        Assert.Equal("Taichi", result.Name.Value);
        Assert.True(result.Bits.HasValue);
        Assert.Equal(100, result.Bits.Value);
        Assert.True(result.Location.HasValue);
        Assert.Equal("0001", result.Location.Value);
        Assert.True(result.PreviousMapId.HasValue);
        Assert.Equal("023E", result.PreviousMapId.Value);
        Assert.True(result.SeabedRoute.HasValue);
        Assert.Equal((byte)0x08, result.SeabedRoute.Value);
        Assert.True(result.MapVariant.HasValue);
        Assert.Equal((byte)0x01, result.MapVariant.Value);
    }

    [Fact]
    public void Diff_ShouldReturnBitsDelta_WhenOnlyBitsChanged()
    {
        var previousPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "0001", PreviousMapId = "023E" };
        var newPlayer = new Player { Name = "Taichi", Bits = 200, MapId = "0001", PreviousMapId = "023E" };

        var result = PlayerDiffer.Diff(previousPlayer, newPlayer);

        Assert.NotNull(result);
        Assert.False(result.Name.HasValue);
        Assert.True(result.Bits.HasValue);
        Assert.Equal(200, result.Bits.Value);
        Assert.False(result.Location.HasValue);
        Assert.False(result.PreviousMapId.HasValue);
        Assert.False(result.SeabedRoute.HasValue);
        Assert.False(result.MapVariant.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnLocationDelta_WhenOnlyMapIdChanged()
    {
        var previousPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "0001", PreviousMapId = "023E" };
        var newPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "0002", PreviousMapId = "023E" };

        var result = PlayerDiffer.Diff(previousPlayer, newPlayer);

        Assert.NotNull(result);
        Assert.False(result.Name.HasValue);
        Assert.False(result.Bits.HasValue);
        Assert.True(result.Location.HasValue);
        Assert.Equal("0002", result.Location.Value);
        Assert.False(result.PreviousMapId.HasValue);
        Assert.False(result.SeabedRoute.HasValue);
        Assert.False(result.MapVariant.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnPreviousMapIdDelta_WhenOnlyPreviousMapIdChanged()
    {
        var previousPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "02E2", PreviousMapId = "023E" };
        var newPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "02E2", PreviousMapId = "02E0" };

        var result = PlayerDiffer.Diff(previousPlayer, newPlayer);

        Assert.True(result.PreviousMapId.HasValue);
        Assert.Equal("02E0", result.PreviousMapId.Value);
        Assert.False(result.Bits.HasValue);
        Assert.False(result.Location.HasValue);
        Assert.False(result.SeabedRoute.HasValue);
        Assert.False(result.MapVariant.HasValue);
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
        var previousPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "0001", SeabedRoute = 0x00, MapVariant = 0x00 };
        var newPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "0001", SeabedRoute = 0x08, MapVariant = 0x00 };

        var result = PlayerDiffer.Diff(previousPlayer, newPlayer);

        Assert.True(result.SeabedRoute.HasValue);
        Assert.Equal((byte)0x08, result.SeabedRoute.Value);
        Assert.False(result.MapVariant.HasValue);
        Assert.False(result.Bits.HasValue);
        Assert.False(result.Location.HasValue);
        Assert.False(result.PreviousMapId.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnMapVariantDelta_WhenOnlyMapVariantChanged()
    {
        var previousPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "0001", SeabedRoute = 0x08, MapVariant = 0x00 };
        var newPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "0001", SeabedRoute = 0x08, MapVariant = 0x01 };

        var result = PlayerDiffer.Diff(previousPlayer, newPlayer);

        Assert.True(result.MapVariant.HasValue);
        Assert.Equal((byte)0x01, result.MapVariant.Value);
        Assert.False(result.SeabedRoute.HasValue);
        Assert.False(result.Bits.HasValue);
        Assert.False(result.Location.HasValue);
        Assert.False(result.PreviousMapId.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnBothSeabedDeltas_WhenSeabedRouteAndMapVariantChanged()
    {
        var previousPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "023E", SeabedRoute = 0x00, MapVariant = 0x00 };
        var newPlayer = new Player { Name = "Taichi", Bits = 100, MapId = "02E2", SeabedRoute = 0x08, MapVariant = 0x01 };

        var result = PlayerDiffer.Diff(previousPlayer, newPlayer);

        Assert.True(result.Location.HasValue);
        Assert.Equal("02E2", result.Location.Value);
        Assert.True(result.SeabedRoute.HasValue);
        Assert.Equal((byte)0x08, result.SeabedRoute.Value);
        Assert.True(result.MapVariant.HasValue);
        Assert.Equal((byte)0x01, result.MapVariant.Value);
    }
}
