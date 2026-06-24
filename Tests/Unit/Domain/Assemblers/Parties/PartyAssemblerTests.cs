namespace Tests.Domain.Assemblers.Parties;

using Backend.Domain.Assemblers;
using Backend.Memory.Resources;
using Backend.Memory.Resources.Parties;
using Backend.Memory.Resources.Parties.Digimons;

public class PartyAssemblerTests
{
    [Fact]
    public void Assemble_ShouldMapAllFieldsCorrectly()
    {
        var resource = new PartyResource
        {
            SlotsResource = [
                new DigimonSlotResource
                {
                    Index = 0,
                    DigimonId = 1,
                    DigimonResource = new DigimonResource
                    {
                        Experience = 100,
                        Level = 2,
                        Vitals = new VitalsResource(),
                        Attributes = new AttributesResource(),
                        Resistances = new ResistancesResource(),
                        Equipments = new EquipmentsResource(),
                        Digievolutions = []
                    }
                }
            ]
        };

        var result = PartyAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Single(result.Slots);
        Assert.Equal(0, result.Slots[0].Index);
        Assert.Equal(1, result.Slots[0].DigimonId);
    }
}
