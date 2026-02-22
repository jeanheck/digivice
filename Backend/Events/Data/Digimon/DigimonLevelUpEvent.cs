namespace Backend.Events.Data.Digimon;

public class DigimonLevelUpEvent : BaseEvent
{
    public int PartySlotIndex { get; }
    public int OldLevel { get; }
    public int NewLevel { get; }

    public DigimonLevelUpEvent(int partySlotIndex, int oldLevel, int newLevel)
        : base(EventType.DigimonLevelUp)
    {
        PartySlotIndex = partySlotIndex;
        OldLevel = oldLevel;
        NewLevel = newLevel;
    }
}
