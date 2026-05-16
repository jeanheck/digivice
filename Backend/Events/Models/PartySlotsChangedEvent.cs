namespace Backend.Events.Models;

public class PartySlotsChangedEvent(List<Backend.Domain.Models.Digimon> newParty) : BaseEvent(EventType.PartySlotsChanged)
{
    public List<Backend.Domain.Models.Digimon> NewParty { get; } = newParty;
}
