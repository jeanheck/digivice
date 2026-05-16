namespace Backend.Events.Models.Digimon;

public class DigimonExperienceChangedEvent(int partySlotIndex, int experience) : BaseEvent(EventType.DigimonExperienceChanged)
{
    public int PartySlotIndex { get; } = partySlotIndex;
    public int Experience { get; } = experience;
}
