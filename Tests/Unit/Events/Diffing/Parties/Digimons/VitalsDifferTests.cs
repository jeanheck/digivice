namespace Tests.Events.Diffing.Parties.Digimons;

using Backend.Events.Diffing.Parties.Digimons;
using Backend.Domain.Models.Parties.Digimons;

public class VitalsDifferTests
{
    [Fact]
    public void Diff_ShouldReturnNull_WhenNoChanges()
    {
        var previous = new Vitals { CurrentHP = 10, MaxHP = 20, CurrentMP = 5, MaxMP = 10 };
        var newObj = new Vitals { CurrentHP = 10, MaxHP = 20, CurrentMP = 5, MaxMP = 10 };

        var result = VitalsDiffer.Diff(previous, newObj);

        Assert.Null(result);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var newObj = new Vitals { CurrentHP = 10, MaxHP = 20, CurrentMP = 5, MaxMP = 10 };

        var result = VitalsDiffer.Diff(null, newObj);

        Assert.NotNull(result);
        Assert.True(result.CurrentHP.HasValue);
        Assert.Equal(10, result.CurrentHP.Value);
        Assert.True(result.MaxHP.HasValue);
        Assert.Equal(20, result.MaxHP.Value);
        Assert.True(result.CurrentMP.HasValue);
        Assert.Equal(5, result.CurrentMP.Value);
        Assert.True(result.MaxMP.HasValue);
        Assert.Equal(10, result.MaxMP.Value);
    }

    [Fact]
    public void Diff_ShouldReturnOnlyChangedFields_WhenPartialChanges()
    {
        var previous = new Vitals { CurrentHP = 10, MaxHP = 20, CurrentMP = 5, MaxMP = 10 };
        var newObj = new Vitals { CurrentHP = 15, MaxHP = 20, CurrentMP = 5, MaxMP = 12 };

        var result = VitalsDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.True(result.CurrentHP.HasValue);
        Assert.Equal(15, result.CurrentHP.Value);
        Assert.False(result.MaxHP.HasValue);
        Assert.False(result.CurrentMP.HasValue);
        Assert.True(result.MaxMP.HasValue);
        Assert.Equal(12, result.MaxMP.Value);
    }
}
