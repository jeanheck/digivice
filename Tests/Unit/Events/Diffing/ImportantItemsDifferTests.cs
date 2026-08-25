namespace Tests.Events.Diffing;

using Backend.Domain.Models;
using Backend.Events.Diffing;

public class ImportantItemsDifferTests
{
    [Fact]
    public void Diff_ShouldReturnEmptyDTO_WhenNoChanges()
    {
        var previous = new ImportantItems { TreeBoots = true, FishingPole = false, AsukaTrophy = true };
        var current = new ImportantItems { TreeBoots = true, FishingPole = false, AsukaTrophy = true };

        var result = ImportantItemsDiffer.Diff(previous, current);

        Assert.False(result.TreeBoots.HasValue);
        Assert.False(result.FishingPole.HasValue);
        Assert.False(result.AsukaTrophy.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var current = new ImportantItems { TreeBoots = true, FishingPole = false, AsukaTrophy = true };

        var result = ImportantItemsDiffer.Diff(null, current);

        Assert.True(result.TreeBoots.HasValue);
        Assert.True(result.TreeBoots.Value);
        Assert.True(result.FishingPole.HasValue);
        Assert.False(result.FishingPole.Value);
        Assert.True(result.AsukaTrophy.HasValue);
        Assert.True(result.AsukaTrophy.Value);
    }

    [Fact]
    public void Diff_ShouldReturnTreeBootsDelta_WhenOnlyTreeBootsChanged()
    {
        var previous = new ImportantItems { TreeBoots = false, FishingPole = true, AsukaTrophy = true };
        var current = new ImportantItems { TreeBoots = true, FishingPole = true, AsukaTrophy = true };

        var result = ImportantItemsDiffer.Diff(previous, current);

        Assert.True(result.TreeBoots.HasValue);
        Assert.True(result.TreeBoots.Value);
        Assert.False(result.FishingPole.HasValue);
        Assert.False(result.AsukaTrophy.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnFalseDelta_WhenItemIsLost()
    {
        var previous = new ImportantItems { TreeBoots = true, FishingPole = false, AsukaTrophy = false };
        var current = new ImportantItems { TreeBoots = false, FishingPole = false, AsukaTrophy = false };

        var result = ImportantItemsDiffer.Diff(previous, current);

        Assert.True(result.TreeBoots.HasValue);
        Assert.False(result.TreeBoots.Value);
        Assert.False(result.FishingPole.HasValue);
        Assert.False(result.AsukaTrophy.HasValue);
    }
}
