using Backend.Domain.Models;
using Backend.Domain.Assemblers;
using Backend.Application.Loaders.Interfaces;
using Backend.Application.Providers.Interfaces;

namespace Backend.Application.Providers
{
    public class PartyProvider(IPartyLoader partyLoader) : IPartyProvider
    {
        public Party Get()
        {
            return PartyAssembler.Assemble(partyLoader.Load());
        }
    }
}
