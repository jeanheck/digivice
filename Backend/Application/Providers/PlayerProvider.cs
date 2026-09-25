using Backend.Domain.Models;
using Backend.Domain.Assemblers;
using Backend.Application.Loaders.Interfaces;
using Backend.Application.Providers.Interfaces;

namespace Backend.Application.Providers
{
    public class PlayerProvider(IPlayerLoader playerLoader) : IPlayerProvider
    {
        public Player Get()
        {
            return PlayerAssembler.Assemble(playerLoader.Load());
        }
    }
}
