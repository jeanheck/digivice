using Backend.Domain.Models;
using Backend.Memory.Resources;

namespace Backend.Domain.Assemblers
{
    public static class AuctionAssembler
    {
        public static List<Auction> Assemble(IEnumerable<AuctionResource> auctionResources)
        {
            return [.. auctionResources.Select(auctionResource => {
                return new Auction
                {
                    Id = auctionResource.Id,
                    Value = auctionResource.Value,
                };
            })];
        }
    }
}
