using Backend.Memory.Addresses.Parties;
using Backend.Memory.Resources.Party.Digimon;

namespace Backend.Memory.Readers.Party.Digimon
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
