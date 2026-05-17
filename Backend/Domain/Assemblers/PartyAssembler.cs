using Backend.Domain.Models;
using Backend.Memory.Resources;

namespace Backend.Domain.Assemblers
{
    public static class PartyAssembler
    {
        public static Party Assemble(PartyResource resource)
        {
            return new Party
            {
                Slots = [.. resource.SlotsResource.Select(SlotAssembler.Assemble)]
            };
        }
    }
}
