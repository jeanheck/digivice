using Backend.Domain.Models.Parties.Digimons;
using Backend.Memory.Resources.Parties.Digimons;

namespace Backend.Domain.Assemblers.Parties.Digimons
{
    public static class DigievolutionSlotAssembler
    {
        public static DigievolutionSlot Assemble(DigievolutionSlotResource resource)
        {
            return new DigievolutionSlot
            {
                Index = resource.Index,
                DigievolutionId = resource.DigievolutionId > 0 ? resource.DigievolutionId : null,
                Digievolution = resource.DigievolutionResource != null
                    ? DigievolutionAssembler.Assemble(resource.DigievolutionResource)
                    : null
            };
        }
    }
}
