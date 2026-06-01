using Backend.Memory.Addresses.Parties;
using Backend.Memory.Resources.Parties.Digimons;

namespace Backend.Memory.Readers.Parties.Digimons
{
    public class DigievolutionSlotReader() : IDigievolutionSlotReader
    {
        public DigievolutionSlotResource Read(MemoryBlockReader memoryBlockReader, SlotAddresses slotAddresses)
        {
            int digievolutionId = memoryBlockReader.ReadInt16((int)slotAddresses.Address);

            return new DigievolutionSlotResource
            {
                Index = slotAddresses.Index,
                DigievolutionId = digievolutionId > 0 ? digievolutionId : null
            };
        }
    }
}
