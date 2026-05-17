using Backend.Domain.Models;
using Backend.Memory.Resources;

namespace Backend.Domain.Assemblers
{
    public static class SlotAssembler
    {
        public static Slot Assemble(SlotResource resource)
        {
            return new Slot
            {
                Index = resource.Index,
                DigimonId = resource.DigimonId,
                Digimon = null
            };
        }
    }
}
