using Backend.Memory.Resources;

namespace Backend.Application.Loaders
{
    public interface IAuctionLoader
    {
        AuctionsResource Load();
    }
}
