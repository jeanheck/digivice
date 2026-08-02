namespace Tests.Events.Diffing.Parties.Digimons;

using Backend.Events.Diffing.Parties.Digimons;
using Backend.Domain.Models.Parties.Digimons;

public class VitalDifferTests
{
    [Fact]
    public void Diff_ShouldReturnNull_WhenNoChanges()
    {
        var previous = new Vital { Current = 10, Max = 20 };
        var newObj = new Vital { Current = 10, Max = 20 };

        var result = VitalDiffer.Diff(previous, newObj);

        Assert.Null(result);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var newObj = new Vital { Current = 10, Max = 20 };

        var result = VitalDiffer.Diff(null, newObj);

        Assert.NotNull(result);
        Assert.True(result.Current.HasValue);
        Assert.Equal(10, result.Current.Value);
        Assert.True(result.Max.HasValue);
        Assert.Equal(20, result.Max.Value);
    }

    [Fact]
    public void Diff_ShouldReturnOnlyChangedFields_WhenPartialChanges()
    {
        var previous = new Vital { Current = 10, Max = 20 };
        var newObj = new Vital { Current = 15, Max = 20 };

        var result = VitalDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.True(result.Current.HasValue);
        Assert.Equal(15, result.Current.Value);
        Assert.False(result.Max.HasValue);
    }
}
