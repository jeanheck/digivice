namespace Backend.Events.Data.Digimon;

public class DigimonXpGainedEvent : BaseEvent
{
    public int PartySlotIndex { get; }
    public int Level { get; }
    public int CurrentEXP { get; }

    public DigimonXpGainedEvent(int partySlotIndex, int level, int currentEXP)
        : base(EventType.DigimonXpGained)
    {
        PartySlotIndex = partySlotIndex;
        Level = level;
        CurrentEXP = currentEXP;
    }
}
