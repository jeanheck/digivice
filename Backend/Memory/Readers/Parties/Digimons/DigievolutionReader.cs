using Backend.Memory.Addresses.Parties.Digimons;
using Backend.Memory.Resources.Parties.Digimons;

namespace Backend.Memory.Readers.Parties.Digimons
{
    public class DigievolutionReader : IDigievolutionReader
    {
        private const int DefaultLevelWhenNotFound = 1;
        private const int DefaultDvxpWhenNotFound = 0;

        public DigievolutionResource Read(
            MemoryBlockReader memoryBlockReader,
            int digievolutionId,
            DigievolutionsAddresses digievolutionsAddresses)
        {
            var entryOffset = FindEntryOffset(memoryBlockReader, digievolutionId, digievolutionsAddresses);

            if (entryOffset is null)
            {
                return new DigievolutionResource
                {
                    Level = DefaultLevelWhenNotFound,
                    Dvxp = DefaultDvxpWhenNotFound
                };
            }

            return new DigievolutionResource
            {
                Level = memoryBlockReader.ReadInt16(entryOffset.Value + digievolutionsAddresses.Level),
                Dvxp = memoryBlockReader.ReadInt32(entryOffset.Value + digievolutionsAddresses.Dvxp)
            };
        }

        private static int? FindEntryOffset(
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
                    return offset;
                }
            }

            return null;
        }
    }
}
