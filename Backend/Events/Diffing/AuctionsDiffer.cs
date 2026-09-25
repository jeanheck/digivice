using Backend.Domain.Models;
using Backend.Events.Converters;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO;

namespace Backend.Events.Diffing;

public static class AuctionsDiffer
{
    public static AuctionsDTO Diff(Auctions? previousAuctions, Auctions newAuctions)
    {
        if (newAuctions.HasNoChanges(previousAuctions))
        {
            return new AuctionsDTO();
        }

        if (previousAuctions == null)
        {
            return AuctionsConverter.ToDTO(newAuctions);
        }

        var dto = new AuctionsDTO();
        if (newAuctions.DivineBarrier != previousAuctions.DivineBarrier)
        {
            dto = dto with { DivineBarrier = newAuctions.DivineBarrier };
        }
        if (newAuctions.HazardShield != previousAuctions.HazardShield)
        {
            dto = dto with { HazardShield = newAuctions.HazardShield };
        }
        if (newAuctions.SniperShield != previousAuctions.SniperShield)
        {
            dto = dto with { SniperShield = newAuctions.SniperShield };
        }
        if (newAuctions.DramonShield != previousAuctions.DramonShield)
        {
            dto = dto with { DramonShield = newAuctions.DramonShield };
        }
        if (newAuctions.YinYangWand != previousAuctions.YinYangWand)
        {
            dto = dto with { YinYangWand = newAuctions.YinYangWand };
        }

        return dto;
    }
}
