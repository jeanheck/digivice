namespace Tests.Domain.Assemblers.Parties;

using Backend.Domain.Assemblers.Parties;
using Backend.Memory.Resources.Parties;
using Backend.Memory.Resources.Parties.Digimons;

public class DigimonAssemblerTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(-1, 0)]
    [InlineData(0xFFFF, 0)]
    [InlineData(5, 5)]
    public void Assemble_ShouldSanitizeActiveDigievolutionId(int inputId, int expectedId)
    {
        var resource = CreateBaseDigimonResource();
        resource.ActiveDigievolutionId = inputId;

        var result = DigimonAssembler.Assemble(resource);

        Assert.Equal(expectedId, result.ActiveDigievolutionId);
    }

    [Fact]
    public void Assemble_ShouldMapAllSubStructuresCorrectly()
    {
        var resource = new DigimonResource
        {
            Experience = 5000,
            Level = 30,
            TP = 15,
            BlastGauge = 600,
            ActiveDigievolutionId = 12,
            Vitals = new VitalsResource { CurrentHP = 100, MaxHP = 120, CurrentMP = 50, MaxMP = 60 },
            Attributes = new AttributesResource { Strength = 10, Defense = 11, Spirit = 12, Wisdow = 13, Speed = 14, Charisma = 15 },
            Resistances = new ResistancesResource { Fire = 1, Water = 2, Ice = 3, Wind = 4, Thunder = 5, Machine = 6, Dark = 7 },
            Equipments = new EquipmentsResource { Head = 101, Body = 102, Right = 103, Left = 104, Accessory1 = 105, Accessory2 = 106 },
            Digievolutions = [
                new DigievolutionSlotResource { Index = 0, DigievolutionId = 1, DigievolutionResource = new DigievolutionResource { Level = 5, Dvxp = 200 } }
            ],
            StoredDigievolutions = [
                new StoredDigievolutionResource { DigievolutionId = 1, Level = 5 },
                new StoredDigievolutionResource { DigievolutionId = 99, Level = 12 }
            ]
        };

        var result = DigimonAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Equal(5000, result.Experience);
        Assert.Equal(30, result.Level);
        Assert.Equal(15, result.TP);
        Assert.Equal(600, result.BlastGauge);
        Assert.Equal(12, result.ActiveDigievolutionId);

        // Vitals
        Assert.Equal(100, result.Vitals.CurrentHP);
        Assert.Equal(120, result.Vitals.MaxHP);
        Assert.Equal(50, result.Vitals.CurrentMP);
        Assert.Equal(60, result.Vitals.MaxMP);

        // Attributes
        Assert.Equal(10, result.Attributes.Strength);
        Assert.Equal(11, result.Attributes.Defense);
        Assert.Equal(12, result.Attributes.Spirit);
        Assert.Equal(13, result.Attributes.Wisdom);
        Assert.Equal(14, result.Attributes.Speed);
        Assert.Equal(15, result.Attributes.Charisma);

        // Resistances
        Assert.Equal(1, result.Resistances.Fire);
        Assert.Equal(7, result.Resistances.Dark);

        // Equipments
        Assert.Equal(101, result.Equipments.Head);
        Assert.Equal(106, result.Equipments.Accessory2);

        // Digievolutions
        Assert.Single(result.Digievolutions);
        Assert.Equal(1, result.Digievolutions[0].DigievolutionId);
        Assert.Equal(5, result.Digievolutions[0].Digievolution!.Level);
        Assert.Equal(200, result.Digievolutions[0].Digievolution!.Dvxp);

        Assert.Equal(2, result.StoredDigievolutions.Count);
        Assert.Equal(99, result.StoredDigievolutions[1].DigievolutionId);
        Assert.Equal(12, result.StoredDigievolutions[1].Level);
    }

    private static DigimonResource CreateBaseDigimonResource()
    {
        return new DigimonResource
        {
            Experience = 100,
            Level = 1,
            TP = 0,
            Vitals = new VitalsResource(),
            Attributes = new AttributesResource(),
            Resistances = new ResistancesResource(),
            Equipments = new EquipmentsResource(),
            Digievolutions = []
        };
    }
}
