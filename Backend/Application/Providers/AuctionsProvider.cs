using Backend.Application.Loaders;
using Backend.Domain.Assemblers;
using Backend.Domain.Models;

namespace Backend.Application.Providers
{
    public class AuctionsProvider(IAuctionsLoader auctionsLoader) : IAuctionsProvider
    {
        public Auctions Get()
        {
            return AuctionsAssembler.Assemble(auctionsLoader.Load());
        }
    }
}
