using Backend.Memory.Addresses.Party.Digimon;
using Backend.Memory.Resources.Digimon;

namespace Backend.Memory.Readers.Digimon
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
