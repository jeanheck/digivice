using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.DTO.Party.Digimon;

namespace Backend.Events.Converters.Parties;

public static class DigievolutionSlotConverter
{
    public static DigievolutionSlotDTO ToDTO(DigievolutionSlot slot) => new()
    {
        Index = slot.Index,
        DigievolutionId = slot.DigievolutionId,
        Digievolution = slot.Digievolution != null ? DigievolutionConverter.ToDTO(slot.Digievolution) : null
    };
}
