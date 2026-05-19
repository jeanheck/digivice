using Backend.Memory.Resources.Parties.Digimons;
using Backend.Memory.Addresses.Parties;

namespace Backend.Memory.Readers.Parties.Digimons
{
    public interface IDigievolutionSlotReader
    {
        DigievolutionSlotResource Read(MemoryBlockReader memoryBlockReader, SlotAddresses slotAddresses);
    }
}
