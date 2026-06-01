namespace Tests.Events.Diffing.Parties;

using Backend.Events.Diffing.Parties;
using Backend.Domain.Models.Parties;
using Backend.Domain.Models.Parties.Digimons;

public class DigimonSlotDifferTests
{
    [Fact]
    public void Diff_ShouldReturnNull_WhenNoChanges()
    {
        var previous = new DigimonSlot { Index = 1, DigimonId = 3, Digimon = CreateBaseDigimon() };
        var newObj = new DigimonSlot { Index = 1, DigimonId = 3, Digimon = CreateBaseDigimon() };

        var result = DigimonSlotDiffer.Diff(previous, newObj);

        Assert.Null(result);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var newObj = new DigimonSlot { Index = 1, DigimonId = 3, Digimon = CreateBaseDigimon() };

        var result = DigimonSlotDiffer.Diff(null, newObj);

        Assert.NotNull(result);
        Assert.Equal(1, result.Index);
        Assert.True(result.DigimonId.HasValue);
        Assert.Equal(3, result.DigimonId.Value);
        Assert.True(result.Digimon.HasValue);
        Assert.NotNull(result.Digimon.Value);
    }

    [Fact]
    public void Diff_ShouldReturnDelta_WhenDigimonIdChangedToNull()
    {
        var previous = new DigimonSlot { Index = 1, DigimonId = 3, Digimon = CreateBaseDigimon() };
        var newObj = new DigimonSlot { Index = 1, DigimonId = null, Digimon = null };

        var result = DigimonSlotDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.Equal(1, result.Index);
        Assert.True(result.DigimonId.HasValue);
        Assert.Null(result.DigimonId.Value);
        Assert.True(result.Digimon.HasValue);
        Assert.Null(result.Digimon.Value);
    }

    [Fact]
    public void Diff_ShouldReturnDelta_WhenNestedDigimonChanged()
    {
        var previous = new DigimonSlot { Index = 1, DigimonId = 3, Digimon = CreateBaseDigimon() };
        var newObj = new DigimonSlot { Index = 1, DigimonId = 3, Digimon = CreateBaseDigimon() };
        newObj.Digimon.Level = 12;

        var result = DigimonSlotDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.Equal(1, result.Index);
        Assert.True(result.DigimonId.HasValue);
        Assert.Equal(3, result.DigimonId.Value);
        Assert.True(result.Digimon.HasValue);
        Assert.NotNull(result.Digimon.Value);
        Assert.True(result.Digimon.Value.Level.HasValue);
        Assert.Equal(12, result.Digimon.Value.Level.Value);
    }

    [Fact]
    public void Diff_ShouldReturnNull_WhenBothSlotsAreEmpty()
    {
        var previous = new DigimonSlot { Index = 2, DigimonId = null, Digimon = null };
        var newObj = new DigimonSlot { Index = 2, DigimonId = null, Digimon = null };

        var result = DigimonSlotDiffer.Diff(previous, newObj);

        Assert.Null(result);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenDigimonIdChangedToAnotherActiveId()
    {
        var previous = new DigimonSlot { Index = 1, DigimonId = 3, Digimon = CreateBaseDigimon() };
        var newObj = new DigimonSlot { Index = 1, DigimonId = 5, Digimon = CreateBaseDigimon() };
        newObj.Digimon.Level = 15;

        var result = DigimonSlotDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.Equal(1, result.Index);
        Assert.True(result.DigimonId.HasValue);
        Assert.Equal(5, result.DigimonId.Value);
        Assert.True(result.Digimon.HasValue);
        Assert.NotNull(result.Digimon.Value);
        Assert.True(result.Digimon.Value.Level.HasValue);
        Assert.Equal(15, result.Digimon.Value.Level.Value);
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
