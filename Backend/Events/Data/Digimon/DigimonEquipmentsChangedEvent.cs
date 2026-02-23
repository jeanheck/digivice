using Backend.Models.Digimons;

namespace Backend.Events.Data.Digimon;

public class DigimonEquipmentsChangedEvent : BaseEvent
{
    public int PartySlotIndex { get; }
    public Equipments Equipments { get; }

    public DigimonEquipmentsChangedEvent(int partySlotIndex, Equipments equipments)
        : base(EventType.DigimonEquipmentsChanged)
    {
        PartySlotIndex = partySlotIndex;
        Equipments = equipments;
    }
}
