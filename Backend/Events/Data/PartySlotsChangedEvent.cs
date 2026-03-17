namespace Backend.Events.Data;

public class PartySlotsChangedEvent : BaseEvent
{
    public List<Models.Digimon> NewParty { get; }

    public PartySlotsChangedEvent(List<Models.Digimon> newParty) : base(EventType.PartySlotsChanged)
    {
        NewParty = newParty;
    }
}
