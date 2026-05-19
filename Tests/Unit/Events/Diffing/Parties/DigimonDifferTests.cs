namespace Tests.Events.Diffing.Parties;

using Backend.Events.Diffing.Parties;
using Backend.Domain.Models.Parties;
using Backend.Domain.Models.Parties.Digimons;

public class DigimonDifferTests
{
    [Fact]
    public void Diff_ShouldReturnNull_WhenNoChanges()
    {
        var previous = CreateBaseDigimon();
        var newObj = CreateBaseDigimon();

        var result = DigimonDiffer.Diff(previous, newObj);

        Assert.Null(result);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var newObj = CreateBaseDigimon();
        newObj.Level = 25;

        var result = DigimonDiffer.Diff(null, newObj);

        Assert.NotNull(result);
        Assert.True(result.Level.HasValue);
        Assert.Equal(25, result.Level.Value);
    }

    [Fact]
    public void Diff_ShouldReturnChangedFields_WhenPartialChanges()
    {
        var previous = CreateBaseDigimon();
        var newObj = CreateBaseDigimon();
        newObj.Level = 26;
        newObj.Vitals.CurrentHP = 150;

        var result = DigimonDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.True(result.Level.HasValue);
        Assert.Equal(26, result.Level.Value);
        Assert.False(result.Experience.HasValue);
        Assert.True(result.Vitals.HasValue);
        Assert.NotNull(result.Vitals.Value);
        Assert.True(result.Vitals.Value.CurrentHP.HasValue);
        Assert.Equal(150, result.Vitals.Value.CurrentHP.Value);
    }

    private static Digimon CreateBaseDigimon()
    {
        return new Digimon
        {
            Level = 10,
            Experience = 1000,
            ActiveDigievolutionId = 3,
            Vitals = new Vitals { CurrentHP = 100, MaxHP = 100, CurrentMP = 50, MaxMP = 50 },
            Attributes = new Attributes { Strength = 5, Defense = 5, Spirit = 5, Wisdom = 5, Speed = 5, Charisma = 5 },
            Resistances = new Resistances { Fire = 1, Water = 1, Ice = 1, Wind = 1, Thunder = 1, Machine = 1, Dark = 1 },
            Equipments = new Equipments { Head = 0, Body = 0, RightHand = 0, LeftHand = 0, Accessory1 = 0, Accessory2 = 0 },
            Digievolutions = []
        };
    }
}
