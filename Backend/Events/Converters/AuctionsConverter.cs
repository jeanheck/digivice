using Backend.Domain.Models;
using Backend.Events.DTO;

namespace Backend.Events.Converters;

public static class AuctionsConverter
{
    public static AuctionsDTO ToDTO(Auctions auctions)
    {
        return new AuctionsDTO
        {
            DivineBarrier = auctions.DivineBarrier,
            HazardShield = auctions.HazardShield,
            SniperShield = auctions.SniperShield,
            DramonShield = auctions.DramonShield,
            YinYangWand = auctions.YinYangWand
        };
    }
}
