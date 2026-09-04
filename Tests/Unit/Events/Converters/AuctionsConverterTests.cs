namespace Tests.Events.Converters;

using Backend.Domain.Models;
using Backend.Events.Converters;

public class AuctionsConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapAllAuctionFields()
    {
        var auctions = new Auctions
        {
            DivineBarrier = true,
            HazardShield = false,
            SniperShield = true,
            DramonShield = false,
            YinYangWand = true,
        };

        var dto = AuctionsConverter.ToDTO(auctions);

        Assert.True(dto.DivineBarrier.HasValue);
        Assert.True(dto.DivineBarrier.Value);
        Assert.True(dto.HazardShield.HasValue);
        Assert.False(dto.HazardShield.Value);
        Assert.True(dto.SniperShield.HasValue);
        Assert.True(dto.SniperShield.Value);
        Assert.True(dto.DramonShield.HasValue);
        Assert.False(dto.DramonShield.Value);
        Assert.True(dto.YinYangWand.HasValue);
        Assert.True(dto.YinYangWand.Value);
    }
}
