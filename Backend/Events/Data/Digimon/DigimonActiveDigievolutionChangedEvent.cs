namespace Backend.Events.Data.Digimon;

public class DigimonActiveDigievolutionChangedEvent : BaseEvent
{
    public int PartySlotIndex { get; }
    public int? ActiveDigievolutionId { get; }

    public DigimonActiveDigievolutionChangedEvent(int partySlotIndex, int? activeDigievolutionId)
        : base(EventType.DigimonActiveDigievolutionChanged)
    {
        PartySlotIndex = partySlotIndex;
        ActiveDigievolutionId = activeDigievolutionId;
    }
}
