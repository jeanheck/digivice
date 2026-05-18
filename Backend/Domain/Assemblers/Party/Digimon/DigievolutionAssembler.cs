using Backend.Domain.Models.Parties.Digimons;
using Backend.Memory.Resources.Party.Digimon;

namespace Backend.Domain.Assemblers.Party.Digimon
{
    public static class DigievolutionAssembler
    {
        public static Digievolution Assemble(DigievolutionResource resource)
        {
            return new Digievolution
            {
                Level = resource.Level
            };
        }
    }
}
