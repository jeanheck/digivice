namespace Tests.Events.Diffing;

using Backend.Domain.Models;
using Backend.Events.Diffing;

public class AuctionsDifferTests
{
    [Fact]
    public void Diff_ShouldReturnEmptyDTO_WhenNoChanges()
    {
        var previous = CreateBaseAuctions();
        var current = CreateBaseAuctions();

        var result = AuctionsDiffer.Diff(previous, current);

        Assert.False(result.DivineBarrier.HasValue);
        Assert.False(result.HazardShield.HasValue);
        Assert.False(result.SniperShield.HasValue);
        Assert.False(result.DramonShield.HasValue);
        Assert.False(result.YinYangWand.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var current = CreateBaseAuctions();
        current.DivineBarrier = true;

        var result = AuctionsDiffer.Diff(null, current);

        Assert.True(result.DivineBarrier.HasValue);
        Assert.True(result.DivineBarrier.Value);
        Assert.True(result.HazardShield.HasValue);
        Assert.False(result.HazardShield.Value);
    }

    [Fact]
    public void Diff_ShouldReturnDivineBarrierDelta_WhenOnlyDivineBarrierChanged()
    {
        var previous = CreateBaseAuctions();
        var current = CreateBaseAuctions();
        current.DivineBarrier = true;

        var result = AuctionsDiffer.Diff(previous, current);

        Assert.True(result.DivineBarrier.HasValue);
        Assert.True(result.DivineBarrier.Value);
        Assert.False(result.HazardShield.HasValue);
    }

    private static Auctions CreateBaseAuctions()
    {
        return new Auctions
        {
            DivineBarrier = false,
            HazardShield = false,
            SniperShield = false,
            DramonShield = false,
            YinYangWand = false,
        };
    }
}
