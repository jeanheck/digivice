using Backend.Domain.Models;
using Backend.Events.Converters;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO.Auctions;

namespace Backend.Events.Diffing;

public static class AuctionDiffer
{
    public static AuctionDTO? Diff(Auction? previousAuction, Auction newAuction)
    {
        if (newAuction.HasNoChanges(previousAuction))
        {
            return null;
        }

        if (previousAuction == null)
        {
            return AuctionConverter.ToDTO(newAuction);
        }

        if (previousAuction.Value == newAuction.Value)
        {
            return null;
        }

        return new AuctionDTO
        {
            Id = newAuction.Id,
            Value = newAuction.Value,
        };
    }
}
