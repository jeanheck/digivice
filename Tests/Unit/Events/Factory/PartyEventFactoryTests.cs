namespace Tests.Events.Factory;

using Backend.Domain.Models;
using Backend.Domain.Models.Parties;
using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.DTO;
using Backend.Events.Factory;
using Backend.Events.Models;

public class PartyEventFactoryTests
{
    [Fact]
    public void Create_ShouldReturnNoEvents_WhenPartyHasNoChanges()
    {
        var previousState = CreateState(CreateParty(10));
        var newState = CreateState(CreateParty(10));

        var result = PartyEventFactory.Create(previousState, newState);

        Assert.Empty(result);
    }

    [Fact]
    public void Create_ShouldReturnPartyChangedEvent_WhenPartyChanges()
    {
        var previousState = CreateState(CreateParty(10));
        var newState = CreateState(CreateParty(11));

        var result = PartyEventFactory.Create(previousState, newState).ToList();

        var ev = Assert.Single(result);
        Assert.Equal(EventType.PartyChanged, ev.Type);

        var dto = Assert.IsType<PartyDTO>(ev.Payload);
        Assert.True(dto.Slots.HasValue);
        Assert.Single(dto.Slots.Value!);
    }

    private static State CreateState(Party party)
    {
        return new State { Party = party };
    }

    private static Party CreateParty(int level)
    {
        return new Party
        {
            Slots =
            [
                new DigimonSlot
                {
                    Index = 1,
                    DigimonId = 1,
                    Digimon = new Digimon
                    {
                        Level = level,
                        Vitals = new Vitals(),
                        Attributes = new Attributes(),
                        Resistances = new Resistances(),
                        Equipments = new Equipments(),
                        Digievolutions = []
                    }
                }
            ]
        };
    }
}
