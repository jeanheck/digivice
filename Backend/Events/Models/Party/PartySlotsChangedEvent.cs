namespace Backend.Events.Models.Party;

public class PartySlotsChangedEvent(List<Backend.Domain.Models.Digimon> newParty) : BaseEvent(EventType.PartySlotsChanged)
{
    public List<Backend.Domain.Models.Digimon> NewParty { get; } = newParty;
}
