namespace Backend.Events.Data.Digimon;

public class DigimonAttributesChangedEvent : BaseEvent
{
    public int PartySlotIndex { get; }
    public int Strength { get; }
    public int Defense { get; }
    public int Spirit { get; }
    public int Wisdom { get; }
    public int Speed { get; }
    public int Charisma { get; }

    public DigimonAttributesChangedEvent(int partySlotIndex, int strength, int defense, int spirit, int wisdom, int speed, int charisma)
        : base(EventType.DigimonAttributesChanged)
    {
        PartySlotIndex = partySlotIndex;
        Strength = strength;
        Defense = defense;
        Spirit = spirit;
        Wisdom = wisdom;
        Speed = speed;
        Charisma = charisma;
    }
}
