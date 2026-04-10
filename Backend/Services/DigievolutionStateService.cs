using Backend.Models.Digimons;
using Backend.Models.Addresses;
using Backend.Utils;

namespace Backend.Services
{
    public class DigievolutionStateService
    {
        public virtual Digievolution?[] GetEquippedDigievolutions(byte[] logicBlock, DigievolutionsOffsets? addresses)
        {
            var equippedEvoIds = new int[]
            {
                MemoryUtils.ReadInt16FromBlock(logicBlock, addresses?.EquipedSlot1),
                MemoryUtils.ReadInt16FromBlock(logicBlock, addresses?.EquipedSlot2),
                MemoryUtils.ReadInt16FromBlock(logicBlock, addresses?.EquipedSlot3)
            };

            var equippedDigievolutions = new Digievolution?[3];
            for (int j = 0; j < 3; j++)
            {
                int id = equippedEvoIds[j];
                if (id == 0xFFFF || id == -1 || id == 0)
                {
                    equippedDigievolutions[j] = null;
                    continue;
                }

                int level = 1;
                int maxEvolutions = MemoryUtils.ReadInt32OffsetSafely(addresses?.MaxUnlockedDigievolutions, 60);
                int stride = MemoryUtils.ReadInt32OffsetSafely(addresses?.UnlockedDigievolutionEntryStride, 8);
                int startOffset = MemoryUtils.ReadInt32OffsetSafely(addresses?.UnlockedDigievolutionsStart, 0x50);

                for (int k = 0; k < maxEvolutions; k++)
                {
                    int entryOffset = startOffset + (k * stride);
                    if (entryOffset + 2 >= logicBlock.Length) break;

                    int entryId = BitConverter.ToInt16(logicBlock, entryOffset);
                    if (entryId == id)
                    {
                        level = BitConverter.ToInt16(logicBlock, entryOffset + 2);
                        break;
                    }
                }

                equippedDigievolutions[j] = new Digievolution { Id = id, Level = level };
            }

            return equippedDigievolutions;
        }
    }
}
