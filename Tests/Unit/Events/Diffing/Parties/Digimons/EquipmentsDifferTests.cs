namespace Tests.Events.Diffing.Parties.Digimons;

using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Diffing.Parties.Digimons;

public class EquipmentsDifferTests
{
    [Fact]
    public void Diff_ShouldReturnNull_WhenNoChanges()
    {
        var previous = new Equipments { Head = 1, Body = 1, Right = 1, Left = 1, Accessory1 = 1, Accessory2 = 1 };
        var newObj = new Equipments { Head = 1, Body = 1, Right = 1, Left = 1, Accessory1 = 1, Accessory2 = 1 };

        var result = EquipmentsDiffer.Diff(previous, newObj);

        Assert.Null(result);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var newObj = new Equipments { Head = 101, Body = 102, Right = 103, Left = 104, Accessory1 = 105, Accessory2 = 106 };

        var result = EquipmentsDiffer.Diff(null, newObj);

        Assert.NotNull(result);
        Assert.True(result.Head.HasValue);
        Assert.Equal(101, result.Head.Value);
        Assert.True(result.Body.HasValue);
        Assert.Equal(102, result.Body.Value);
        Assert.True(result.Right.HasValue);
        Assert.Equal(103, result.Right.Value);
        Assert.True(result.Left.HasValue);
        Assert.Equal(104, result.Left.Value);
        Assert.True(result.Accessory1.HasValue);
        Assert.Equal(105, result.Accessory1.Value);
        Assert.True(result.Accessory2.HasValue);
        Assert.Equal(106, result.Accessory2.Value);
    }

    [Fact]
    public void Diff_ShouldReturnOnlyChangedFields_WhenPartialChanges()
    {
        var previous = new Equipments { Head = 1, Body = 1, Right = 1, Left = 1, Accessory1 = 1, Accessory2 = 1 };
        var newObj = new Equipments { Head = 1, Body = 200, Right = 1, Left = 1, Accessory1 = 205, Accessory2 = 1 };

        var result = EquipmentsDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.False(result.Head.HasValue);
        Assert.True(result.Body.HasValue);
        Assert.Equal(200, result.Body.Value);
        Assert.False(result.Right.HasValue);
        Assert.False(result.Left.HasValue);
        Assert.True(result.Accessory1.HasValue);
        Assert.Equal(205, result.Accessory1.Value);
        Assert.False(result.Accessory2.HasValue);
    }
}
