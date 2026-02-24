using Backend.Models.Digimons;

namespace Backend.Events.Data.Digimon;

public class DigimonEvolutionsChangedEvent : BaseEvent
{
    public int PartySlotIndex { get; }
    public Evolution?[] EquippedEvolutions { get; }

    public DigimonEvolutionsChangedEvent(int partySlotIndex, Evolution?[] equippedEvolutions)
        : base(EventType.DigimonEvolutionsChanged)
    {
        PartySlotIndex = partySlotIndex;
        EquippedEvolutions = equippedEvolutions;
    }
}
