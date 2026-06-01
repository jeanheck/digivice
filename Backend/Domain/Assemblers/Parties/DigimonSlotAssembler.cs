using Backend.Domain.Models.Parties;
using Backend.Memory.Resources.Parties;

namespace Backend.Domain.Assemblers.Parties
{
    public static class DigimonSlotAssembler
    {
        public static DigimonSlot Assemble(DigimonSlotResource resource)
        {
            return new DigimonSlot
            {
                Index = resource.Index,
                DigimonId = resource.DigimonId,
                Digimon = resource.DigimonResource != null
                    ? DigimonAssembler.Assemble(resource.DigimonResource)
                    : null
            };
        }
    }
}
