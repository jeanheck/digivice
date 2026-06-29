namespace Tests.Events.Diffing.Parties;

using Backend.Domain.Models.Parties;
using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Diffing.Parties;

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

    [Fact]
    public void Diff_ShouldReturnStoredDigievolutionsDelta_WhenStoredLevelChanges()
    {
        var previous = CreateBaseDigimon();
        previous.StoredDigievolutions = [new StoredDigievolution { DigievolutionId = 386, Level = 14 }];

        var newObj = CreateBaseDigimon();
        newObj.StoredDigievolutions = [new StoredDigievolution { DigievolutionId = 386, Level = 15 }];

        var result = DigimonDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.False(result.Level.HasValue);
        Assert.True(result.StoredDigievolutions.HasValue);
        var storedDelta = Assert.Single(result.StoredDigievolutions.Value!);
        Assert.Equal(386, storedDelta.DigievolutionId.Value);
        Assert.Equal(15, storedDelta.Level.Value);
    }

    [Fact]
    public void Diff_ShouldReturnStoredDigievolutionsDelta_WhenNewDigievolutionIsUnlocked()
    {
        var previous = CreateBaseDigimon();
        previous.StoredDigievolutions = [new StoredDigievolution { DigievolutionId = 386, Level = 14 }];

        var newObj = CreateBaseDigimon();
        newObj.StoredDigievolutions =
        [
            new StoredDigievolution { DigievolutionId = 386, Level = 14 },
            new StoredDigievolution { DigievolutionId = 260, Level = 1 }
        ];

        var result = DigimonDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.True(result.StoredDigievolutions.HasValue);
        var storedDelta = Assert.Single(result.StoredDigievolutions.Value!);
        Assert.Equal(260, storedDelta.DigievolutionId.Value);
        Assert.Equal(1, storedDelta.Level.Value);
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
            Equipments = new Equipments { Head = 0, Body = 0, Right = 0, Left = 0, Accessory1 = 0, Accessory2 = 0 },
            Digievolutions = []
        };
    }
}
