namespace Backend.Events.Models.Digimon;

public class DigimonLevelChangedEvent(int partySlotIndex, int level) : BaseEvent(EventType.DigimonLevelChanged)
{
    public int PartySlotIndex { get; } = partySlotIndex;
    public int Level { get; } = level;
}
