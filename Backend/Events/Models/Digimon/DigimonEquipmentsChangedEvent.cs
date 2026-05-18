using Backend.Domain.Models.Parties.Digimons;

namespace Backend.Events.Models.Digimon;

public class DigimonEquipmentsChangedEvent(int partySlotIndex, Equipments equipments) : BaseEvent(EventType.DigimonEquipmentsChanged)
{
    public int PartySlotIndex { get; } = partySlotIndex;
    public Equipments Equipments { get; } = equipments;
}
