using Backend.Domain.Models;

namespace Backend.Application.Providers
{
    public interface IAuctionsProvider
    {
        Auctions Get();
    }
}
