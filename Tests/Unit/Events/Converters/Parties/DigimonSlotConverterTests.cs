namespace Tests.Events.Converters.Parties;

using Backend.Domain.Models.Parties;
using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties;

public class DigimonSlotConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapSlotAndNestedDigimon()
    {
        var slot = new DigimonSlot
        {
            Index = 2,
            DigimonId = 7,
            Digimon = new Digimon
            {
                Level = 30,
                Vitals = new Vitals(),
                Attributes = new Attributes(),
                Resistances = new Resistances(),
                Equipments = new Equipments(),
                Digievolutions = []
            }
        };

        var dto = DigimonSlotConverter.ToDTO(slot);

        Assert.Equal(2, dto.Index);
        Assert.True(dto.DigimonId.HasValue);
        Assert.Equal(7, dto.DigimonId.Value);
        Assert.True(dto.Digimon.HasValue);
        Assert.NotNull(dto.Digimon.Value);
        Assert.Equal(30, dto.Digimon.Value!.Level.Value);
    }

    [Fact]
    public void ToDTO_ShouldPreserveNullDigimon()
    {
        var slot = new DigimonSlot
        {
            Index = 3,
            DigimonId = 255,
            Digimon = null
        };

        var dto = DigimonSlotConverter.ToDTO(slot);

        Assert.Equal(3, dto.Index);
        Assert.True(dto.DigimonId.HasValue);
        Assert.Equal(255, dto.DigimonId.Value);
        Assert.True(dto.Digimon.HasValue);
        Assert.Null(dto.Digimon.Value);
    }
}
