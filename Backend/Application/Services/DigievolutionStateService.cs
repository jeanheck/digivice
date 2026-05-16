using Backend.Domain.Models.Digimons;
using Backend.Memory.Readers;
using Backend.Memory.Addresses.Digimon;
using Backend.Memory.Parsing;

namespace Backend.Application.Services
{
    public class DigievolutionStateService
    {
        private const int EmptySlotId = -1; // 0xFFFF in memory
        private const int NoDigievolutionId = 0;

        public Digievolution[] GetDigievolutions(MemoryBlockReader memoryBlockReader, DigievolutionsAddresses digievolutionsAddresses)
        {
            ReadOnlySpan<int> digievolutionsSlotsAddresses = [
                digievolutionsAddresses.Slot1,
                digievolutionsAddresses.Slot2,
                digievolutionsAddresses.Slot3
            ];
            var digievolutions = new Digievolution[3];

            for (int i = 0; i < digievolutionsSlotsAddresses.Length; i++)
            {
                int digievolutionId = memoryBlockReader.ReadInt16(digievolutionsSlotsAddresses[i]);

                if (digievolutionId == EmptySlotId || digievolutionId == NoDigievolutionId)
                {
                    digievolutions[i] = null;
                    continue;
                }

                digievolutions[i] = new Digievolution
                {
                    Id = digievolutionId,
                    Level = DigievolutionMemoryBlockParser
                        .FindLevel(memoryBlockReader, digievolutionId, digievolutionsAddresses)
                };
            }

            return digievolutions;
        }
    }
}
