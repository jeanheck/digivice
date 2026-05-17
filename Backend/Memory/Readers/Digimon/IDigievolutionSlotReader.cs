using Backend.Memory.Resources.Party.Digimon;
using Backend.Memory.Addresses.Party;

namespace Backend.Memory.Readers.Digimon
{
    public interface IDigievolutionSlotReader
    {
        DigievolutionSlotResource Read(MemoryBlockReader memoryBlockReader, SlotAddresses slotAddresses);
    }
}
