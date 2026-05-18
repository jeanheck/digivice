namespace Backend.Events.Models.Party;

public class PartySlotsChangedEvent(List<Backend.Domain.Models.Parties.Digimon> newParty) : BaseEvent(EventType.PartySlotsChanged)
{
    public List<Backend.Domain.Models.Parties.Digimon> NewParty { get; } = newParty;
}
