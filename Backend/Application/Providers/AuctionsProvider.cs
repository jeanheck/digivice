using Backend.Application.Loaders.Interfaces;
using Backend.Domain.Assemblers;
using Backend.Domain.Models;
using Backend.Application.Providers.Interfaces;

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
