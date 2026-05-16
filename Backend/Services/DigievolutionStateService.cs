using Backend.Domain.Backend.Domain.Models.Digimons;
using Backend.Memory.Readers;
using Backend.Memory.Addresses;
using Backend.Memory.Addresses.Digimon;
using Backend.Utils;

namespace Backend.Services
{
    public class DigievolutionStateService
    {
        private const int EmptySlotId = -1; // 0xFFFF in memory
        private const int NoDigievolutionId = 0;

        public Digievolution[] GetDigievolutions(
            byte[] memoryBlock,
            DigievolutionsAddresses digievolutionsAddresses)
        {
            var memoryBlockReader = new MemoryBlockReader(memoryBlock);
            ReadOnlySpan<int> digievolutionsSlotsAddresses = [
                digievolutionsAddresses.Slot1,
                digievolutionsAddresses.Slot2,
                digievolutionsAddresses.Slot3
            ];
            var digievolutions = new Digievolution[3];

            for (int i = 0; i < digievolutionsSlotsAddresses.Length; i++)
            {
                int id = memoryBlockReader.ReadInt16(digievolutionsSlotsAddresses[i]);

                if (id == EmptySlotId || id == NoDigievolutionId)
                {
                    digievolutions[i] = null;
                    continue;
                }

                int level = FindDigievolutionLevel(memoryBlockReader, id, digievolutionsAddresses);
                digievolutions[i] = new Digievolution { Id = id, Level = level };
            }

            return digievolutions;
        }

        private int FindDigievolutionLevel(MemoryBlockReader blockReader, int id, DigievolutionsAddresses addresses)
        {
            for (int k = 0; k < addresses.MaxUnlockedDigievolutions; k++)
            {
                int entryOffset = addresses.UnlockedDigievolutionsStart + (k * addresses.UnlockedDigievolutionEntryStride);

                int entryId = blockReader.ReadInt16(entryOffset);
                if (entryId == id)
                {
                    return blockReader.ReadInt16(entryOffset + 2);
                }
            }

            return 1; // Default level if not found
        }
    }
}
