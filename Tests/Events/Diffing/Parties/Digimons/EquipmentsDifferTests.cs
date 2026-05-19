namespace Tests.Events.Diffing.Parties.Digimons;

using Backend.Events.Diffing.Parties.Digimons;
using Backend.Domain.Models.Parties.Digimons;

public class EquipmentsDifferTests
{
    [Fact]
    public void Diff_ShouldReturnNull_WhenNoChanges()
    {
        var previous = new Equipments { Head = 1, Body = 1, RightHand = 1, LeftHand = 1, Accessory1 = 1, Accessory2 = 1 };
        var newObj = new Equipments { Head = 1, Body = 1, RightHand = 1, LeftHand = 1, Accessory1 = 1, Accessory2 = 1 };

        var result = EquipmentsDiffer.Diff(previous, newObj);

        Assert.Null(result);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var newObj = new Equipments { Head = 101, Body = 102, RightHand = 103, LeftHand = 104, Accessory1 = 105, Accessory2 = 106 };

        var result = EquipmentsDiffer.Diff(null, newObj);

        Assert.NotNull(result);
        Assert.True(result.Head.HasValue);
        Assert.Equal(101, result.Head.Value);
        Assert.True(result.Body.HasValue);
        Assert.Equal(102, result.Body.Value);
        Assert.True(result.RightHand.HasValue);
        Assert.Equal(103, result.RightHand.Value);
        Assert.True(result.LeftHand.HasValue);
        Assert.Equal(104, result.LeftHand.Value);
        Assert.True(result.Accessory1.HasValue);
        Assert.Equal(105, result.Accessory1.Value);
        Assert.True(result.Accessory2.HasValue);
        Assert.Equal(106, result.Accessory2.Value);
    }

    [Fact]
    public void Diff_ShouldReturnOnlyChangedFields_WhenPartialChanges()
    {
        var previous = new Equipments { Head = 1, Body = 1, RightHand = 1, LeftHand = 1, Accessory1 = 1, Accessory2 = 1 };
        var newObj = new Equipments { Head = 1, Body = 200, RightHand = 1, LeftHand = 1, Accessory1 = 205, Accessory2 = 1 };

        var result = EquipmentsDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.False(result.Head.HasValue);
        Assert.True(result.Body.HasValue);
        Assert.Equal(200, result.Body.Value);
        Assert.False(result.RightHand.HasValue);
        Assert.False(result.LeftHand.HasValue);
        Assert.True(result.Accessory1.HasValue);
        Assert.Equal(205, result.Accessory1.Value);
        Assert.False(result.Accessory2.HasValue);
    }
}
