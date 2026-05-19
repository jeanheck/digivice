using Backend.Memory.Addresses.Parties;
using Backend.Memory.Resources.Party;

namespace Backend.Memory.Readers.Party
{
    public interface IDigimonSlotReader
    {
        DigimonSlotResource Read(SlotAddresses addresses, int bytesPerSlot);
    }
}
