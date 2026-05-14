namespace Backend.Events.Data;

public class PartySlotsChangedEvent(List<Models.Digimon> newParty) : BaseEvent(EventType.PartySlotsChanged)
{
    public List<Models.Digimon> NewParty { get; } = newParty;
}
