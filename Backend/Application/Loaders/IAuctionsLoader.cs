using Backend.Memory.Resources;

namespace Backend.Application.Loaders
{
    public interface IAuctionsLoader
    {
        AuctionsResource Load();
    }
}
