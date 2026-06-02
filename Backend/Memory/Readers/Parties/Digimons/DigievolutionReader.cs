using Backend.Memory.Addresses.Parties.Digimons;
using Backend.Memory.Resources.Parties.Digimons;

namespace Backend.Memory.Readers.Parties.Digimons
{
    public class DigievolutionReader : IDigievolutionReader
    {
        public DigievolutionResource Read(
            MemoryBlockReader memoryBlockReader,
            int digievolutionId,
            DigievolutionsAddresses digievolutionsAddresses)
        {
            return new DigievolutionResource()
            {
                Level = ReadLevel(memoryBlockReader, digievolutionId, digievolutionsAddresses)
            };
        }

        private static int ReadLevel(
            MemoryBlockReader blockReader,
            int digievolutionId,
            DigievolutionsAddresses digievolutionsAddresses)
        {
            var (unlockedDigievolutionsStart, unlockedDigievolutionEntryStride, maxUnlockedDigievolutions)
                = digievolutionsAddresses;

            for (int i = 0; i < maxUnlockedDigievolutions; i++)
            {
                int offset = unlockedDigievolutionsStart + (i * unlockedDigievolutionEntryStride);
                int entryDigievolutionId = blockReader.ReadInt16(offset + digievolutionsAddresses.Id);
                if (entryDigievolutionId == digievolutionId)
                {
                    return blockReader.ReadInt16(offset + digievolutionsAddresses.Level);
                }
            }

            return 1;
        }
    }
}
