using Backend.Domain.Models.Parties.Digimons;
using Backend.Memory.Resources.Parties.Digimons;

namespace Backend.Domain.Assemblers.Parties.Digimons
{
    public static class StoredDigievolutionAssembler
    {
        public static StoredDigievolution Assemble(StoredDigievolutionResource resource)
        {
            return new StoredDigievolution
            {
                DigievolutionId = resource.DigievolutionId,
                Level = resource.Level
            };
        }
    }
}
