namespace Tests.Events.Diffing.Parties.Digimons;

using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Diffing.Parties.Digimons;

public class DigimonInCombatDifferTests
{
    [Fact]
    public void Diff_ShouldReturnNull_WhenNoChanges()
    {
        var previous = CreateBaseDigimonInCombat();
        var newObj = CreateBaseDigimonInCombat();

        var result = DigimonInCombatDiffer.Diff(previous, newObj);

        Assert.Null(result);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var newObj = CreateBaseDigimonInCombat();
        newObj.Condition = 0x04;

        var result = DigimonInCombatDiffer.Diff(null, newObj);

        Assert.NotNull(result);
        Assert.Equal(0x04, result.Condition.Value);
        Assert.Equal(100, result.HP.Value!.Current.Value);
    }

    [Fact]
    public void Diff_ShouldReturnOnlyChangedFields_WhenPartialChanges()
    {
        var previous = CreateBaseDigimonInCombat();
        var newObj = CreateBaseDigimonInCombat();
        newObj.Condition = 0x01;
        newObj.HP.Current = 50;

        var result = DigimonInCombatDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.Equal(0x01, result.Condition.Value);
        Assert.Equal(50, result.HP.Value!.Current.Value);
        Assert.False(result.HP.Value.Max.HasValue);
        Assert.False(result.MP.HasValue);
    }

    private static DigimonInCombat CreateBaseDigimonInCombat()
    {
        return new DigimonInCombat
        {
            Condition = 0,
            HP = new Vital { Current = 100, Max = 100 },
            MP = new Vital { Current = 50, Max = 50 }
        };
    }
}
