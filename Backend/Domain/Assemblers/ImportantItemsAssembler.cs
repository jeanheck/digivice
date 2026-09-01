using Backend.Domain.Models;
using Backend.Memory.Resources;

namespace Backend.Domain.Assemblers
{
    public static class ImportantItemsAssembler
    {
        public static ImportantItems Assemble(ImportantItemsResource resource)
        {
            return new ImportantItems
            {
                TreeBoots = (resource.TreeBoots ?? 0) != 0,
                FishingPole = (resource.FishingPole ?? 0) != 0,
                AsukaTrophy = (resource.AsukaTrophy ?? 0) != 0,
                SunTrophy = (resource.SunTrophy ?? 0) != 0
            };
        }
    }
}
