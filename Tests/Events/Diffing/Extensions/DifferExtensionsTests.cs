namespace Tests.Events.Diffing.Extensions;

using Backend.Events.Diffing.Extensions;
using Backend.Domain.Models;

public class DifferExtensionsTests
{
    [Fact]
    public void HasNoChanges_ShouldReturnTrue_WhenNewObjIsNull()
    {
        Player? newObj = null;
        var previousObj = new Player { Name = "Taichi", Bits = 100, MapId = "0001" };

        var result = newObj.HasNoChanges(previousObj);

        Assert.True(result);
    }

    [Fact]
    public void HasNoChanges_ShouldReturnTrue_WhenObjectsAreValueEqual()
    {
        var newObj = new Player { Name = "Taichi", Bits = 100, MapId = "0001" };
        var previousObj = new Player { Name = "Taichi", Bits = 100, MapId = "0001" };

        var result = newObj.HasNoChanges(previousObj);

        Assert.True(result);
    }

    [Fact]
    public void HasNoChanges_ShouldReturnFalse_WhenObjectsAreDifferent()
    {
        var newObj = new Player { Name = "Taichi", Bits = 101, MapId = "0001" };
        var previousObj = new Player { Name = "Taichi", Bits = 100, MapId = "0001" };

        var result = newObj.HasNoChanges(previousObj);

        Assert.False(result);
    }
}
