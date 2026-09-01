using Backend.Memory.Addresses;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers
{
    public class ImportantItemsReader(IMemoryReader memoryReader) : IImportantItemsReader
    {
        public ImportantItemsResource Read(ImportantItemsAddresses addresses)
        {
            return new ImportantItemsResource
            {
                TreeBoots = memoryReader.ReadBytes(addresses.TreeBoots, 1)[0],
                FishingPole = memoryReader.ReadBytes(addresses.FishingPole, 1)[0],
                AsukaTrophy = memoryReader.ReadBytes(addresses.AsukaTrophy, 1)[0],
                SunTrophy = memoryReader.ReadBytes(addresses.SunTrophy, 1)[0]
            };
        }
    }
}
