using Backend.Memory.Addresses.Party;
using Backend.Memory.Resources.Digimon;

namespace Backend.Memory.Readers.Digimon
{
    public class DigievolutionSlotReader() : IDigievolutionSlotReader
    {
        public DigievolutionSlotResource Read(MemoryBlockReader memoryBlockReader, SlotAddresses slotAddresses)
        {
            return new DigievolutionSlotResource
            {
                Index = slotAddresses.Index,
                DigievolutionId = memoryBlockReader.ReadInt16((int)slotAddresses.Address)
            };
        }
    }
}
