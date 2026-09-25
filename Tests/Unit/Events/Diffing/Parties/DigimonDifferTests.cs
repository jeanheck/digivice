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
        newObj.HP.Current = 150;

        var result = DigimonDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.True(result.Level.HasValue);
        Assert.Equal(26, result.Level.Value);
        Assert.False(result.Experience.HasValue);
        Assert.True(result.HP.HasValue);
        Assert.NotNull(result.HP.Value);
        Assert.True(result.HP.Value.Current.HasValue);
        Assert.Equal(150, result.HP.Value.Current.Value);
        Assert.False(result.MP.HasValue);
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

    [Fact]
    public void Diff_ShouldReturnBlastDelta_WhenOnlyBlastChanges()
    {
        var previous = CreateBaseDigimon();
        var newObj = CreateBaseDigimon();
        newObj.Blast = 500;

        var result = DigimonDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.True(result.Blast.HasValue);
        Assert.Equal(500, result.Blast.Value);
        Assert.False(result.Level.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnInBattleConditionDelta_WhenOnlyInBattleConditionChanges()
    {
        var previous = CreateBaseDigimon();
        var newObj = CreateBaseDigimon();
        newObj.InBattle.Condition = 0x04;

        var result = DigimonDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.True(result.InBattle.HasValue);
        Assert.True(result.InBattle.Value!.Condition.HasValue);
        Assert.Equal(0x04, result.InBattle.Value.Condition.Value);
        Assert.False(result.Level.HasValue);
    }

    private static Digimon CreateBaseDigimon()
    {
        return new Digimon
        {
            Level = 10,
            TP = 5,
            Blast = 100,
            Experience = 1000,
            ActiveDigievolutionId = 3,
            HP = new Vital { Current = 100, Max = 100 },
            MP = new Vital { Current = 50, Max = 50 },
            InBattle = new InBattle
            {
                Condition = 0,
                HP = new Vital { Current = 0, Max = 0 },
                MP = new Vital { Current = 0, Max = 0 }
            },
            Attributes = new Attributes { Strength = 5, Defense = 5, Spirit = 5, Wisdom = 5, Speed = 5, Charisma = 5 },
            Resistances = new Resistances { Fire = 1, Water = 1, Ice = 1, Wind = 1, Thunder = 1, Machine = 1, Dark = 1 },
            Equipments = new Equipments { Head = 0, Body = 0, Right = 0, Left = 0, Accessory1 = 0, Accessory2 = 0 },
            Digievolutions = []
        };
    }
}
