using Backend.Memory.Addresses.Party;
using Backend.Memory.Resources.Party;

namespace Backend.Memory.Readers.Party
{
    public interface IDigimonSlotReader
    {
        DigimonSlotResource Read(SlotAddresses addresses, int bytesPerSlot);
    }
}
