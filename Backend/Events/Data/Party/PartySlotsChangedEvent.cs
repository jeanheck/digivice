using System.Collections.Generic;
using Backend.Models;

namespace Backend.Events.Data.Party;

public class PartySlotsChangedEvent : BaseEvent
{
    public List<Backend.Models.Digimons.Digimon> NewParty { get; }

    public PartySlotsChangedEvent(List<Backend.Models.Digimons.Digimon> newParty) : base(EventType.PartySlotsChanged)
    {
        NewParty = newParty;
    }
}
