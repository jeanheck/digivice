namespace Tests.Domain.Assemblers.Parties;

using Backend.Domain.Assemblers;
using Backend.Memory.Resources;
using Backend.Memory.Resources.Parties;
using Backend.Memory.Resources.Parties.Digimons;

public class PartyAssemblerTests
{
    private const int EmptySlotId = 0xFF;

    [Fact]
    public void Assemble_ShouldMapOccupiedAndEmptySlots()
    {
        var resource = new PartyResource
        {
            SlotsResource = [
                new DigimonSlotResource
                {
                    Index = 1,
                    DigimonId = 1,
                    DigimonResource = new DigimonResource
                    {
                        Experience = 100,
                        Level = 2,
                        HP = new VitalResource(),
                        MP = new VitalResource(),
                        Attributes = new AttributesResource(),
                        Resistances = new ResistancesResource(),
                        Equipments = new EquipmentsResource(),
                        Digievolutions = []
                    }
                },
                new DigimonSlotResource { Index = 2, DigimonId = EmptySlotId, DigimonResource = null },
                new DigimonSlotResource { Index = 3, DigimonId = EmptySlotId, DigimonResource = null }
            ]
        };

        var result = PartyAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Equal(3, result.Slots.Count);

        Assert.Equal(1, result.Slots[0].Index);
        Assert.Equal(1, result.Slots[0].DigimonId);
        Assert.NotNull(result.Slots[0].Digimon);
        Assert.Equal(2, result.Slots[0].Digimon!.Level);

        Assert.Equal(2, result.Slots[1].Index);
        Assert.Null(result.Slots[1].DigimonId);
        Assert.Null(result.Slots[1].Digimon);

        Assert.Equal(3, result.Slots[2].Index);
        Assert.Null(result.Slots[2].DigimonId);
        Assert.Null(result.Slots[2].Digimon);
    }
}
