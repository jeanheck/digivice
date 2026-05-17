using Backend.Domain.Models;
using Backend.Memory.Resources.Party;

namespace Backend.Domain.Assemblers.Party
{
    public static class DigimonSlotAssembler
    {
        public static DigimonSlot Assemble(DigimonSlotResource resource)
        {
            return new DigimonSlot
            {
                Index = resource.Index,
                DigimonId = resource.DigimonId,
                Digimon = DigimonAssembler.Assemble(resource.DigimonResource)
            }; ;
        }
    }
}
