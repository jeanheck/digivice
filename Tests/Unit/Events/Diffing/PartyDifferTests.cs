namespace Tests.Events.Diffing;

using Backend.Domain.Models;
using Backend.Domain.Models.Parties;
using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Diffing;

public class PartyDifferTests
{
    [Fact]
    public void Diff_ShouldReturnEmptyDTO_WhenNoChanges()
    {
        var previous = new Party { Slots = [] };
        var newObj = new Party { Slots = [] };

        var result = PartyDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.False(result.Slots.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var slot = new DigimonSlot { Index = 0, DigimonId = 1, Digimon = CreateBaseDigimon() };
        var newObj = new Party { Slots = [slot] };

        var result = PartyDiffer.Diff(null, newObj);

        Assert.NotNull(result);
        Assert.True(result.Slots.HasValue);


        var slots = result.Slots.Value!;
        Assert.Single(slots);
        Assert.Equal(0, slots[0].Index);
        Assert.True(slots[0].DigimonId.HasValue);
        Assert.Equal(1, slots[0].DigimonId.Value);
    }

    [Fact]
    public void Diff_ShouldReturnChangedSlots_WhenSlotsChanged()
    {
        var previousSlot = new DigimonSlot { Index = 0, DigimonId = 1, Digimon = CreateBaseDigimon() };
        var previous = new Party { Slots = [previousSlot] };

        var newSlot = new DigimonSlot { Index = 0, DigimonId = 1, Digimon = CreateBaseDigimon() };
        newSlot.Digimon.Level = 15;
        var newObj = new Party { Slots = [newSlot] };

        var result = PartyDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.True(result.Slots.HasValue);


        var slots = result.Slots.Value!;
        Assert.Single(slots);
        Assert.Equal(0, slots[0].Index);
        Assert.True(slots[0].Digimon.HasValue);


        var digimon = slots[0].Digimon.Value!;
        Assert.True(digimon.Level.HasValue);
        Assert.Equal(15, digimon.Level.Value);
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
