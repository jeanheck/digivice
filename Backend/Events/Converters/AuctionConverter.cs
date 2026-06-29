using Backend.Domain.Models;
using Backend.Events.DTO.Auctions;

namespace Backend.Events.Converters;

public static class AuctionConverter
{
    public static AuctionDTO ToDTO(Auction auction)
    {
        return new AuctionDTO
        {
            Id = auction.Id,
            Value = auction.Value,
        };
    }
}
