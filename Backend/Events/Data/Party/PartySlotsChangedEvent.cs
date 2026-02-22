namespace Backend.Events.Data.Party;

public class PartySlotsChangedEvent : BaseEvent
{
    public List<Models.Digimons.Digimon> NewParty { get; }

    public PartySlotsChangedEvent(List<Models.Digimons.Digimon> newParty) : base(EventType.PartySlotsChanged)
    {
        NewParty = newParty;
    }
}
