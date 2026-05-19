using Backend.Domain.Models;
using Backend.Domain.Assemblers;
using Backend.Application.Loaders;

namespace Backend.Application.Providers
{
    public class PartyProvider(PartyLoader partyLoader) : IPartyProvider
    {
        public Party Get()
        {
            return PartyAssembler.Assemble(partyLoader.Load());
        }
    }
}
