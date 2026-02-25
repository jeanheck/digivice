namespace Backend.Events.Data.Digimon;

public class DigimonDigievolutionLevelUpEvent : BaseEvent
{
    public int PartySlotIndex { get; }
    public int DigievolutionId { get; }
    public int OldLevel { get; }
    public int NewLevel { get; }

    public DigimonDigievolutionLevelUpEvent(int partySlotIndex, int digievolutionId, int oldLevel, int newLevel) 
        : base(EventType.DigimonDigievolutionLevelUp)
    {
        PartySlotIndex = partySlotIndex;
        DigievolutionId = digievolutionId;
        OldLevel = oldLevel;
        NewLevel = newLevel;
    }
}
