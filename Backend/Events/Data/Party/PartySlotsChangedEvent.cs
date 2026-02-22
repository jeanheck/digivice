namespace Backend.Events.Data.Party;

public class PartySlotsChangedEvent : BaseEvent
{
    public List<Models.Digimon> NewParty { get; }

    public PartySlotsChangedEvent(List<Models.Digimon> newParty) : base(EventType.PartySlotsChanged)
    {
        NewParty = newParty;
    }
}
