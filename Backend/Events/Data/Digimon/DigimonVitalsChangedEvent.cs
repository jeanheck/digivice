namespace Backend.Events.Data.Digimon;

public class DigimonVitalsChangedEvent : BaseEvent
{
    public int PartySlotIndex { get; }
    public int CurrentHP { get; }
    public int MaxHP { get; }
    public int CurrentMP { get; }
    public int MaxMP { get; }

    public DigimonVitalsChangedEvent(int partySlotIndex, int currentHP, int maxHP, int currentMP, int maxMP)
        : base(EventType.DigimonVitalsChanged)
    {
        PartySlotIndex = partySlotIndex;
        CurrentHP = currentHP;
        MaxHP = maxHP;
        CurrentMP = currentMP;
        MaxMP = maxMP;
    }
}
