namespace Backend.Events.Data.Digimon;

public class DigimonActiveDigievolutionChangedEvent(int partySlotIndex, int? activeDigievolutionId) : BaseEvent(EventType.DigimonActiveDigievolutionChanged)
{
    public int PartySlotIndex { get; } = partySlotIndex;
    public int? ActiveDigievolutionId { get; } = activeDigievolutionId;
}
