namespace Tests.Events.Converters;

using Backend.Domain.Models;
using Backend.Events.Converters;

public class ImportantItemsConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapAllImportantItemsFields()
    {
        var importantItems = new ImportantItems
        {
            TreeBoots = true,
            FishingPole = false,
            AsukaTrophy = true
        };

        var dto = ImportantItemsConverter.ToDTO(importantItems);

        Assert.True(dto.TreeBoots.HasValue);
        Assert.True(dto.TreeBoots.Value);
        Assert.True(dto.FishingPole.HasValue);
        Assert.False(dto.FishingPole.Value);
        Assert.True(dto.AsukaTrophy.HasValue);
        Assert.True(dto.AsukaTrophy.Value);
    }
}
