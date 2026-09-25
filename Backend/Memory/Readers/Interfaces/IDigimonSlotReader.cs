using Backend.Memory.Addresses.Parties;
using Backend.Memory.Resources.Parties;

namespace Backend.Memory.Readers.Interfaces
{
    public interface IDigimonSlotReader
    {
        DigimonSlotResource Read(SlotAddresses addresses, int bytesPerSlot);
    }
}
