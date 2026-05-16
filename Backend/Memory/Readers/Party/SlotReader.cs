using Backend.Memory.Addresses.Party;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers.Party
{
    public class SlotReader(IMemoryReader memoryReader) : ISlotReader
    {
        public SlotResource Read(SlotAddresses addresses, int bytesPerSlot)
        {
            var bytes = memoryReader.ReadBytes(addresses.Address, bytesPerSlot);
            var digimonId = (bytes != null && bytes.Length > 0) ? (int)bytes[0] : 0;

            return new SlotResource
            {
                Index = addresses.Index,
                DigimonId = digimonId
            };
        }
    }
}
