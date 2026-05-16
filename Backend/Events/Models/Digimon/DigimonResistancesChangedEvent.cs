namespace Backend.Events.Models.Digimon;

public class DigimonResistancesChangedEvent(int partySlotIndex, int fire, int water, int ice, int wind, int thunder, int machine, int dark) : BaseEvent(EventType.DigimonResistancesChanged)
{
    public int PartySlotIndex { get; } = partySlotIndex;
    public int Fire { get; } = fire;
    public int Water { get; } = water;
    public int Ice { get; } = ice;
    public int Wind { get; } = wind;
    public int Thunder { get; } = thunder;
    public int Machine { get; } = machine;
    public int Dark { get; } = dark;
}
