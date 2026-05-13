namespace Backend.Events.Data.Digimon;

public class DigimonLevelChangedEvent : BaseEvent
{
    public int PartySlotIndex { get; }
    public int OldLevel { get; }
    public int NewLevel { get; }

    public DigimonLevelChangedEvent(int partySlotIndex, int oldLevel, int newLevel)
        : base(EventType.DigimonLevelChanged)
    {
        PartySlotIndex = partySlotIndex;
        OldLevel = oldLevel;
        NewLevel = newLevel;
    }
}
