namespace Backend.Events.Data.Digimon;

public class DigimonResistancesChangedEvent : BaseEvent
{
    public int PartySlotIndex { get; }
    public int Fire { get; }
    public int Water { get; }
    public int Ice { get; }
    public int Wind { get; }
    public int Thunder { get; }
    public int Machine { get; }
    public int Dark { get; }

    public DigimonResistancesChangedEvent(int partySlotIndex, int fire, int water, int ice, int wind, int thunder, int machine, int dark)
        : base(EventType.DigimonResistancesChanged)
    {
        PartySlotIndex = partySlotIndex;
        Fire = fire;
        Water = water;
        Ice = ice;
        Wind = wind;
        Thunder = thunder;
        Machine = machine;
        Dark = dark;
    }
}
