using Backend.Memory.Addresses.Party;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers.Party
{
    public interface ISlotReader
    {
        SlotResource Read(SlotAddresses addresses, int bytesPerSlot);
    }
}
