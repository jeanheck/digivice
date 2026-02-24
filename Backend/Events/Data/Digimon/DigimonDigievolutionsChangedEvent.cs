using Backend.Models.Digimons;

namespace Backend.Events.Data.Digimon;

public class DigimonDigievolutionsChangedEvent : BaseEvent
{
    public int PartySlotIndex { get; }
    public Digievolution?[] EquippedDigievolutions { get; }

    public DigimonDigievolutionsChangedEvent(int partySlotIndex, Digievolution?[] equippedDigievolutions)
        : base(EventType.DigimonDigievolutionsChanged)
    {
        PartySlotIndex = partySlotIndex;
        EquippedDigievolutions = equippedDigievolutions;
    }
}
