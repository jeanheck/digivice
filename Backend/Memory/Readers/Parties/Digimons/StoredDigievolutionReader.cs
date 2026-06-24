using Backend.Memory.Addresses.Parties.Digimons;
using Backend.Memory.Resources.Parties.Digimons;

namespace Backend.Memory.Readers.Parties.Digimons
{
    public class StoredDigievolutionReader : IStoredDigievolutionReader
    {
        public List<StoredDigievolutionResource> Read(
            MemoryBlockReader memoryBlockReader,
            DigievolutionsAddresses digievolutionsAddresses)
        {
            var (unlockedDigievolutionsStart, unlockedDigievolutionEntryStride, maxUnlockedDigievolutions)
                = digievolutionsAddresses;

            var storedDigievolutions = new List<StoredDigievolutionResource>();

            for (int i = 0; i < maxUnlockedDigievolutions; i++)
            {
                int offset = unlockedDigievolutionsStart + (i * unlockedDigievolutionEntryStride);
                int digievolutionId = memoryBlockReader.ReadInt16(offset + digievolutionsAddresses.Id);

                if (digievolutionId <= 0)
                {
                    break;
                }

                storedDigievolutions.Add(new StoredDigievolutionResource
                {
                    DigievolutionId = digievolutionId,
                    Level = memoryBlockReader.ReadInt16(offset + digievolutionsAddresses.Level)
                });
            }

            return storedDigievolutions;
        }
    }
}
