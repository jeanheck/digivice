using Backend.Domain.Models;
using Backend.Memory.Resources.Digimon;

namespace Backend.Domain.Assemblers
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
