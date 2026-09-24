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
        var previous = CreateParty();
        var newParty = CreateParty();

        var result = PartyDiffer.Diff(previous, newParty);

        Assert.NotNull(result);
        Assert.False(result.Slots.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var newParty = CreateParty();

        var result = PartyDiffer.Diff(null, newParty);

        Assert.NotNull(result);
        Assert.True(result.Slots.HasValue);

        var slots = result.Slots.Value!;
        Assert.Equal(3, slots.Count);
        Assert.Equal(1, slots[0].Index);
        Assert.True(slots[0].DigimonId.HasValue);
        Assert.Equal(1, slots[0].DigimonId.Value);
        Assert.True(slots[1].DigimonId.HasValue);
        Assert.Null(slots[1].DigimonId.Value);
        Assert.True(slots[2].DigimonId.HasValue);
        Assert.Null(slots[2].DigimonId.Value);
    }

    [Fact]
    public void Diff_ShouldReturnOnlyChangedSlot_WhenOneSlotChanged()
    {
        var previous = CreateParty();
        var newParty = CreateParty();
        newParty.Slots[0].Digimon!.Level = 15;

        var result = PartyDiffer.Diff(previous, newParty);

        Assert.NotNull(result);
        Assert.True(result.Slots.HasValue);

        var slot = Assert.Single(result.Slots.Value!);
        Assert.Equal(1, slot.Index);
        Assert.True(slot.Digimon.HasValue);

        var digimon = slot.Digimon.Value!;
        Assert.True(digimon.Level.HasValue);
        Assert.Equal(15, digimon.Level.Value);
        Assert.False(digimon.Experience.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnJoinedSlot_WhenDigimonJoinsEmptySlot()
    {
        var previous = CreateParty();
        var newParty = CreateParty();
        newParty.Slots[1] = new DigimonSlot { Index = 2, DigimonId = 2, Digimon = CreateBaseDigimon() };

        var result = PartyDiffer.Diff(previous, newParty);

        var slot = Assert.Single(result.Slots.Value!);
        Assert.Equal(2, slot.Index);
        Assert.Equal(2, slot.DigimonId.Value);
        Assert.True(slot.Digimon.HasValue);
        Assert.NotNull(slot.Digimon.Value);
    }

    private static Party CreateParty()
    {
        return new Party
        {
            Slots =
            [
                new DigimonSlot { Index = 1, DigimonId = 1, Digimon = CreateBaseDigimon() },
                new DigimonSlot { Index = 2, DigimonId = null, Digimon = null },
                new DigimonSlot { Index = 3, DigimonId = null, Digimon = null }
            ]
        };
    }

    private static Digimon CreateBaseDigimon()
    {
        return new Digimon
        {
            Level = 10,
            Experience = 1000,
            ActiveDigievolutionId = 3,
            HP = new Vital { Current = 100, Max = 100 },
            MP = new Vital { Current = 50, Max = 50 },
            Attributes = new Attributes { Strength = 5, Defense = 5, Spirit = 5, Wisdom = 5, Speed = 5, Charisma = 5 },
            Resistances = new Resistances { Fire = 1, Water = 1, Ice = 1, Wind = 1, Thunder = 1, Machine = 1, Dark = 1 },
            Equipments = new Equipments(),
            Digievolutions = []
        };
    }
}
