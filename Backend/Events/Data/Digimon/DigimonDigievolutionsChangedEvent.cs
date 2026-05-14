using Backend.Models.Digimons;

namespace Backend.Events.Data.Digimon;

public class DigimonDigievolutionsChangedEvent(int partySlotIndex, Digievolution?[] digievolutions) : BaseEvent(EventType.DigimonDigievolutionsChanged)
{
    public int PartySlotIndex { get; } = partySlotIndex;
    public Digievolution?[] Digievolutions { get; } = digievolutions;
}
