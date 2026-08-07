using Backend.Application.Loaders;
using Backend.Domain.Assemblers;
using Backend.Domain.Models;

namespace Backend.Application.Providers
{
    public class BattleProvider(IBattleLoader battleLoader) : IBattleProvider
    {
        public Battle Get()
        {
            return BattleAssembler.Assemble(battleLoader.Load());
        }
    }
}
