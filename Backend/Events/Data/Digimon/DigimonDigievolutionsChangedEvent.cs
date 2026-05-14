using Backend.Models.Digimons;

namespace Backend.Events.Data.Digimon;

public class DigimonDigievolutionsChangedEvent(int partySlotIndex, Digievolution?[] equippedDigievolutions) : BaseEvent(EventType.DigimonDigievolutionsChanged)
{
    public int PartySlotIndex { get; } = partySlotIndex;
    public Digievolution?[] EquippedDigievolutions { get; } = equippedDigievolutions;
}
