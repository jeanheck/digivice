using Backend.Domain.Models;
using Backend.Events.DTO;

namespace Backend.Events.Converters;

public static class ImportantItemsConverter
{
    public static ImportantItemsDTO ToDTO(ImportantItems importantItems)
    {
        return new ImportantItemsDTO
        {
            TreeBoots = importantItems.TreeBoots,
            FishingPole = importantItems.FishingPole,
            AsukaTrophy = importantItems.AsukaTrophy,
            SunTrophy = importantItems.SunTrophy
        };
    }
}
