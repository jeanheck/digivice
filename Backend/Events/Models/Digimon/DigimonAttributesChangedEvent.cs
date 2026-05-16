namespace Backend.Events.Models.Digimon;

public class DigimonAttributesChangedEvent(int partySlotIndex, int strength, int defense, int spirit, int wisdom, int speed, int charisma) : BaseEvent(EventType.DigimonAttributesChanged)
{
    public int PartySlotIndex { get; } = partySlotIndex;
    public int Strength { get; } = strength;
    public int Defense { get; } = defense;
    public int Spirit { get; } = spirit;
    public int Wisdom { get; } = wisdom;
    public int Speed { get; } = speed;
    public int Charisma { get; } = charisma;
}
