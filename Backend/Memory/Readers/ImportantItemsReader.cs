using Backend.Memory.Addresses;
using Backend.Memory.Resources;
using Backend.Memory.Readers.Interfaces;

namespace Backend.Memory.Readers
{
    public class ImportantItemsReader(IMemoryReader memoryReader) : IImportantItemsReader
    {
        public ImportantItemsResource Read(ImportantItemsAddresses addresses)
        {
            return new ImportantItemsResource
            {
                TreeBoots = memoryReader.ReadByte(addresses.TreeBoots),
                FishingPole = memoryReader.ReadByte(addresses.FishingPole),
                AsukaTrophy = memoryReader.ReadByte(addresses.AsukaTrophy),
                SunTrophy = memoryReader.ReadByte(addresses.SunTrophy)
            };
        }
    }
}
