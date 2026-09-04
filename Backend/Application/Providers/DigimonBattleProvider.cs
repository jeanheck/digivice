using Backend.Application.Loaders;
using Backend.Domain.Assemblers;
using Backend.Domain.Models;

namespace Backend.Application.Providers
{
    public class DigimonBattleProvider(IDigimonBattleLoader digimonBattleLoader) : IDigimonBattleProvider
    {
        public DigimonBattle Get()
        {
            return DigimonBattleAssembler.Assemble(digimonBattleLoader.Load());
        }
    }
}
