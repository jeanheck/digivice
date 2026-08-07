namespace Tests.Events.Converters.Parties;

using Backend.Domain.Models.Parties;
using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties;

public class DigimonConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapDigimonAndAllNestedStructures()
    {
        var digimon = new Digimon
        {
            Level = 42,
            TP = 20,
            Blast = 850,
            Experience = 123456,
            ActiveDigievolutionId = 5,
            HP = new Vital { Current = 101, Max = 202 },
            MP = new Vital { Current = 33, Max = 44 },
            InCombat = new InCombat
            {
                Condition = 0x04,
                Strength = 252,
                Defense = 185,
                Speed = 84,
                HP = new Vital { Current = 90, Max = 202 },
                MP = new Vital { Current = 20, Max = 44 }
            },
            Attributes = new Attributes { Strength = 1, Defense = 2, Spirit = 3, Wisdom = 4, Speed = 5, Charisma = 6 },
            Resistances = new Resistances { Fire = 7, Water = 8, Ice = 9, Wind = 10, Thunder = 11, Machine = 12, Dark = 13 },
            Equipments = new Equipments { Head = 14, Body = 15, Right = 16, Left = 17, Accessory1 = 18, Accessory2 = 19 },
            Digievolutions =
            [
                new DigievolutionSlot
                {
                    Index = 1,
                    DigievolutionId = 5,
                    Digievolution = new Digievolution { Level = 9, Dvxp = 200 }
                }
            ],
            StoredDigievolutions =
            [
                new StoredDigievolution { DigievolutionId = 5, Level = 9 },
                new StoredDigievolution { DigievolutionId = 99, Level = 3 }
            ]
        };

        var dto = DigimonConverter.ToDTO(digimon);

        Assert.Equal(42, dto.Level.Value);
        Assert.Equal(20, dto.TP.Value);
        Assert.Equal(850, dto.Blast.Value);
        Assert.Equal(123456, dto.Experience.Value);
        Assert.Equal(5, dto.ActiveDigievolutionId.Value);
        Assert.Equal(101, dto.HP.Value!.Current.Value);
        Assert.Equal(202, dto.HP.Value.Max.Value);
        Assert.Equal(33, dto.MP.Value!.Current.Value);
        Assert.Equal(44, dto.MP.Value.Max.Value);
        Assert.Equal(0x04, dto.InCombat.Value!.Condition.Value);
        Assert.Equal(252, dto.InCombat.Value.Strength.Value);
        Assert.Equal(185, dto.InCombat.Value.Defense.Value);
        Assert.Equal(84, dto.InCombat.Value.Speed.Value);
        Assert.Equal(90, dto.InCombat.Value.HP.Value!.Current.Value);
        Assert.Equal(202, dto.InCombat.Value.HP.Value.Max.Value);
        Assert.Equal(20, dto.InCombat.Value.MP.Value!.Current.Value);
        Assert.Equal(44, dto.InCombat.Value.MP.Value.Max.Value);
        Assert.Equal(1, dto.Attributes.Value!.Strength.Value);
        Assert.Equal(4, dto.Attributes.Value.Wisdom.Value);
        Assert.Equal(13, dto.Resistances.Value!.Dark.Value);
        Assert.Equal(16, dto.Equipments.Value!.Right.Value);
        var evolutionSlot = Assert.Single(dto.Digievolutions.Value!);
        Assert.Equal(1, evolutionSlot.Index);
        Assert.Equal(5, evolutionSlot.DigievolutionId.Value);
        Assert.Equal(9, evolutionSlot.Digievolution.Value!.Level.Value);
        Assert.Equal(200, evolutionSlot.Digievolution.Value!.Dvxp.Value);

        Assert.Equal(2, dto.StoredDigievolutions.Value!.Count);
        Assert.Equal(99, dto.StoredDigievolutions.Value[1].DigievolutionId.Value);
        Assert.Equal(3, dto.StoredDigievolutions.Value[1].Level.Value);
    }
}
