namespace Backend.Events.Data.Digimon;

public class DigimonDigievolutionLevelChangedEvent : BaseEvent
{
    public int PartySlotIndex { get; }
    public int DigievolutionId { get; }
    public int OldLevel { get; }
    public int NewLevel { get; }

    public DigimonDigievolutionLevelChangedEvent(int partySlotIndex, int digievolutionId, int oldLevel, int newLevel)
        : base(EventType.DigimonDigievolutionLevelChanged)
    {
        PartySlotIndex = partySlotIndex;
        DigievolutionId = digievolutionId;
        OldLevel = oldLevel;
        NewLevel = newLevel;
    }
}
