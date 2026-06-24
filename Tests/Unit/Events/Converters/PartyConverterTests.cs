namespace Tests.Events.Converters;

using Backend.Domain.Models;
using Backend.Domain.Models.Parties;
using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters;

public class PartyConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapPartySlots()
    {
        var party = new Party
        {
            Slots =
            [
                new DigimonSlot
                {
                    Index = 1,
                    DigimonId = 7,
                    Digimon = new Digimon
                    {
                        Level = 33,
                        Vitals = new Vitals(),
                        Attributes = new Attributes(),
                        Resistances = new Resistances(),
                        Equipments = new Equipments(),
                        Digievolutions = []
                    }
                }
            ]
        };

        var dto = PartyConverter.ToDTO(party);

        Assert.True(dto.Slots.HasValue);
        var slot = Assert.Single(dto.Slots.Value!);
        Assert.Equal(1, slot.Index);
        Assert.True(slot.DigimonId.HasValue);
        Assert.Equal(7, slot.DigimonId.Value);
        Assert.True(slot.Digimon.HasValue);
        Assert.NotNull(slot.Digimon.Value);
        Assert.True(slot.Digimon.Value!.Level.HasValue);
        Assert.Equal(33, slot.Digimon.Value.Level.Value);
    }
}
