namespace Tests.Domain.Assemblers.Parties;

using Backend.Domain.Assemblers.Parties;
using Backend.Memory.Resources.Parties;
using Backend.Memory.Resources.Parties.Digimons;

public class DigimonSlotAssemblerTests
{
    [Fact]
    public void Assemble_ShouldMapAllFieldsCorrectly()
    {
        var resource = new DigimonSlotResource
        {
            Index = 2,
            DigimonId = 8,
            DigimonResource = new DigimonResource
            {
                Experience = 200,
                Level = 5,
                Vitals = new VitalsResource(),
                Attributes = new AttributesResource(),
                Resistances = new ResistancesResource(),
                Equipments = new EquipmentsResource(),
                Digievolutions = []
            }
        };

        var result = DigimonSlotAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Equal(2, result.Index);
        Assert.Equal(8, result.DigimonId);
        Assert.NotNull(result.Digimon);
        Assert.Equal(5, result.Digimon.Level);
    }
}
