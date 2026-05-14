namespace Backend.Events.Data.Digimon;

public class DigimonVitalsChangedEvent(int partySlotIndex, int currentHP, int maxHP, int currentMP, int maxMP) : BaseEvent(EventType.DigimonVitalsChanged)
{
    public int PartySlotIndex { get; } = partySlotIndex;
    public int CurrentHP { get; } = currentHP;
    public int MaxHP { get; } = maxHP;
    public int CurrentMP { get; } = currentMP;
    public int MaxMP { get; } = maxMP;
}
