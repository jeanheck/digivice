using Backend.Domain.Models;
using Backend.Events.Converters;
using Backend.Events.Diffing.Extensions;
using Backend.Events.DTO;

namespace Backend.Events.Diffing;

public static class ImportantItemsDiffer
{
    public static ImportantItemsDTO Diff(ImportantItems? previousImportantItems, ImportantItems newImportantItems)
    {
        if (newImportantItems.HasNoChanges(previousImportantItems))
        {
            return new ImportantItemsDTO();
        }

        if (previousImportantItems == null)
        {
            return ImportantItemsConverter.ToDTO(newImportantItems);
        }

        var dto = new ImportantItemsDTO();
        if (newImportantItems.TreeBoots != previousImportantItems.TreeBoots)
        {
            dto = dto with { TreeBoots = newImportantItems.TreeBoots };
        }
        if (newImportantItems.FishingPole != previousImportantItems.FishingPole)
        {
            dto = dto with { FishingPole = newImportantItems.FishingPole };
        }
        if (newImportantItems.AsukaTrophy != previousImportantItems.AsukaTrophy)
        {
            dto = dto with { AsukaTrophy = newImportantItems.AsukaTrophy };
        }

        return dto;
    }
}
