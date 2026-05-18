using Backend.Domain.Models.Parties.Digimons;
using Backend.Memory.Resources.Party.Digimon;

namespace Backend.Domain.Assemblers.Parties.Digimons
{
    public static class DigievolutionSlotAssembler
    {
        public static DigievolutionSlot Assemble(DigievolutionSlotResource resource)
        {
            return new DigievolutionSlot
            {
                Index = resource.Index,
                DigievolutionId = resource.DigievolutionId,
                Digievolution = DigievolutionAssembler.Assemble(resource.DigievolutionResource)
            };
        }
    }
}
