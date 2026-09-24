using Backend.Domain.Models.Parties.Digimons;
using Backend.Memory.Resources.Parties.Digimons;

namespace Backend.Domain.Assemblers.Parties.Digimons
{
    public static class DigievolutionSlotAssembler
    {
        public static DigievolutionSlot Assemble(
            DigievolutionSlotResource resource,
            List<StoredDigievolutionResource> storedDigievolutions)
        {
            if (resource.DigievolutionId <= 0)
            {
                return new DigievolutionSlot
                {
                    Index = resource.Index,
                    DigievolutionId = null,
                    Digievolution = null
                };
            }

            var storedDigievolution = storedDigievolutions
                .FirstOrDefault(stored => stored.DigievolutionId == resource.DigievolutionId);

            return new DigievolutionSlot
            {
                Index = resource.Index,
                DigievolutionId = resource.DigievolutionId,
                Digievolution = new Digievolution
                {
                    Level = storedDigievolution?.Level ?? 1,
                    Dvxp = storedDigievolution?.Dvxp ?? 0
                }
            };
        }
    }
}
