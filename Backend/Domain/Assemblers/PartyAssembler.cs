using Backend.Domain.Assemblers.Party;
using Backend.Memory.Resources;

namespace Backend.Domain.Assemblers
{
    public static class PartyAssembler
    {
        public static Backend.Domain.Models.Party Assemble(PartyResource resource)
        {
            return new Backend.Domain.Models.Party
            {
                Slots = [.. resource.SlotsResource.Select(DigimonSlotAssembler.Assemble)]
            };
        }
    }
}
