using Backend.Domain.Backend.Domain.Models.Digimons;

namespace Backend.Events.Data.Digimon;

public class DigimonEquipmentsChangedEvent(int partySlotIndex, Equipments equipments) : BaseEvent(EventType.DigimonEquipmentsChanged)
{
    public int PartySlotIndex { get; } = partySlotIndex;
    public Equipments Equipments { get; } = equipments;
}
