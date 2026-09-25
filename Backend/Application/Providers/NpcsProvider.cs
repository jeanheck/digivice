using Backend.Application.Loaders.Interfaces;
using Backend.Domain.Assemblers;
using Backend.Domain.Models;
using Backend.Application.Providers.Interfaces;

namespace Backend.Application.Providers
{
    public class NpcsProvider(INpcsLoader npcsLoader) : INpcsProvider
    {
        public Npcs Get()
        {
            return NpcsAssembler.Assemble(npcsLoader.Load());
        }
    }
}
