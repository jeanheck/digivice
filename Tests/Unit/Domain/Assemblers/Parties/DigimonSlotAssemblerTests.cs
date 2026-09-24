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
                HP = new VitalResource(),
                MP = new VitalResource(),
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

    [Fact]
    public void Assemble_ShouldKeepKotemonAsOccupiedSlot_WhenDigimonIdIsZero()
    {
        var resource = new DigimonSlotResource
        {
            Index = 1,
            DigimonId = 0,
            DigimonResource = new DigimonResource { Level = 3 }
        };

        var result = DigimonSlotAssembler.Assemble(resource);

        Assert.Equal(0, result.DigimonId);
        Assert.NotNull(result.Digimon);
        Assert.Equal(3, result.Digimon.Level);
    }

    [Theory]
    [InlineData(0xFF)]
    [InlineData(42)]
    public void Assemble_ShouldReturnEmptySlot_WhenDigimonResourceIsNull(int rawDigimonId)
    {
        var resource = new DigimonSlotResource
        {
            Index = 2,
            DigimonId = rawDigimonId,
            DigimonResource = null
        };

        var result = DigimonSlotAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Equal(2, result.Index);
        Assert.Null(result.DigimonId);
        Assert.Null(result.Digimon);
    }
}
