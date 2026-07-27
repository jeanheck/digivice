using Backend.Memory.Resources;

namespace Backend.Application.Loaders
{
    public interface IAuctionLoader
    {
        List<AuctionResource> LoadAuctions();
    }
}
