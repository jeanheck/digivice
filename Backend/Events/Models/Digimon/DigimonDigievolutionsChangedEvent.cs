using Backend.Domain.Models.Digimons;

namespace Backend.Events.Models.Digimon;

public class DigimonDigievolutionsChangedEvent(int partySlotIndex, Digievolution?[] digievolutions) : BaseEvent(EventType.DigimonDigievolutionsChanged)
{
    public int PartySlotIndex { get; } = partySlotIndex;
    public Digievolution?[] Digievolutions { get; } = digievolutions;
}
