using Backend.Models.Digimons;
using Backend.Models.Addresses;
using Backend.Utils;

namespace Backend.Services
{
    public class DigievolutionStateService
    {
        public Digievolution[] GetEquippedDigievolutions(byte[] logicBlock, DigievolutionsAddresses digievolutionsAddresses)
        {
            ReadOnlySpan<int> digievolutionsEquippedSlotsAddresses = [
                digievolutionsAddresses.EquipedSlot1,
                digievolutionsAddresses.EquipedSlot2,
                digievolutionsAddresses.EquipedSlot3
            ];
            var equippedDigievolutions = new Digievolution[3];

            for (int i = 0; i < digievolutionsEquippedSlotsAddresses.Length; i++)
            {
                int id = MemoryUtils.ReadInt16FromBlock(logicBlock, digievolutionsEquippedSlotsAddresses[i]);

                // Redundant check for item 1/3 (will be handled separately)
                if (id == 0xFFFF || id == -1 || id == 0)
                {
                    equippedDigievolutions[i] = null;
                    continue;
                }

                int level = FindDigievolutionLevel(logicBlock, id, digievolutionsAddresses);
                equippedDigievolutions[i] = new Digievolution { Id = id, Level = level };
            }

            return equippedDigievolutions;
        }

        private static int FindDigievolutionLevel(byte[] logicBlock, int id, DigievolutionsAddresses addresses)
        {
            for (int k = 0; k < addresses.MaxUnlockedDigievolutions; k++)
            {
                int entryOffset = addresses.UnlockedDigievolutionsStart + (k * addresses.UnlockedDigievolutionEntryStride);
                if (entryOffset + 2 >= logicBlock.Length) break;

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
