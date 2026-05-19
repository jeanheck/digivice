using Backend.Domain.Models.Parties.Digimons;
using Backend.Memory.Resources.Parties.Digimons;

namespace Backend.Domain.Assemblers.Parties.Digimons
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
