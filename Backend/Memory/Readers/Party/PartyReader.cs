using Backend.Memory.Addresses;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers.Party
{
    public class PartyReader(IDigimonSlotReader slotReader) : IPartyReader
    {
        public PartyResource Read(PartyAddresses addresses)
        {
            return new PartyResource
            {
                SlotsResource = [.. addresses.Slots.Select(slot => slotReader.Read(slot, addresses.BytesPerSlot))]
            };
        }
    }
}
