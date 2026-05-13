namespace Backend.Events.Data.Digimon;

public class DigimonExperienceChangedEvent : BaseEvent
{
    public int PartySlotIndex { get; }
    public int Level { get; }
    public int CurrentEXP { get; }

    public DigimonExperienceChangedEvent(int partySlotIndex, int level, int currentEXP)
        : base(EventType.DigimonExperienceChanged)
    {
        PartySlotIndex = partySlotIndex;
        Level = level;
        CurrentEXP = currentEXP;
    }
}
