using Backend.Domain.Models;
using Backend.Domain.Assemblers;
using Backend.Application.Loaders;

namespace Backend.Application.Providers
{
    public class PlayerProvider(PlayerLoader playerLoader)
    {
        public Player Get()
        {
            return PlayerAssembler.Assemble(playerLoader.Load());
        }
    }
}
