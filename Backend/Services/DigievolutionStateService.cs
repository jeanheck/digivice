using Backend.Models.Digimons;
using Backend.Models.Addresses;
using Backend.Utils;

namespace Backend.Services
{
    public class DigievolutionStateService
    {
        private const int EmptySlotId = -1; // 0xFFFF in memory
        private const int NoDigievolutionId = 0;

        public Digievolution[] GetDigievolutions(
            byte[] logicBlock,
            DigievolutionsAddresses digievolutionsAddresses)
        {
            ReadOnlySpan<int> digievolutionsSlotsAddresses = [
                digievolutionsAddresses.Slot1,
                digievolutionsAddresses.Slot2,
                digievolutionsAddresses.Slot3
            ];
            var digievolutions = new Digievolution[3];

            for (int i = 0; i < digievolutionsSlotsAddresses.Length; i++)
            {
                int id = MemoryUtils.ReadInt16FromBlock(logicBlock, digievolutionsSlotsAddresses[i]);

                if (id == EmptySlotId || id == NoDigievolutionId)
                {
                    digievolutions[i] = null;
                    continue;
                }

                int level = FindDigievolutionLevel(logicBlock, id, digievolutionsAddresses);
                digievolutions[i] = new Digievolution { Id = id, Level = level };
            }

            return digievolutions;
        }

        private static int FindDigievolutionLevel(byte[] logicBlock, int id, DigievolutionsAddresses addresses)
        {
            for (int k = 0; k < addresses.MaxUnlockedDigievolutions; k++)
            {
                int entryOffset = addresses.UnlockedDigievolutionsStart + (k * addresses.UnlockedDigievolutionEntryStride);
                if (entryOffset + 4 > logicBlock.Length) break;

                int entryId = MemoryUtils.ReadInt16FromBlock(logicBlock, entryOffset);
                if (entryId == id)
                {
                    return MemoryUtils.ReadInt16FromBlock(logicBlock, entryOffset + 2);
                }
            }

            return 1; // Default level if not found
        }
    }
}
