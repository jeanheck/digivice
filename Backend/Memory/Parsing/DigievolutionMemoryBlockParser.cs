using Backend.Memory.Readers;
using Backend.Memory.Addresses.Digimon;

namespace Backend.Memory.Parsing
{
    public static class DigievolutionMemoryBlockParser
    {
        public static int FindLevel(
            MemoryBlockReader blockReader,
            int digievolutionId,
            DigievolutionsAddresses digievolutionsAddresses)
        {
            var (unlockedDigievolutionsStart, unlockedDigievolutionEntryStride, maxUnlockedDigievolutions)
                = digievolutionsAddresses;

            for (int i = 0; i < maxUnlockedDigievolutions; i++)
            {
                int offset = unlockedDigievolutionsStart + (i * unlockedDigievolutionEntryStride);
                int entryDigievolutionId = blockReader.ReadInt16(offset);
                if (entryDigievolutionId == digievolutionId)
                {
                    return blockReader.ReadInt16(offset + 2);
                }
            }

            return 1;
        }
    }
}
