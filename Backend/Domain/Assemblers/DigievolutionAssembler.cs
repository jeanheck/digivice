using Backend.Domain.Models.Digimons;
using Backend.Memory.Resources.Digimon;

namespace Backend.Domain.Assemblers
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
